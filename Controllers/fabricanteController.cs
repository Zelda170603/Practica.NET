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
    public class fabricanteController : Controller
    {
        private readonly AppDbContext _context;

        public fabricanteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: fabricante
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fabricante.ToListAsync());
        }

        // GET: fabricante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricante = await _context.Fabricante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabricante == null)
            {
                return NotFound();
            }

            return View(fabricante);
        }

        // GET: fabricante/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: fabricante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,ubicacion,direccion,correo,telefono,descripcion,foto")] Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Fabricantes");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                var fileinfo = new FileInfo(fabricante.foto.FileName);
                string filename = timestamps + fileinfo.Name + fileinfo.Extension;
                string fileNameWITHpATH = Path.Combine(path, filename);

                using (var stream = new FileStream(fileNameWITHpATH, FileMode.Create))
                {
                    fabricante.foto.CopyTo(stream);
                }

                fabricante.foto_fab = filename;

                _context.Add(fabricante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        // GET: fabricante/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricante = await _context.Fabricante.FindAsync(id);
            if (fabricante == null)
            {
                return NotFound();
            }
            return View(fabricante);
        }

        // POST: fabricante/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,ubicacion,direccion,correo,telefono,descripcion,foto_fab,foto")] Fabricante fabricante)
        {
            if (id != fabricante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si se sube una nueva imagen, se procesa
                    if (fabricante.foto != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Fabricantes");
                        // Elimina la imagen anterior si existe
                        if (!string.IsNullOrEmpty(fabricante.foto_fab))
                        {
                            string existingFile = Path.Combine(path, fabricante.foto_fab);
                            if (System.IO.File.Exists(existingFile))
                            {
                                System.IO.File.Delete(existingFile);
                            }
                        }
                        // Guardar la nueva imagen
                        string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        var fileinfo = new FileInfo(fabricante.foto.FileName);
                        string filename = timestamps + fileinfo.Name + fileinfo.Extension;
                        string fileNameWITHpATH = Path.Combine(path, filename);
                        using (var stream = new FileStream(fileNameWITHpATH, FileMode.Create))
                        {
                            fabricante.foto.CopyTo(stream);
                        }
                        fabricante.foto_fab = filename;
                    }
                    _context.Update(fabricante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricanteExists(fabricante.Id))
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
            return View(fabricante);
        }


        // GET: fabricante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricante = await _context.Fabricante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabricante == null)
            {
                return NotFound();
            }

            return View(fabricante);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabricante = await _context.Fabricante.FindAsync(id);
            if (fabricante != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Fabricantes");

                // Elimina la imagen del fabricante si existe
                if (!string.IsNullOrEmpty(fabricante.foto_fab))
                {
                    string existingFile = Path.Combine(path, fabricante.foto_fab);
                    if (System.IO.File.Exists(existingFile))
                    {
                        System.IO.File.Delete(existingFile);
                    }
                }

                _context.Fabricante.Remove(fabricante);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool FabricanteExists(int id)
        {
            return _context.Fabricante.Any(e => e.Id == id);
        }
    }
}
