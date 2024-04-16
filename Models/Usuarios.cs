using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class Usuarios
    {
        [Key]
        public int IdPersonal { get; set; }

        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int IdRol { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string NumeroCelular { get; set; }

        public string CodigoVerificacion { get; set; }

    }
}