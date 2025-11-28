using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CitaService : ICitaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cita>> GetAllCitasAsync()
        {
            return await _unitOfWork.Citas.GetAllAsync();
        }

        public async Task<Cita> GetCitaByIdAsync(int id)
        {
            var cita = await _unitOfWork.Citas.GetByIdAsync(id);
            if (cita == null)
                throw new KeyNotFoundException($"Cita con ID {id} no encontrada");
            
            return cita;
        }

        public async Task<Cita> CreateCitaAsync(Cita cita, List<int> serviciosIds)
        {
            // Validaciones básicas
            if (cita.IdCliente <= 0)
                throw new ArgumentException("Debe especificar un cliente válido");
            
            if (cita.IdUsuario <= 0)
                throw new ArgumentException("Debe especificar un usuario/especialista válido");
            
            if (serviciosIds == null || !serviciosIds.Any())
                throw new ArgumentException("Debe seleccionar al menos un servicio"); // RF A3

            // Validar que el cliente existe
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(cita.IdCliente);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente con ID {cita.IdCliente} no encontrado");

            // Validar que el usuario existe
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(cita.IdUsuario);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con ID {cita.IdUsuario} no encontrado");

            // Obtener los servicios seleccionados
            var servicios = new List<Servicio>();
            decimal totalPrecio = 0;
            int totalDuracion = 0;

            foreach (var servicioId in serviciosIds)
            {
                var servicio = await _unitOfWork.Servicios.GetByIdAsync(servicioId);
                if (servicio == null)
                    throw new KeyNotFoundException($"Servicio con ID {servicioId} no encontrado");
                
                if (servicio.Activo != true)
                    throw new InvalidOperationException($"El servicio '{servicio.NombreServicio}' no está activo");

                servicios.Add(servicio);
                totalPrecio += servicio.Precio;
                totalDuracion += servicio.DuracionEstimadaMin; // RF A8: Suma de duraciones
            }

            // Calcular hora de fin basada en la duración total (RF A8)
            var horaInicioDateTime = cita.FechaCita.ToDateTime(cita.HoraInicio);
            var horaFinDateTime = horaInicioDateTime.AddMinutes(totalDuracion);
            cita.HoraFin = TimeOnly.FromDateTime(horaFinDateTime);

            // Validar disponibilidad (RF A5, A6)
            var disponible = await ValidarDisponibilidadAsync(
                cita.IdUsuario, 
                cita.FechaCita, 
                cita.HoraInicio, 
                cita.HoraFin
            );

            if (!disponible)
            {
                throw new InvalidOperationException(
                    $"La especialista ya tiene una cita asignada en el horario {cita.HoraInicio} - {cita.HoraFin}. " +
                    "Por favor seleccione otro horario."
                );
            }

            // Establecer valores calculados
            cita.TotalPrecio = totalPrecio;
            cita.TotalDuracionMin = totalDuracion;
            cita.Estado = cita.Estado ?? "Pendiente"; // RF D1
            cita.CreatedAt = DateTime.Now;
            cita.UpdatedAt = DateTime.Now;

            // Crear la cita
            _unitOfWork.Citas.Add(cita);
            await _unitOfWork.SaveAsync();

            // Asociar servicios a la cita
            foreach (var servicio in servicios)
            {
                var citaServicio = new CitaServicio
                {
                    IdCita = cita.Id,
                    IdServicio = servicio.Id,
                    PrecioUnitarioMomento = servicio.Precio // Guardar precio en el momento de la cita
                };
                
                _unitOfWork.Citas.Add(cita); // Esto agregará la relación
            }

            await _unitOfWork.SaveAsync();
            
            return cita;
        }

        public async Task<Cita> UpdateEstadoCitaAsync(int id, string nuevoEstado)
        {
            var cita = await _unitOfWork.Citas.GetByIdAsync(id);
            if (cita == null)
                throw new KeyNotFoundException($"Cita con ID {id} no encontrada");

            // Validar estados permitidos (RF D1)
            var estadosPermitidos = new[] { "Pendiente", "Confirmada", "En Curso", "Finalizada", "Cancelada", "Reprogramada" };
            if (!estadosPermitidos.Contains(nuevoEstado))
                throw new ArgumentException($"Estado '{nuevoEstado}' no es válido");

            cita.Estado = nuevoEstado;
            cita.UpdatedAt = DateTime.Now;

            _unitOfWork.Citas.Update(cita);
            await _unitOfWork.SaveAsync();
            
            return cita;
        }

        public async Task<bool> ValidarDisponibilidadAsync(
            int usuarioId, 
            DateOnly fecha, 
            TimeOnly horaInicio, 
            TimeOnly horaFin, 
            int? citaIdExcluir = null)
        {
            // RF A6: Una especialista no puede tener dos citas en el mismo rango de hora
            // Lógica: NuevaCita.Inicio < CitaExistente.Fin && NuevaCita.Fin > CitaExistente.Inicio
            
            var citasExistentes = _unitOfWork.Citas
                .Find(c => 
                    c.IdUsuario == usuarioId && 
                    c.FechaCita == fecha &&
                    c.Estado != "Cancelada" && // No considerar citas canceladas
                    (citaIdExcluir == null || c.Id != citaIdExcluir) // Excluir una cita específica (útil para reprogramar)
                )
                .ToList();

            foreach (var citaExistente in citasExistentes)
            {
                // Detectar solapamiento de horarios
                if (horaInicio < citaExistente.HoraFin && horaFin > citaExistente.HoraInicio)
                {
                    return false; // Hay conflicto de horario
                }
            }

            return true; // Está disponible
        }

        public async Task<IEnumerable<Cita>> GetCitasByFechaAsync(DateOnly fecha)
        {
            var citas = _unitOfWork.Citas
                .Find(c => c.FechaCita == fecha)
                .OrderBy(c => c.HoraInicio);

            return citas;
        }

        public async Task<IEnumerable<Cita>> GetCitasByEstadoAsync(string estado)
        {
            var citas = _unitOfWork.Citas
                .Find(c => c.Estado == estado)
                .OrderBy(c => c.FechaCita)
                .ThenBy(c => c.HoraInicio);

            return citas;
        }
    }
}
