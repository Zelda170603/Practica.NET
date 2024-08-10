using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnetproyect.Models;

namespace dotnetproyect.Controllers
{
    public class productController : Controller
    {
        private readonly AppDbContext _context;

        public productController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Incluye la relación con Fabricante
            var productos = await _context.Product.Include(p => p.Fabricante)
                        .ToListAsync();
            return View(productos);
        }

        // GET: fabricante/Create
        public IActionResult Create()
        {
            var fabricantes = _context.Fabricante.ToList();
            ViewBag.FabricanteId = new SelectList(fabricantes, "Id", "nombre");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Existencia,Tipo_prod,foto,FabricanteId")] Product product)
        {
            
            if (ModelState.IsValid)
            {
                // Agregar el producto a la base de datos
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
    }
}