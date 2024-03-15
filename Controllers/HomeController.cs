using CrafterCodes.Data;
using CrafterCodes.Models;
using CrafterCodes.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace CrafterCodes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly Contexto _contexto;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }


        /*[HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Verificar las credenciales en la base de datos
            var user = _context.Personal.FirstOrDefault(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);

            if (user != null)
            {
                // Autenticación exitosa
                // Puedes implementar la lógica de sesiones aquí
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Credenciales incorrectas
                ModelState.AddModelError("", "Credenciales incorrectas");
                return View(model);
            }
        }*/
        
        public IActionResult Index()
        {
            var menuManager = new MenuManager();
            string? userRole = @User.FindFirstValue(ClaimTypes.Role);
            List<string> userMenu = menuManager.GetMenuForUserRole(userRole);

            var viewModel = new MenuViewModel
            {
                UserMenu = userMenu
            };

            return View(viewModel);
            // return View();
        }
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Login");

            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("SP_Login", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@correo", model.Username);
                        cmd.Parameters.AddWithValue("@contraseña", GetMd5Hash(model.Pass)); // Encriptar la contraseña antes de buscarla en la base de datos
                        con.Open();
                        try
                        {
                            using (var dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    // Tu código de autenticación aquí
                                    int idUsuario = (int)dr["idPersonal"];
                                    string? name = (string)dr["nombre"];
                                    string? ap = (string)dr["apellidoPaterno"];
                                    string? am = (string)dr["apellidoMaterno"];
                                    string nombreCompleto = name + " " + ap + " " + am;

                                    if (name != null)
                                    {
                                        // Crear las reclamaciones del usuario para la autenticación
                                        var claims = new List<Claim>()
                                            {
                                                new Claim(ClaimTypes.NameIdentifier, name),
                                                new Claim(ClaimTypes.Name, nombreCompleto),
                                                new Claim(ClaimTypes.SerialNumber, idUsuario.ToString())
                                            };

                                        // Obtener el rol del usuario de la base de datos
                                        int perfilId = (int)dr["idRol"];
                                        string perfilNombre = (string)dr["nombreRol"];
                                        claims.Add(new Claim(ClaimTypes.Role, perfilNombre));

                                        // Crear la identidad del usuario con las reclamaciones
                                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                        // Configurar las propiedades de autenticación
                                        var authenticationProperties = new AuthenticationProperties
                                        {
                                            AllowRefresh = true,
                                            IsPersistent = true,
                                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1))
                                        };

                                        // Iniciar sesión del usuario
                                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authenticationProperties);

                                        // Redireccionar al usuario a la página de inicio
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        ViewBag.Error = "Usuario no registrado";
                                    }
                                }
                                else
                                {
                                    ViewBag.Error = "Usuario no Registrado";
                                    dr.Close();
                                }
                            }
                        }
                        finally
                        {
                            if (cmd != null)
                                cmd.Dispose();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Login");
        }

        // Método para encriptar la contraseña usando MD5
        private string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}