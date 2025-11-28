using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");
            
            return usuario;
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            var usuario = _unitOfWork.Usuarios
                .Find(u => u.CorreoElectronico == email)
                .FirstOrDefault();
            
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con email {email} no encontrado");
            
            return usuario;
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario, string password)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                throw new ArgumentException("El nombre completo es obligatorio");
            
            if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico))
                throw new ArgumentException("El correo electrónico es obligatorio");
            
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña es obligatoria");

            // Validar email único
            var usuarioExistente = _unitOfWork.Usuarios
                .Find(u => u.CorreoElectronico == usuario.CorreoElectronico)
                .FirstOrDefault();
            
            if (usuarioExistente != null)
                throw new InvalidOperationException($"Ya existe un usuario con el email {usuario.CorreoElectronico}");

            // RNF5: Encriptar contraseña con BCrypt
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(password);
            usuario.FechaCreacion = DateTime.Now;
            usuario.Rol = usuario.Rol ?? "admin"; // RF B2: Por defecto admin

            _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.SaveAsync();
            
            return usuario;
        }

        public async Task<bool> ValidarCredencialesAsync(string email, string password)
        {
            try
            {
                var usuario = _unitOfWork.Usuarios
                    .Find(u => u.CorreoElectronico == email)
                    .FirstOrDefault();
                
                if (usuario == null)
                    return false;

                // Verificar contraseña con BCrypt
                return BCrypt.Net.BCrypt.Verify(password, usuario.Contrasena);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo)
        {
            // RF B5: Funcionalidad para cambio de contraseña
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado");

            // Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(passwordActual, usuario.Contrasena))
                throw new InvalidOperationException("La contraseña actual es incorrecta");

            if (string.IsNullOrWhiteSpace(passwordNuevo))
                throw new ArgumentException("La nueva contraseña no puede estar vacía");

            // RNF5: Encriptar nueva contraseña
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(passwordNuevo);
            
            _unitOfWork.Usuarios.Update(usuario);
            await _unitOfWork.SaveAsync();
            
            return true;
        }
    }
}
