using Core.Entities;

namespace Core.Interfaces
{
    public interface IPagoService
    {
        Task<Pago> RegistrarPagoAsync(Pago pago);
        Task<IEnumerable<Pago>> GetPagosByCitaAsync(int citaId);
        Task<decimal> GetTotalPagosByFechaAsync(DateOnly fecha);
        Task<Dictionary<string, decimal>> GetReportePagosByPeriodoAsync(DateTime inicio, DateTime fin);
    }
}
