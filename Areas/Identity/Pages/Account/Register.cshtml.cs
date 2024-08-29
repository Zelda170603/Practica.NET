// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using dotnetproyect.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Http;
public class RegisterModel : PageModel
{
    [Required]
    [Display(Name = "Nombre Completo")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirma tu Contraseña")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Sube una foto de perfil")]
    public IFormFile Picture { get; set; }
}

