using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;


namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize(Roles = "Cliente")]
    public class ConfirmacionOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public int OrdenId { get; set; }

        public ConfirmacionOrdenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void OnGet(int id)
        {
            Orden orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);

            if (orden.SessionId != null)
            {
                var servicio = new SessionService();
                Session session = servicio.Get(orden.SessionId);
                if(session.PaymentStatus.ToLower() == "paid")
                {
                    orden.PaymentIntentId = session.PaymentIntentId;
                    orden.Estado = CNT.EstadoPagoEnviado;
                    _unitOfWork.Save();
                }
            }

            List<CarritoCompra> carritoCompras = _unitOfWork.CarritoCompra.GetAll(
                u => u.ApplicationUserId == orden.UsuarioId
                ).ToList();

            _unitOfWork.CarritoCompra.RemoveRange(carritoCompras);
            _unitOfWork.Save();
            OrdenId = id;
        }
    }
}
