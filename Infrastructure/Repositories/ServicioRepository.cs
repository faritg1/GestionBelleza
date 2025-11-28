using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ServicioRepository : GenericRepository<Servicio>, IServicio
    {
        public ServicioRepository(GestionBellezaDbContext context) : base(context)
        {
        }
    }
}
