using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize(Roles = "Cliente")]
    public class ResumenModel : PageModel
    {

        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        [BindProperty]
        public Orden Orden { get; set; }   
        
        private readonly IUnitOfWork _unitOfWork;

        public ResumenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Orden = new Orden();
        }
        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.
                    GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    "Producto,Producto.Categoria");

                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    Orden.TotalOrden += (double)(itemCarrito.Producto.Precio * itemCarrito.Cantidad);
                }

                //Obtener datos del usuario del repositorio de ApplicationUser
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.
                    GetFirstOrDefault(u => u.Id == claim.Value);

                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Direccion = applicationUser.Direccion;
                Orden.Telefono = applicationUser.PhoneNumber;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales;
            }
        }
        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.
                    GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    "Producto,Producto.Categoria");

                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    Orden.TotalOrden += (double)(itemCarrito.Producto.Precio * itemCarrito.Cantidad);
                }

                //Obtener datos del usuario del repositorio de ApplicationUser
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.
                    GetFirstOrDefault(u => u.Id == claim.Value);

                Orden.Estado = CNT.EstadoPendiente;
                Orden.FechaOrden = DateTime.Now;
                Orden.UsuarioId = claim.Value;
                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Telefono = Orden.Telefono;
                Orden.Direccion = Orden.Direccion;
                Orden.Telefono = applicationUser.PhoneNumber;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales;

                _unitOfWork.Orden.Add(Orden);
                _unitOfWork.Save();

                //Aqui se agrega el DetalleOrden
                foreach (var item in ListaCarritoCompra)
                {
                    DetalleOrden detalleOrden = new DetalleOrden()
                    {
                        ProductoId = item.ProductoId,
                        OrdenId = Orden.Id,
                        NombreProducto = item.Producto.Nombre,
                        Precio = (double)item.Producto.Precio,
                        Cantidad = item.Cantidad
                    };
                    _unitOfWork.DetalleOrden.Add(detalleOrden);
                    _unitOfWork.Save();
                }

                //Obtencion de cantidad 
                //int cantidad = ListaCarritoCompra.ToList().Count; 

                //_unitOfWork.CarritoCompra.RemoveRange(ListaCarritoCompra);
                _unitOfWork.Save();

                //Aqui esta el codigo para reenviar al pago en stripe
                var domain = $"{Request.Scheme}://{Request.Host.Value}";

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                   
                    Mode = "payment",
                    SuccessUrl = $"{domain}/Cliente/Carrito/ConfirmacionOrden?id={Orden.Id}",
                    CancelUrl = $"{domain}/Cliente/Carrito/Index",
                };

                //Agregar items 
                foreach(var item in ListaCarritoCompra)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            //7.34 734
                            UnitAmount = (long)(item.Producto.Precio * 100), //Total en centavos
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Producto.Nombre,
                                //Description = "Total de items Distintos: " + cantidad,
                            },
                        },
                        Quantity = item.Cantidad,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);
                
                Orden.SessionId = session.Id;
                //Orden.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Save();

                return new StatusCodeResult(303);
            }
            return Page();
        }
    }
}
