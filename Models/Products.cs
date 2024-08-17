namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public int? Existencia { get; set; }
    public string? Tipo_prod { get; set; }
    public string? foto { get; set; }

    [NotMapped]
    [Required(ErrorMessage ="Por favor seleccione un archivo")]
    public IFormFile? file { get; set; }

    public int FabricanteId { get; set; }
    public Fabricante? Fabricante { get; set; } // Inicialización con un valor no nulo predeterminado

}
