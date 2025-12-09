using ECommerce.DataAccess;
using ECommerce.Models;
using ECommerceDataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class OrdenRepository : Repository<Orden>, IOrdenRepository
    {
        private readonly ApplicationDbContext _context;
        public OrdenRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
       
        public void update(Orden orden)
        {
            var objDesdeBd = _context.Ordenes.FirstOrDefault(c => c.Id == orden.Id);
           _context.Ordenes.Update(objDesdeBd);
        }
    }
}
