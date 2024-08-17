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
        // GET: fabricante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Existencia,Tipo_prod,file,FabricanteId")] Product product)
        {
            
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                var fileinfo = new FileInfo(product.file.FileName);
                string filename = timestamps + fileinfo.Name + fileinfo.Extension;
                string fileNameWITHpATH = Path.Combine(path, filename);

                using (var stream = new FileStream(fileNameWITHpATH, FileMode.Create))
                {
                    product.file.CopyTo(stream);
                }

                product.foto = filename;

                // Agregar el producto a la base de datos
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Product.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            var fabricantes = _context.Fabricante.ToList();
            ViewBag.FabricanteId = new SelectList(fabricantes, "Id", "nombre", producto.FabricanteId);

            return View(producto);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Existencia,Tipo_prod,file,FabricanteId,foto")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Manejo de la imagen
                    if (product.file != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        var fileinfo = new FileInfo(product.file.FileName);
                        string filename = timestamps + fileinfo.Name + fileinfo.Extension;
                        string fileNameWITHpATH = Path.Combine(path, filename);

                        using (var stream = new FileStream(fileNameWITHpATH, FileMode.Create))
                        {
                            product.file.CopyTo(stream);
                        }

                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(product.foto))
                        {
                            string oldFilePath = Path.Combine(path, product.foto);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        product.foto = filename;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductosExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var fabricantes = _context.Fabricante.ToList();
            ViewBag.FabricanteId = new SelectList(fabricantes, "Id", "nombre", product.FabricanteId);

            return View(product);
        }


                public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Product
                .Include(p => p.Fabricante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Product.FindAsync(id);
            if (producto != null)
            {
                // Eliminar la imagen asociada
                if (!string.IsNullOrEmpty(producto.foto))
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos");
                    string filePath = Path.Combine(path, producto.foto);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Product.Remove(producto);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
        private bool ProductosExists(int id)
        {
            return _context.Fabricante.Any(e => e.Id == id);
        }

    }
}