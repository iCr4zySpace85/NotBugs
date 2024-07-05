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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // Importante para Include
using System.Linq;


namespace CrafterCodes.Controllers
{
    
    public class PageinfoController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        
        private readonly Contexto _contexto;
        private readonly ApplicationDbContext _context;


        public PageinfoController(ILogger<HomeController> logger, Contexto contexto, ApplicationDbContext context)
        {
            _logger = logger;
            _contexto = contexto;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var torneos = await GetTorneos(); // Suponiendo que esta función devuelve una lista de torneos
            var noticias = await GetNoticias(); // Suponiendo que esta función devuelve una lista de noticias

            var viewModel = new PageInfoViewModel
            {
                Torneos = torneos,
                Noticias = noticias
            };

            return View(viewModel);

        }
        public async Task<IActionResult> Torneos(int id)
        {
            var torneo = await GetTorneos(); 
           var equipos = await GetEquipos(id);
           var noticias = await GetNoticiasTorneo(id);

           var viewModel = new PageInfoViewModel
            {
                Torneos = torneo,
                Equipo = equipos,
                Noticias = noticias
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Equipo(int id)
        {
            var Equipos = await GetEquipos(id);
            return View(Equipos);
        }
        public IActionResult Calendario()
        {
            
            return View();
        }

        public async Task<List<Torneos>> GetTorneos()
        {
            var torneos = new List<Torneos>();

            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SP_PageInfo_ListaTorneos", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var torneo = new Torneos
                            {
                                ID_torneo = reader.GetInt32(reader.GetOrdinal("ID_torneo")),
                                IMG_torneo = reader.GetString(reader.GetOrdinal("IMG_torneo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Deporte = new Deporte
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("NombreDeporte"))
                                },
                                Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
                                Fecha_inicio = reader.GetDateTime(reader.GetOrdinal("Fecha_inicio")),
                                Fecha_fin = reader.GetDateTime(reader.GetOrdinal("Fecha_fin"))
                            };
                            torneos.Add(torneo);
                        }
                    }
                }
            }

            return torneos;
        }

        public async Task<List<Equipo>> GetEquipos(int idTorneo)
        {
            var equipos = new List<Equipo>();

            using (var conn = new SqlConnection(_contexto.Conexion))
            {
                using (var cmd = new SqlCommand("SP_PageInfo_ListaEquiposID", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_torneo", idTorneo);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var equipo = new Equipo
                            {
                                ID_equipo = reader.GetInt32(reader.GetOrdinal("ID_equipo")),
                                IMG_equipo = reader.GetString(reader.GetOrdinal("IMG_equipo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                ID_deporte = reader.GetInt32(reader.GetOrdinal("ID_deporte")),
                                Categoria = reader.GetString(reader.GetOrdinal("Categoria"))
                            };
                            equipos.Add(equipo);
                        }
                    }
                }
            }

            return equipos;
        }

       private async Task<List<Noticia>> GetNoticias()
{
    List<Noticia> noticias = new List<Noticia>();
    using (var conn = new SqlConnection(_contexto.Conexion))
    {
        using (var cmd = new SqlCommand("SP_PageInfo_GetNoticias", conn))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    noticias.Add(new Noticia
                    {
                        ID_noticia = reader.GetInt32("ID_noticia"),
                        Titulo = reader.GetString("Titulo"),
                        Contenido = reader.IsDBNull(reader.GetOrdinal("Contenido")) ? null : reader.GetString("Contenido"),
                        Fecha_publicacion = reader.GetDateTime("Fecha_publicacion"),
                        ID_autor = reader.GetInt32("ID_autor")
                    });
                }
            }
        }
    }
    return noticias;
}

private async Task<List<Noticia>> GetNoticiasTorneo(int idTorneo)
{
    List<Noticia> noticias = new List<Noticia>();
    using (var conn = new SqlConnection(_contexto.Conexion))
    {
        using (var cmd = new SqlCommand("SP_PageInfo_GetNoticiasTorneo", conn))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_torneo", idTorneo);
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    noticias.Add(new Noticia
                    {
                        ID_noticia = reader.GetInt32("ID_noticia"),
                        Titulo = reader.GetString("Titulo"),
                        Contenido = reader.IsDBNull(reader.GetOrdinal("Contenido")) ? null : reader.GetString("Contenido"),
                        Fecha_publicacion = reader.GetDateTime("Fecha_publicacion"),
                        ID_autor = reader.GetInt32("ID_autor")
                    });
                }
            }
        }
    }
    return noticias;
}

private async Task<List<Noticia>> GetNoticiasEquipo(int idEquipo)
{
    List<Noticia> noticias = new List<Noticia>();
    using (var conn = new SqlConnection(_contexto.Conexion))
    {
        using (var cmd = new SqlCommand("SP_PageInfo_GetNoticiasEquipo", conn))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_equipo", idEquipo);
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    noticias.Add(new Noticia
                    {
                        ID_noticia = reader.GetInt32("ID_noticia"),
                        Titulo = reader.GetString("Titulo"),
                        Contenido = reader.IsDBNull(reader.GetOrdinal("Contenido")) ? null : reader.GetString("Contenido"),
                        Fecha_publicacion = reader.GetDateTime("Fecha_publicacion"),
                        ID_autor = reader.GetInt32("ID_autor")
                    });
                }
            }
        }
    }
    return noticias;
}





    }
}
