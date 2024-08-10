using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace dotnetproyect.Models;
public class AppDbContext : IdentityDbContext<IdentityUser>{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){

    }
    public DbSet<Product> Product{ get; set;}
    public DbSet<Fabricante> Fabricante{ get; set;}
}