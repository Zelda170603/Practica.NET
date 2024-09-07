using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnetproyect.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace dotnetproyect.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Auth/Register
        [HttpGet]
        public async Task<IActionResult> Fabricantes()
        {
            return View(await _context.Fabricante.ToListAsync());
        }
        // GET: /Auth/Register
        [HttpGet]
        public async Task<IActionResult> Productos()
        {
            // Incluye la relación con Fabricante
            var productos = await _context.Product.Include(p => p.Fabricante)
                        .ToListAsync();
            return View(productos);
        }
        // GET: /Auth/Register
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            
            return View(await _context.Usuario.ToListAsync());
        }
    }
}