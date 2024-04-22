using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class Deporte
    {
        [Key]
        public int ID_deporte { get; set; }
        public string Nombre { get; set; }

        // Propiedad de navegación para la relación con Torneos
        public ICollection<Torneos> Torneos { get; set; }
    }
}