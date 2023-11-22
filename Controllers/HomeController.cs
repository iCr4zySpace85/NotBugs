using CrafterCodes.Data;
using CrafterCodes.Models;
using CrafterCodes.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]

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
                        cmd.Parameters.AddWithValue("@contraseña", model.Pass);
                        con.Open();
                        try
                        {
                            using (var dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    
                                    int idUsuario = (int)dr["idPersonal"];
                                    string? name = (string)dr["nombre"];
                                    string? ap = (string)dr["apellidoPaterno"];
                                    string? am = (string)dr["apellidoMaterno"];
                                    string nombreCompleto = name + " " + ap + " " + am;

                                    if (name != null)
                                    {
                                        var claims = new List<Claim>()
                                                {
                                                    new Claim(ClaimTypes.NameIdentifier, name),
                                                    new Claim(ClaimTypes.Name, nombreCompleto),
                                                    new Claim(ClaimTypes.SerialNumber, idUsuario.ToString())
                                                };

                                        int perfilId = (int)dr["idRol"];
                                        string perfilNombre = (string)dr["nombreRol"];
                                        claims.Add(new Claim(ClaimTypes.Role, perfilNombre));

                                        var identify = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                        var propiedades = new AuthenticationProperties
                                        {
                                            AllowRefresh = true,
                                            IsPersistent = true,
                                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1)),
                                        };

                                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identify), propiedades);

                                        return RedirectToAction("Login", "Home");
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