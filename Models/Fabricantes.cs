namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Fabricante
{public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del fabricante es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder de 100 caracteres.")]
    public string? nombre { get; set; }

    [StringLength(150, ErrorMessage = "La ubicación no puede exceder de 150 caracteres.")]
    public string? ubicacion { get; set; }

    [StringLength(200, ErrorMessage = "La dirección no puede exceder de 200 caracteres.")]
    public string? direccion { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido.")]
    public string? correo { get; set; }

    [Required(ErrorMessage = "El numero de telefono es obligatorio")]
    public int? telefono { get; set; }

    [StringLength(300, ErrorMessage = "La descripción no puede exceder de 300 caracteres.")]
    public string? descripcion { get; set; }

    [DataType(DataType.Upload)]
    [StringLength(255, ErrorMessage = "La ruta de la foto no puede exceder de 255 caracteres.")]
    public string? foto_fab { get; set; }

    [NotMapped]
    [Required(ErrorMessage ="Por favor seleccione un archivo")]
    public IFormFile? foto { get; set; }

    public ICollection<Product> Productos { get; set; } = new List<Product>();

    // Constructor
    public Fabricante()
    {
        Productos = new List<Product>(); // Asegura que la colección esté inicializada
    }
}

