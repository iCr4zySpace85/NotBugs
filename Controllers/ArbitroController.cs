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
    [Authorize(Roles = "Organizador, Arbitro, Administrador")]
    public class ArbitroController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        
        private readonly Contexto _contexto;
        private readonly ApplicationDbContext _context;

        public ArbitroController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }
        public IActionResult index()
        {
            return View();
        }
        
        public IActionResult panelArbitro()
        {
            return View();
        }

    }
}