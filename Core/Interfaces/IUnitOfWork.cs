

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICita Citas { get; }
        ICliente Clientes { get; }
        IPago Pagos { get; }
        IUsuario Usuarios { get; }
        IServicio Servicios { get; }
        Task<int> SaveAsync();
    }
}