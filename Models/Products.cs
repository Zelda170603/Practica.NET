namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public int? Existencia { get; set; }
    public string? Tipo_prod { get; set; }
    public string? foto { get; set; }

    public int FabricanteId { get; set; }
    public Fabricante Fabricante { get; set; } = null!; // Inicialización con un valor no nulo predeterminado

    // Constructor
    public Product()
    {
        Fabricante = new Fabricante(); 
    }
}
