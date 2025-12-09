using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize(Roles = "Cliente")]
    public class IndexModel : PageModel
    {
        public IEnumerable<CarritoCompra> ListaCarritoCompra {  get; set; }
        public double TotalCarrito { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            TotalCarrito = 0;
        }
        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.
                    GetAll(filter: u=> u.ApplicationUserId == claim.Value,
                    "Producto,Producto.Categoria");

                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    TotalCarrito += (double)(itemCarrito.Producto.Precio *itemCarrito.Cantidad);
                }
            }           
        }
        public IActionResult OnPostMas(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.
                GetFirstOrDefault(c => c.Id ==carritoId);
            _unitOfWork.CarritoCompra.IncrementarContador(carrito, 1);
            return RedirectToPage("/Cliente/Carrito/Index");
        }
        public IActionResult OnPostMenos(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.
                GetFirstOrDefault(c => c.Id == carritoId);


            if (carrito.Cantidad == 1)
            {
                var contador = _unitOfWork.CarritoCompra.
                GetAll(u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count - 1;

                _unitOfWork.CarritoCompra.Remove(carrito);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(CNT.CarritoSession, contador);
            }
            else
            {
                _unitOfWork.CarritoCompra.DecrementarContador(carrito, 1);
            }
              
            return RedirectToPage("/Cliente/Carrito/Index");
        }
        public IActionResult OnPostEliminar(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.
                GetFirstOrDefault(c => c.Id == carritoId);
            _unitOfWork.CarritoCompra.Remove(carrito);

            var contador = _unitOfWork.CarritoCompra.
                GetAll(u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count-1;

            _unitOfWork.Save();
            HttpContext.Session.SetInt32(CNT.CarritoSession, contador);
             
            return RedirectToPage("/Cliente/Carrito/Index");
        }
    }
}
