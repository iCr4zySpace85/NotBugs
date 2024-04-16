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
        private readonly ApplicationDbContext _context;

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

        public IActionResult RecuperarPassword()
        { 
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword(string correoElectronico)
        {
            // try
            // {
                // Verificar si el correo electrónico existe en la base de datos
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SELECT IdPersonal FROM Usuarios WHERE Correo = @correo", con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correoElectronico);
                        var idPersonal = await cmd.ExecuteScalarAsync();

                        if (idPersonal != null)
                        {
                            // Obtener el número de teléfono vinculado al correo electrónico
                            string numeroDestino = ObtenerNumeroTelefono((int)idPersonal);

                            // Generar y enviar el código de verificación por SMS
                            string smsEnviado = await EnviarCodigo(numeroDestino);

                            if (smsEnviado != null)
                            {
                                // Almacenar el código de verificación en la base de datos para su posterior verificación
                                
                                AlmacenarCodigoVerificacion((int)idPersonal, smsEnviado);

                                // Redirigir a una página donde se ingrese el código de verificación
                                return RedirectToAction("IngresarCodigoVerificacion");
                            }
                            else
                            {
                                // Error al enviar el SMS
                                ViewBag.Error = "Error al enviar el código de verificación. Por favor, intenta de nuevo más tarde.";
                            }
                        }
                        else
                        {
                            // El correo electrónico no está registrado en la base de datos
                            ViewBag.Error = "El correo electrónico proporcionado no está registrado.";
                        }
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     // Manejar cualquier excepción
            //     ViewBag.Error = "Ocurrió un error al procesar la solicitud.";
            //     _logger.LogError(ex, "Error al recuperar la contraseña");
            // }

            // Si hay algún error, volver a mostrar la vista RecuperarPassword
            return View();
        }

        private string ObtenerNumeroTelefono(int idUsuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT NumeroCelular FROM Usuarios WHERE IdPersonal = @idUsuario", con))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        var numeroCelular = cmd.ExecuteScalar();

                        if (numeroCelular != null)
                        {
                            return numeroCelular.ToString();
                        }
                        else
                        {
                            // Si no se encuentra el número de teléfono, puedes manejarlo de acuerdo a tus necesidades
                            return null; // O lanzar una excepción, dependiendo de tus requerimientos
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la consulta a la base de datos
                ViewBag.Error = "Ocurrió un error al obtener el número de teléfono.";
                _logger.LogError(ex, "Error al obtener el número de teléfono");
                return null;
            }
        }

        private void AlmacenarCodigoVerificacion(int idUsuario, string codigoVerificacion)
        {
            // try
            // {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT COUNT(*) FROM Usuarios WHERE IdPersonal = @idUsuario";
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Si existe un registro de código de verificación para este usuario, actualiza el código existente
                            cmd.CommandText = "UPDATE Usuarios SET CodigoVerificacion = @codigoVerificacion WHERE IdPersonal = @idUsuario";
                        }
                        else
                        {
                            // Si no existe un registro de código de verificación para este usuario, inserta un nuevo registro
                            cmd.CommandText = "INSERT INTO Usuarios (IdPersonal, CodigoVerificacion) VALUES (@idUsuario, @codigoVerificacion)";
                        }

                        cmd.Parameters.AddWithValue("@codigoVerificacion", codigoVerificacion);
                        cmd.ExecuteNonQuery();
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     // Manejar cualquier excepción
            //     ViewBag.Error = "Ocurrió un error al almacenar el código de verificación.";
            //     _logger.LogError(ex, "Error al almacenar el código de verificación");
            // }
        }


        private readonly string _accountSid = "ACd102e6c898ae2fffe1ee72bfc1c89e27";
        private readonly string _authToken = "c22e50dc460a80c8a020c2053e6d6d6c";
        private readonly string _twilioNumber = "+13344542050";

        private async Task<string> EnviarCodigo(string numeroDestino)
        {
            try
            {
                // Generar un código de verificación aleatorio
                string codigoVerificacion = GenerarCodigoVerificacion();

                // Cuerpo del mensaje SMS
                string mensaje = $"Tu código de verificación es: {codigoVerificacion}";

                // Construir la solicitud HTTP para enviar el mensaje SMS
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_accountSid}:{_authToken}")));

                var parametros = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("From", _twilioNumber),
            new KeyValuePair<string, string>("To", numeroDestino),
            new KeyValuePair<string, string>("Body", mensaje)
        });

                // Endpoint de la API de Twilio para enviar mensajes SMS
                var url = $"https://api.twilio.com/2010-04-01/Accounts/{_accountSid}/Messages.json";

                // Enviar la solicitud HTTP para enviar el mensaje SMS
                var respuesta = await client.PostAsync(url, parametros);

                // Verificar si el mensaje se envió correctamente
                if (respuesta.IsSuccessStatusCode)
                {
                    // El mensaje fue enviado correctamente
                    return codigoVerificacion;
                }
                else
                {
                    // Hubo un error al enviar el mensaje
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la solicitud HTTP
                Console.WriteLine($"Error al enviar el mensaje SMS: {ex.Message}");
                return null;
            }
        }


        private string GenerarCodigoVerificacion()
        {
            // Longitud del código de verificación
            int longitudCodigo = 6;

            // Caracteres válidos para el código de verificación
            string caracteres = "0123456789";

            // Generar el código de verificación aleatorio
            StringBuilder codigo = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < longitudCodigo; i++)
            {
                int indice = random.Next(caracteres.Length);
                codigo.Append(caracteres[indice]);
            }

            return codigo.ToString();
        }

        [HttpGet]
    public IActionResult IngresarCodigoVerificacion()
    {
        return View();
    }



        [HttpPost]
        public IActionResult IngresarCodigoVerificacion(string codigoVerificacion)
        {
            try
            {

                int idUsuario = ObtenerIdUsuarioPorCodigoVerificacion(codigoVerificacion);
                // Obtener el código de verificación almacenado en la base de datos para el usuario actual
                // Suponiendo que tengas una tabla llamada "CodigoVerificacion" que almacena los códigos de verificación
                string codigoAlmacenado = ObtenerCodigoVerificacionFromDatabase(idUsuario); // Implementa este método para obtener el código de verificación almacenado

                // Comparar el código de verificación ingresado por el usuario con el código almacenado en la base de datos
                if (codigoVerificacion == codigoAlmacenado)
                {

                    TempData["CodigoVerificacion"] = codigoVerificacion;
                    TempData["IdUsuario"] = idUsuario;// Utilizando TempData
                    // Si los códigos coinciden, el código de verificación es válido
                    // Puedes redirigir al usuario a una página donde pueda cambiar su contraseña o realizar la acción correspondiente
                    return RedirectToAction("CambiarContraseña");
                }
                else
                {
                    // Si los códigos no coinciden, el código de verificación es inválido
                    // Muestra un mensaje de error al usuario
                    ViewBag.Error = "El código de verificación ingresado es incorrecto.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                ViewBag.Error = "Ocurrió un error al verificar el código de verificación.";
                _logger.LogError(ex, "Error al verificar el código de verificación");
                return View();
            }
        }

        private string ObtenerCodigoVerificacionFromDatabase(int idUsuario)
        {
            string codigoVerificacion = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();

                    // Consulta SQL para obtener el código de verificación para el usuario actual
                    string query = "SELECT CodigoVerificacion FROM Usuarios WHERE IdPersonal = @IdPersonal";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IdPersonal", idUsuario);
                        codigoVerificacion = (string)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la consulta a la base de datos
                ViewBag.Error = "Ocurrió un error al obtener el código de verificación.";
                _logger.LogError(ex, "Error al obtener el código de verificación de la base de datos");
            }

            return codigoVerificacion;
        }

        private int ObtenerIdUsuarioPorCodigoVerificacion(string codigoVerificacion)
        {
            int idUsuario = -1; // Valor por defecto si no se encuentra el código de verificación

            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();

                    // Consulta SQL para obtener el ID del usuario asociado al código de verificación
                    string query = "SELECT IdPersonal FROM Usuarios WHERE CodigoVerificacion = @CodigoVerificacion";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CodigoVerificacion", codigoVerificacion);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            idUsuario = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la consulta a la base de datos
                ViewBag.Error = "Ocurrió un error al obtener el ID del usuario.";
                _logger.LogError(ex, "Error al obtener el ID del usuario por código de verificación");
            }

            return idUsuario;
        }

        [HttpGet]
        public IActionResult CambiarContraseña()
        {
            // Recuperar el código de verificación de TempData o ViewBag
            string codigoVerificacion = TempData["CodigoVerificacion"]?.ToString();
             string idUsuario = TempData["IdUsuario"]?.ToString(); // Utilizando TempData

            // Pasar el código de verificación a la vista utilizando ViewBag
            ViewBag.CodigoVerificacion = codigoVerificacion;
            ViewBag.IdUsuario = idUsuario;
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContraseña(string nuevaContraseña, string codigoVerificacion, int idUsuario)
        {
            try
            {

                // Verificar si el código de verificación ingresado coincide con el almacenado en la base de datos para el usuario actual
                bool codigoVerificacionCorrecto = VerificarCodigoVerificacion(idUsuario, codigoVerificacion);

                if (!codigoVerificacionCorrecto)
                {
                    // El código de verificación ingresado es incorrecto, mostrar un mensaje de error
                    ViewBag.Error = "El código de verificación ingresado es incorrecto.";
                    return View();
                }

                // Si el código de verificación es correcto, proceder con el cambio de contraseña
                // Encriptar la nueva contraseña utilizando el método GetMd5Hash
                string contraseñaEncriptada = GetMd5Hash(nuevaContraseña);

                // Aquí puedes implementar la lógica para cambiar la contraseña del usuario en la base de datos
                // Por ejemplo, actualizando el registro del usuario con la nueva contraseña encriptada
                ActualizarContraseñaUsuario(idUsuario, contraseñaEncriptada);

                // Después de cambiar la contraseña, puedes redirigir al usuario a una página de inicio de sesión u otra página relevante
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                ViewBag.Error = "Ocurrió un error al cambiar la contraseña.";
                _logger.LogError(ex, "Error al cambiar la contraseña");
                return View();
            }
        }
        private bool VerificarCodigoVerificacion(int idUsuario, string codigoVerificacion)
        {
            try
            {
                // Conexión a la base de datos
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    // Consulta SQL para verificar el código de verificación
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE IdPersonal = @IdUsuario AND CodigoVerificacion = @CodigoVerificacion";

                    // Comando SQL
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@CodigoVerificacion", codigoVerificacion);

                        // Abrir la conexión
                        con.Open();

                        // Ejecutar el comando y obtener el resultado
                        int count = (int)cmd.ExecuteScalar();

                        // Verificar si se encontró una coincidencia
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                ViewBag.Error = "Ocurrió un error al verificar el código de verificación.";
                _logger.LogError(ex, "Error al verificar el código de verificación");
                return false;
            }
        }
        private void ActualizarContraseñaUsuario(int idUsuario, string nuevaContraseñaEncriptada)
{
    try
    {
        // Conexión a la base de datos
        using (SqlConnection con = new SqlConnection(_contexto.Conexion))
        {
            // Consulta SQL para actualizar la contraseña del usuario
            string query = "UPDATE Usuarios SET contraseña = @NuevaContraseña WHERE IdPersonal = @IdUsuario";

            // Comando SQL
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                // Agregar parámetros
                cmd.Parameters.AddWithValue("@NuevaContraseña", nuevaContraseñaEncriptada);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                // Abrir la conexión
                con.Open();

                // Ejecutar el comando
                cmd.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Manejar cualquier excepción
        ViewBag.Error = "Ocurrió un error al actualizar la contraseña.";
        _logger.LogError(ex, "Error al actualizar la contraseña del usuario");
    }
}


    }


}