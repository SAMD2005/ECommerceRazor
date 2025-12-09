
using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class EditarModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]

        public Categoria Categoria { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == id);

            if (Categoria == null)
            {
                return NotFound();
            }
                 
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var categoriaBd = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == Categoria.Id);

            if (categoriaBd == null) 
            { 
                return NotFound();  
            }

            //Actualizamos los campos modificables
            categoriaBd.Nombre = Categoria.Nombre;
            categoriaBd.OrdenVisualizacion = Categoria.OrdenVisualizacion;

            //Guardar los cambios          
            _unitOfWork.Save();
            TempData["Success"] = "Categoria Editada con Éxito";
            return RedirectToPage("Index");
        }
    }
}
