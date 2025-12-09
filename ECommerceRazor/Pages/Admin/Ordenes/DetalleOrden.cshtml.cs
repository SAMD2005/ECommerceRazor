using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Ordenes
{
    [Authorize(Roles = "Administrador")]
    public class DetalleOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Orden> Ordenes { get; set; }

        public DetalleOrdenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<string> EstadosDisponibles { get; set; }
        public Orden Orden { get; set; }
        public IEnumerable<DetalleOrden> DetallesOrden { get; set; }

        public IActionResult OnGet(int id)
        {
            Orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id, "ApplicationUser");
            if(Orden == null)
            {
                return NotFound("No se encontro la ");
            }

            DetallesOrden = _unitOfWork.DetalleOrden.GetAll(d => d.OrdenId == Id, "Producto");

            //Definir los estados disponibles
            EstadosDisponibles = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EstadoCancelado,
                CNT.EstadoReembolsado

            };
            return Page();
        }

        public IActionResult OnPostActualizarEstado(int id, string nuevoEstado)
        {
            //Buscar la orden 
            var orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);
            if(orden == null)
            {
                return new JsonResult(new {success = false, message = "Orden no encontrada."});
            }

            //Definir los estados permitidos
            var estadosPermitidos = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EstadoCancelado,
                CNT.EstadoReembolsado

            };

            if(!estadosPermitidos.Contains(nuevoEstado))
            {
                return new JsonResult(new { success = false, message = "Estado invalido." });
            }
            //Actualizar el estado 
            orden.Estado = nuevoEstado;
            _unitOfWork.Save();

            //Retornar respuesta exitosa 
            return new JsonResult(new { success = true , message = "EL estado de la orden se actualizo correctamente." });
        }
    }
}
