using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class Torneos
    {
        [Key]
        public int ID_torneo { get; set; }
        public string IMG_torneo { get; set; } // Para almacenar imágenes como datos binarios
        public string Nombre { get; set; }
        public int ID_deporte { get; set; }
        public string Categoria { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }

        // Propiedad de navegación para la relación con Deportes
        public Deporte Deporte { get; set; }
    }


}