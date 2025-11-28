using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CitaRepository : GenericRepository<Cita>, ICita
    {
        private readonly GestionBellezaDbContext _context;

        public CitaRepository(GestionBellezaDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .ToListAsync();
        }

        public override async Task<Cita> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.CitaServicios)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}