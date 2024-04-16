using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography; // Para MD5
using System.Text; // Para Encoding
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrafterCodes.Models;

namespace CrafterCodes.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdPersonal == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersonal,Nombre,ApellidoPaterno,ApellidoMaterno,IdRol,Correo,Contraseña,NumeroCelular,CodigoVerificacion")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                // Encriptar la contraseña antes de guardarla
                usuarios.Contraseña = GetMd5Hash(usuarios.Contraseña);
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersonal,Nombre,ApellidoPaterno,ApellidoMaterno,IdRol,Correo,Contraseña,NumeroCelular,CodigoVerificacion")] Usuarios usuarios)
        {
            if (id != usuarios.IdPersonal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuarios.Contraseña = GetMd5Hash(usuarios.Contraseña);
                    _context.Update(usuarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.IdPersonal))
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
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdPersonal == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdPersonal == id);
        }

        // Método para encriptar la contraseña usando MD5
        private string GetMd5Hash(string input)
        {
            // Crear una instancia del algoritmo MD5
            using (MD5 md5Hash = MD5.Create())
            {
                // Convertir la cadena de entrada a un array de bytes y calcular el hash
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Crear una cadena para almacenar el hash en formato hexadecimal
                StringBuilder sBuilder = new StringBuilder();

                // Convertir cada byte del hash en su representación hexadecimal
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Devolver el hash en formato hexadecimal
                return sBuilder.ToString();
            }
        }
    }
}
