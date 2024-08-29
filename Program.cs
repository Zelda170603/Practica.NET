using dotnetproyect.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("AppDbContext"),
    new MySqlServerVersion(new Version(10, 4, 32))).EnableDetailedErrors()
);

// Configuración de Identity con ApplicationUser
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configura el almacenamiento en memoria para sesiones
builder.Services.AddDistributedMemoryCache(); 

// Configura las sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de inactividad antes de que la sesión expire
    options.Cookie.HttpOnly = true; // Configura el cookie como HttpOnly para mayor seguridad
});



var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthentication(); // Asegúrate de que la autenticación esté habilitada
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
