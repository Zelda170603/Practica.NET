namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Nombre { get; set; }

    [Required]
    [StringLength(100)]
    public string? Apellido { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string? CorreoElectronico { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Contrasena { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(250)]
    public string? Direccion { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    public string? Genero { get; set; }

    [StringLength(250)]
    public string? FotoPerfil { get; set; }

    public string? Rol { get; set; }

    public string? foto { get; set; }

    [NotMapped]
    [Required(ErrorMessage ="Por favor seleccione un archivo")]
    public IFormFile? file { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
