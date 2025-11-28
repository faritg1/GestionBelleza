using Core.Entities;

namespace Core.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllCitasAsync();
        Task<Cita> GetCitaByIdAsync(int id);
        Task<Cita> CreateCitaAsync(Cita cita, List<int> serviciosIds);
        Task<Cita> UpdateEstadoCitaAsync(int id, string nuevoEstado);
        Task<bool> ValidarDisponibilidadAsync(int usuarioId, DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, int? citaIdExcluir = null);
        Task<IEnumerable<Cita>> GetCitasByFechaAsync(DateOnly fecha);
        Task<IEnumerable<Cita>> GetCitasByEstadoAsync(string estado);
    }
}
