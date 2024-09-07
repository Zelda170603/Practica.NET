using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using dotnetproyect.Models; // Cambia esto al espacio de nombres adecuado


using Microsoft.EntityFrameworkCore;

namespace dotnetproyect.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UserController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        // GET: fabricante/CreateUser
        public IActionResult CreateUser()
        {
            return View();
        }

        public async Task<IActionResult> Create(string userName, string Apellido, string email, string password, IFormFile picture, string confirmPassword)
        {
            // Validaciones iniciales
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son obligatorios.");
                return View();
            }

            try
            {
                // Verificar si el correo ya está registrado
                if (_context.Usuario.Any(u => u.CorreoElectronico == email))
                {
                    ModelState.AddModelError(string.Empty, "Este correo electrónico ya está registrado.");
                    return View();
                }

                string filename = "";

                // Procesar la imagen de perfil si se ha subido
                if (picture != null && picture.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile_pictures");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    var fileinfo = new FileInfo(picture.FileName);
                    filename = timestamps + "_" + fileinfo.Name;

                    string filePath = Path.Combine(path, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }
                }
                var rolNormal = _context.Role.FirstOrDefault(r => r.Nombre == "Administrador");
                // Obtener el rol "Normal" var rolNormal = await _context.Role.FirstOrDefault(r => r.Nombre == "Normal");
                if (rolNormal == null)
                {
                    ModelState.AddModelError(string.Empty, "El rol 'Normal' no está disponible.");
                    return View();
                }

                // Crear el nuevo usuario con el rol "Normal"
                var usuario = new Usuario
                {
                    Nombre = userName,
                    Apellido = Apellido,
                    CorreoElectronico = email,

                    FotoPerfil = filename, // Guardar el nombre del archivo en la base de datos
                    RoleId = rolNormal.Id // Asignar el rol "Normal" al usuario
                };
                usuario.Contrasena = _passwordHasher.HashPassword(usuario, password);
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al registrar el usuario: {ex.Message}");
                return View();
            }
        }

        // GET: fabricante/EditUser/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser([Bind("Nombre,Apellido,CorreoElectronico,Telefono,Direccion,Genero,FechaNacimiento,FotoPerfil")] Usuario usuario, IFormFile FotoPerfil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Procesar la imagen de perfil si se ha subido
                    if (FotoPerfil != null && FotoPerfil.Length > 0)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Usuarios");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        // Generar un nombre único para la imagen de perfil
                        string timestamps = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        var fileinfo = new FileInfo(FotoPerfil.FileName);
                        string filename = timestamps + fileinfo.Extension;
                        string filePath = Path.Combine(path, filename);

                        // Guardar la imagen en el directorio
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await FotoPerfil.CopyToAsync(stream);
                        }

                        // Asignar el nombre del archivo de imagen al usuario
                        usuario.FotoPerfil = filename;
                    }

                    // Agregar el usuario a la base de datos
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(IndexUsers)); // Redirige a la lista de usuarios después de registrarlo
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error al registrar el usuario: {ex.Message}");
                }
            }

            return View(usuario); // Devuelve la vista en caso de que ocurra algún error
        }


        // GET: fabricante/DeleteUser/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: fabricante/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Usuarios");

                // Elimina la imagen del usuario si existe
                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    string existingFile = Path.Combine(path, usuario.FotoPerfil);
                    if (System.IO.File.Exists(existingFile))
                    {
                        System.IO.File.Delete(existingFile);
                    }
                }

                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexUsers));
        }

        // Verifica si el usuario existe
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        // GET: fabricante/IndexUsers (Vista para listar usuarios)
        public async Task<IActionResult> IndexUsers()
        {
            return View(await _context.Usuario.ToListAsync());
        }

    }
}
