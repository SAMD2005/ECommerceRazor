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
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductoRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
       
        public void Update(Producto producto)
        {
            var objDesdeBd = _context.Productos.FirstOrDefault(c => c.Id == producto.Id);
            objDesdeBd.Nombre = producto.Nombre;
            objDesdeBd.Descripcion = producto.Descripcion;
            objDesdeBd.Precio = producto.Precio;
            objDesdeBd.CantidadDisponible = producto.CantidadDisponible;
            objDesdeBd.CategoriaId = producto.CategoriaId;        
            objDesdeBd.Imagen = producto.Imagen;
           
        }
    }
}
