using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Ordenes
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public IEnumerable<Orden> Ordenes { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
            //Cargar las ordenes de la BD
            Ordenes = _unitOfWork.Orden.GetAll(filter:null, "ApplicationUser");
        }
    }
}
