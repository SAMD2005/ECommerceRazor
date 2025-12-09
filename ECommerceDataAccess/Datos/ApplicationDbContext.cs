using ECommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) :base (options)
        {
            
        }
        //Aqui se añadiran los Dbsets de los modelos (tablas) mas adelante
        public DbSet<Categoria> Categorias { get; set; } 

        public DbSet<Producto> Productos { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<CarritoCompra> CarritoCompras { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetalleOrdenes { get; set; }

      
    }
}
