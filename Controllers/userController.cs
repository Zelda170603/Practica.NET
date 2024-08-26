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
    public class userController : Controller
    {

        private readonly AppDbContext _context;

        public userController(AppDbContext context)
        {
            _context = context;
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Guardar la imagen
                if (user.file != null && user.file.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/usuarios");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    var fileinfo = new FileInfo(user.file.FileName);
                    string filename = timestamps + fileinfo.Extension;
                    string fileNameWithPath = Path.Combine(path, filename);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await user.file.CopyToAsync(stream);
                    }

                    user.foto = filename; // Guardar el nombre de la imagen en el campo `foto`
                }

                // Guardar el usuario en la base de datos
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirigir a la lista de usuarios (puedes cambiar el nombre del método de la acción)
            }
            return View(user);
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}