using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Cliente.Inicio
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Producto> Productos { get; set; }

        public void OnGet()
        {
            //Usamos el metodo GetAll para incluir categorias relacionadas
            Productos = _unitOfWork.Producto.GetAll(filter: null, "Categoria");
        }
    }
}
