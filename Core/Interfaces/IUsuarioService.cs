using Core.Entities;

namespace Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task<Usuario> GetUsuarioByEmailAsync(string email);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario, string password);
        Task<bool> ValidarCredencialesAsync(string email, string password);
        Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo);
    }
}
