
using ECommerce.DataAccess;
using ECommerce.Models;
using ECommerce.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public IndexModel(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IEnumerable<Categoria> Categorias { get; set; } = default!;
        public void OnGet()
        {
            //Cargar todas las categorias de la base de datos 
            Categorias = _unitOfWork.Categoria.GetAll();

            //Prueba de envio de Email
            string email = "serguiomendoza2@gmail.com";
            string subject = "Prueba de envio de correo";
            string htmlMessage = "<h1>Este es un mensaje de prueba</h1><p>Saludos desde .NET</p>";

            _emailSender.SendEmailAsync(email, subject, htmlMessage);
        }
        public async Task<IActionResult> OnPostDeleteAsync([FromBody]int id)
        {
            var categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                TempData["Error"] = "La categoria no fue encontrada";
                return RedirectToPage("Index");
                //return NotFound();
            }
            _unitOfWork.Categoria.Remove(categoria);
            _unitOfWork.Save();
            TempData["Success"] = "Categoria Eliminada con Éxito";
            return new JsonResult(new { success = true });
        }
    }
}
