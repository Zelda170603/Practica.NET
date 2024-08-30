
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace dotnetproyect.Models{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }

        // Relación uno a muchos con Usuario
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
