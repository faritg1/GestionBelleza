using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _unitOfWork.Clientes.GetAllAsync();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");
            
            return cliente;
        }

        public async Task<Cliente> CreateClienteAsync(Cliente cliente)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre es obligatorio");
            
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new ArgumentException("El apellido es obligatorio");
            
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw new ArgumentException("El teléfono es obligatorio");

            // Validar cédula única si se proporciona
            if (!string.IsNullOrWhiteSpace(cliente.Cedula))
            {
                var clienteExistente = _unitOfWork.Clientes
                    .Find(c => c.Cedula == cliente.Cedula)
                    .FirstOrDefault();
                
                if (clienteExistente != null)
                    throw new InvalidOperationException($"Ya existe un cliente con la cédula {cliente.Cedula}");
            }

            cliente.FechaRegistro = DateTime.Now;
            _unitOfWork.Clientes.Add(cliente);
            await _unitOfWork.SaveAsync();
            
            return cliente;
        }

        public async Task<Cliente> UpdateClienteAsync(Cliente cliente)
        {
            var clienteExistente = await _unitOfWork.Clientes.GetByIdAsync(cliente.Id);
            if (clienteExistente == null)
                throw new KeyNotFoundException($"Cliente con ID {cliente.Id} no encontrado");

            // Validar cédula única si se modifica
            if (!string.IsNullOrWhiteSpace(cliente.Cedula) && cliente.Cedula != clienteExistente.Cedula)
            {
                var clienteConCedula = _unitOfWork.Clientes
                    .Find(c => c.Cedula == cliente.Cedula && c.Id != cliente.Id)
                    .FirstOrDefault();
                
                if (clienteConCedula != null)
                    throw new InvalidOperationException($"Ya existe otro cliente con la cédula {cliente.Cedula}");
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.CorreoElectronico = cliente.CorreoElectronico;
            clienteExistente.Alias = cliente.Alias;
            clienteExistente.Cedula = cliente.Cedula;

            _unitOfWork.Clientes.Update(clienteExistente);
            await _unitOfWork.SaveAsync();
            
            return clienteExistente;
        }

        public async Task<IEnumerable<Cita>> GetHistorialCitasAsync(int clienteId)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente con ID {clienteId} no encontrado");

            var citas = _unitOfWork.Citas
                .Find(c => c.IdCliente == clienteId)
                .OrderByDescending(c => c.FechaCita)
                .ThenByDescending(c => c.HoraInicio);

            return citas;
        }
    }
}
