using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using dotnetproyect.Models; // Cambia esto al espacio de nombres adecuado

public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public AuthController(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    // GET: /Auth/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // GET: /Auth/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string userName, string Apellido, string email, string password, IFormFile picture, string confirmPassword)
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

    // Método para el login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.CorreoElectronico == email);

            if (usuario != null)
            {
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(usuario, usuario.Contrasena, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // Guardar datos en la sesión
                    HttpContext.Session.SetString("UserEmail", email);
                    HttpContext.Session.SetString("UserName", usuario.Nombre);
                    HttpContext.Session.SetString("UserFoto", usuario.FotoPerfil);

                    // Redirigir según el rol adel usuario
                    if (usuario.RoleId != null)
                    {
                        switch (usuario.RoleId)
                        {
                            case 1:
                                return RedirectToAction("AdminDashboard", "Admin"); // Redirige al panel de administrador

                            case 2:
                                return RedirectToAction("Index", "Home"); // Redirige al panel de usuario normal

                            case 3:
                                return RedirectToAction("FabricanteDashboard", "Fabricante"); // Redirige al panel de fabricante

                            default:
                                return RedirectToAction("Index", "Home"); // Redirige por defecto si el rol no es reconocido
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la página de inicio o de login
        return RedirectToAction("Index", "Home");
    }

}
