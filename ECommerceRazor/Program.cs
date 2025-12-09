using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Utility;
using ECommerceDataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;



var builder = WebApplication.CreateBuilder(args);

//Configurar el contexto con la cadena de conexion
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")
));

//Soporte para la configuracion de stripe
builder.Services.Configure<ConfiguracionStripe>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
   options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Configurar requerimiento de confirmacion de Email
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; //Esto debe ser true para enviar el correo
});

//Soporte para Cookies de Autenticacion y Autorizacion
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; //Ruta predeterminada para iniciar sesion
    options.LogoutPath = "/Identity/Account/Logout";//Ruta predeterminada para iniciar sesion
    options.AccessDeniedPath = "/Identity/Account/DeniedPath"; //Ruta predeterminada para acceso denegado
    options.Cookie.HttpOnly = true; //Mejora la seguridad al prevenir acceso del lado del cliente a la cookie
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //Duracion de la cookie antes de expirar
    options.SlidingExpiration = true; //Remueva la cookie si el usuario permanece activo
});

builder.Services.AddDistributedMemoryCache();

//soporte para trabajos con secciones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddRazorPages();

//Agregar el soporte para EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

//Agregar Repositorios al contendor de inyeccion de dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

string key = builder.Configuration.GetSection("Stripe:ClaveSecreta").Get<string>();
StripeConfiguration.ApiKey = key;

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

//Redirigir Manualmente a la pagina deseada
app.MapGet("/", context =>
   {
       context.Response.Redirect("/Cliente/Inicio/Index");
       return Task.CompletedTask;
   });


app.Run();
