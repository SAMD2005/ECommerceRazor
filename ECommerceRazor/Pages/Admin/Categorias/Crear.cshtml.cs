using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class CrearModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrearModel(IUnitOfWork unitOfWork)   
        {
            _unitOfWork = unitOfWork; 
        }

        [BindProperty]

        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Validacion personalizada: comprobar si el nombre ya existe
            //bool nombreExiste = _context.Categorias.Any(c => c.Nombre == Categoria.Nombre);
            //if(nombreExiste)
            //{
            //    ModelState.AddModelError("Categoria.Nombre", "El nombre de la categoria ya existe. Por favor elige otro.");
            //        return Page();
            //}

            //Validacion personalizada: comprobar si el nombre ya existe V 2.0 con repository
            if(_unitOfWork.Categoria.ExisteNombre(Categoria.Nombre))
            {
                ModelState.AddModelError("Categoria.Nombre", "El nombre de la categoria ya existe. Por favor elige otro.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Asignar la fecha de creacion
            Categoria.FechaCreacion = DateTime.Now;

            _unitOfWork.Categoria.Add(Categoria);
            _unitOfWork.Save();

            //Usar TempData para mostrar el mensaje en la pagina del indice
            TempData["Success"] = "Categoria Creada con Éxito";

            return RedirectToPage("Index");
        }
    }
}
