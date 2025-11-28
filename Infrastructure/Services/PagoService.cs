using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pago> RegistrarPagoAsync(Pago pago)
        {
            // Validaciones
            if (pago.IdCita <= 0)
                throw new ArgumentException("Debe especificar una cita válida");
            
            if (pago.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0");
            
            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
                throw new ArgumentException("Debe especificar un método de pago");

            // Validar que la cita existe
            var cita = await _unitOfWork.Citas.GetByIdAsync(pago.IdCita);
            if (cita == null)
                throw new KeyNotFoundException($"Cita con ID {pago.IdCita} no encontrada");

            // RF D3: Validar métodos de pago permitidos
            var metodosPermitidos = new[] { "Efectivo", "Transferencia", "NEQUI", "DaviPlata", "Tarjeta" };
            if (!metodosPermitidos.Contains(pago.MetodoPago))
                throw new ArgumentException($"Método de pago '{pago.MetodoPago}' no es válido");

            pago.FechaPago = DateTime.Now;

            _unitOfWork.Pagos.Add(pago);
            await _unitOfWork.SaveAsync();
            
            return pago;
        }

        public async Task<IEnumerable<Pago>> GetPagosByCitaAsync(int citaId)
        {
            var cita = await _unitOfWork.Citas.GetByIdAsync(citaId);
            if (cita == null)
                throw new KeyNotFoundException($"Cita con ID {citaId} no encontrada");

            var pagos = _unitOfWork.Pagos
                .Find(p => p.IdCita == citaId)
                .OrderBy(p => p.FechaPago);

            return pagos;
        }

        public async Task<decimal> GetTotalPagosByFechaAsync(DateOnly fecha)
        {
            // Convertir DateOnly a rango DateTime para la comparación
            var fechaInicio = fecha.ToDateTime(TimeOnly.MinValue);
            var fechaFin = fecha.ToDateTime(TimeOnly.MaxValue);

            var pagos = _unitOfWork.Pagos
                .Find(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin);

            return pagos.Sum(p => p.Monto);
        }

        public async Task<Dictionary<string, decimal>> GetReportePagosByPeriodoAsync(DateTime inicio, DateTime fin)
        {
            // RF D4: Reportes diarios, semanales y mensuales
            var pagos = _unitOfWork.Pagos
                .Find(p => p.FechaPago >= inicio && p.FechaPago <= fin)
                .ToList();

            var reporte = new Dictionary<string, decimal>
            {
                { "TotalVentas", pagos.Sum(p => p.Monto) },
                { "Efectivo", pagos.Where(p => p.MetodoPago == "Efectivo").Sum(p => p.Monto) },
                { "Transferencia", pagos.Where(p => p.MetodoPago == "Transferencia").Sum(p => p.Monto) },
                { "NEQUI", pagos.Where(p => p.MetodoPago == "NEQUI").Sum(p => p.Monto) },
                { "DaviPlata", pagos.Where(p => p.MetodoPago == "DaviPlata").Sum(p => p.Monto) },
                { "Tarjeta", pagos.Where(p => p.MetodoPago == "Tarjeta").Sum(p => p.Monto) },
                { "TotalPagos", pagos.Count }
            };

            return reporte;
        }
    }
}
