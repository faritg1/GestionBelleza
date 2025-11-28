using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CitaRepository : GenericRepository<Cita>, ICita
    {
        public CitaRepository(GestionBellezaDbContext context) : base(context)
        {
        }
    }
}