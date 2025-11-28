using Core.Entities;

namespace Core.Interfaces
{
    public interface IServicioService
    {
        Task<IEnumerable<Servicio>> GetAllServiciosAsync();
        Task<IEnumerable<Servicio>> GetServiciosActivosAsync();
        Task<Servicio> GetServicioByIdAsync(int id);
        Task<Servicio> CreateServicioAsync(Servicio servicio);
        Task<Servicio> UpdateServicioAsync(Servicio servicio);
        Task<Servicio> UpdatePrecioAsync(int id, decimal nuevoPrecio);
    }
}
