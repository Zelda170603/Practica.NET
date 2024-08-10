using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetproyect.Models;

namespace dotnetproyect.Controllers;

public class AuthController : Controller
{
    public IActionResult login()
    {
        return View();
    }

    public IActionResult register(){
        return View();
    }
}