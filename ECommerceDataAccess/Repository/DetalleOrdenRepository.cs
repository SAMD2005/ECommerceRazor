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
    public class DetalleOrdenRepository : Repository<DetalleOrden>, IDetalleOrdenRepository
    {
        private readonly ApplicationDbContext _context;
        public DetalleOrdenRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
       
        public void update(DetalleOrden detalleOrden)
        {
            var objDesdeBd = _context.DetalleOrdenes.FirstOrDefault(c => c.Id == detalleOrden.Id);
           _context.DetalleOrdenes.Update(objDesdeBd);
        }
    }
}
