using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GestionBellezaDbContext _context;
        public UnitOfWork(GestionBellezaDbContext context)
        {
            _context = context;
        }
        public ICita _citas;
        public ICita Citas
        {
            get
            {
                if (_citas == null)
                {
                    _citas = new CitaRepository(_context);
                }
                return _citas;
            }
        }

        public ICliente _clientes;
        public ICliente Clientes
        {
            get
            {
                if (_clientes == null)
                {
                    _clientes = new ClienteRepository(_context);
                }
                return _clientes;
            }
        }

        public IPago _pagos;
        public IPago Pagos
        {
            get
            {
                if (_pagos == null)
                {
                    _pagos = new PagoRepository(_context);
                }
                return _pagos;
            }
        }

        public IUsuario _usuarios;
        public IUsuario Usuarios
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new UsuarioRepository(_context);
                }
                return _usuarios;
            }
        }

        public IServicio _servicios;
        public IServicio Servicios
        {
            get
            {
                if (_servicios == null)
                {
                    _servicios = new ServicioRepository(_context);
                }
                return _servicios;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}