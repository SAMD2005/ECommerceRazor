using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Producto> Productos { get; set; } = default!;
        public void OnGet()
        {
            //Cargar todas las categorias de la base de datos 
            Productos = _unitOfWork.Producto.GetAll(filter: null,"Categoria");
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id)
        {
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == id);
            if (producto == null)
            {
                TempData["Error"] = "La producto no fue encontrado";
                return RedirectToPage("Index");            
            }
            _unitOfWork.Producto.Remove(producto);
            _unitOfWork.Save();
            TempData["Success"] = "Producto Eliminado con Éxito";
            return new JsonResult(new { success = true });
        }
    }
}
