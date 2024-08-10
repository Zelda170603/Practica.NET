namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Fabricante
{
    public int Id { get; set; }
    public string? nombre { get; set; }
    public string? ubicacion { get; set; }
    public string? direccion { get; set; }
    public string? correo { get; set; }
    public int? telefono { get; set; }
    public string? descripcion { get; set; }
    public string? foto_fab { get; set; }
    public ICollection<Product> Productos { get; set; } = new List<Product>(); // Inicialización de la colección

    // Constructor
    public Fabricante()
    {
        Productos = new List<Product>(); // Asegura que la colección esté inicializada
    }
}
