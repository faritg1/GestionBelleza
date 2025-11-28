using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Servicio>> GetAllServiciosAsync()
        {
            return await _unitOfWork.Servicios.GetAllAsync();
        }

        public async Task<IEnumerable<Servicio>> GetServiciosActivosAsync()
        {
            var servicios = _unitOfWork.Servicios
                .Find(s => s.Activo == true)
                .OrderBy(s => s.Categoria)
                .ThenBy(s => s.NombreServicio);

            return servicios;
        }

        public async Task<Servicio> GetServicioByIdAsync(int id)
        {
            var servicio = await _unitOfWork.Servicios.GetByIdAsync(id);
            if (servicio == null)
                throw new KeyNotFoundException($"Servicio con ID {id} no encontrado");
            
            return servicio;
        }

        public async Task<Servicio> CreateServicioAsync(Servicio servicio)
        {
            // Validaciones de negocio (RF C1, C2)
            if (string.IsNullOrWhiteSpace(servicio.NombreServicio))
                throw new ArgumentException("El nombre del servicio es obligatorio");
            
            if (servicio.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");
            
            if (servicio.DuracionEstimadaMin <= 0)
                throw new ArgumentException("La duración estimada debe ser mayor a 0");

            servicio.Activo = servicio.Activo ?? true;

            _unitOfWork.Servicios.Add(servicio);
            await _unitOfWork.SaveAsync();
            
            return servicio;
        }

        public async Task<Servicio> UpdateServicioAsync(Servicio servicio)
        {
            var servicioExistente = await _unitOfWork.Servicios.GetByIdAsync(servicio.Id);
            if (servicioExistente == null)
                throw new KeyNotFoundException($"Servicio con ID {servicio.Id} no encontrado");

            if (servicio.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");
            
            if (servicio.DuracionEstimadaMin <= 0)
                throw new ArgumentException("La duración estimada debe ser mayor a 0");

            servicioExistente.NombreServicio = servicio.NombreServicio;
            servicioExistente.Descripcion = servicio.Descripcion;
            servicioExistente.Precio = servicio.Precio;
            servicioExistente.DuracionEstimadaMin = servicio.DuracionEstimadaMin;
            servicioExistente.Categoria = servicio.Categoria;
            servicioExistente.Activo = servicio.Activo;

            _unitOfWork.Servicios.Update(servicioExistente);
            await _unitOfWork.SaveAsync();
            
            return servicioExistente;
        }

        public async Task<Servicio> UpdatePrecioAsync(int id, decimal nuevoPrecio)
        {
            // RF C3: El administrador puede actualizar los precios en cualquier momento
            var servicio = await _unitOfWork.Servicios.GetByIdAsync(id);
            if (servicio == null)
                throw new KeyNotFoundException($"Servicio con ID {id} no encontrado");

            if (nuevoPrecio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            servicio.Precio = nuevoPrecio;
            
            _unitOfWork.Servicios.Update(servicio);
            await _unitOfWork.SaveAsync();
            
            return servicio;
        }
    }
}
