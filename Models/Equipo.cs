using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class Equipo
{
    [Key]
    public int ID_equipo { get; set; }
    public string IMG_equipo { get; set; }  // Asumiendo que se guarda como una URL o un path de archivo
    public string Nombre { get; set; }
    public int ID_deporte { get; set; }
    public string Categoria { get; set; }

    // Relación con Jugadores
    public List<Jugador> Jugadores { get; set; }  // Una lista de jugadores en el equipo

    // Relación con Torneos a través de la tabla Equipos_Torneos
    public List<EquipoTorneo> EquiposTorneos { get; set; }
}


}