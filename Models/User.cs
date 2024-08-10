namespace dotnetproyect.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class User{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? correo {get; set; }
    public string? password {get; set; }
}