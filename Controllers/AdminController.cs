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

namespace CrafterCodes.Controllers
{
    [Authorize(Roles = "Organizador, Administrador")]// Esto requiere autenticación para todas las acciones en este controlador
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult agregarEquipos()
        {
           return View("~/Views/Organizador/torneos/agregarEquipos.cshtml");
        }
        
        public IActionResult gestionar()
        {
            return View("~/Views/Organizador/torneos/gestionar.cshtml");
        }
        
        public IActionResult gestionarTorneo()
        {
            return View("~/Views/Organizador/torneos/gestionarTorneo.cshtml");
        }
        
        public IActionResult crearTorneo()
        {
            // Suponiendo que la vista está directamente bajo la carpeta Views
            return View("~/Views/Organizador/torneos/crearTorneo.cshtml");
        }
        
        public IActionResult reglas()
        {
            return View("~/Views/Organizador/torneos/reglas.cshtml");
        }
        
        public IActionResult coaches()
        {
            return View();
        }
        
        public IActionResult arbitros()
        {
            return View();
        }
        
        public IActionResult contabilidad()
        {
            return View();
        }
        
        public IActionResult equipos()
        {
            return View();
        }
        
        public IActionResult noticias()
        {
            return View();
        }
    }
}