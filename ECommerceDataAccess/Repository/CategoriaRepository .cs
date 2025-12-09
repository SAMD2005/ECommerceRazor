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
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
       
        public void update(Categoria categoria)
        {
            var objDesdeBd = _context.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            objDesdeBd.Nombre = categoria.Nombre;
            objDesdeBd.OrdenVisualizacion = categoria.OrdenVisualizacion;
        }
    }
}
