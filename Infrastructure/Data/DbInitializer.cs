using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(GestionBellezaDbContext context)
        {
            // Verificar si ya existe el usuario admin específico
            var adminEmail = "admin@gestionbelleza.com";
            var adminExistente = await context.Usuarios
                                    .Where(u => u.CorreoElectronico == adminEmail)
                                    .FirstOrDefaultAsync();

            if (adminExistente != null)
            {
                return; // El admin ya existe, no hacemos nada
            }

            // Crear usuario Admin por defecto
            var admin = new Usuario
            {
                NombreCompleto = "Administrador Sistema",
                CorreoElectronico = adminEmail,
                // Encriptamos la contraseña "Admin123!"
                Contrasena = BCrypt.Net.BCrypt.HashPassword("Admin123!"), 
                Rol = "admin",
                FechaCreacion = DateTime.Now,
                Telefono = "0000000000"
            };

            context.Usuarios.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
