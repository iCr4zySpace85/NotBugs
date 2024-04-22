

namespace CrafterCodes.Models
{
    public class EquipoTorneo
{
    public int ID_equipo_torneo { get; set; }
    public int ID_equipo { get; set; }
    public int ID_torneo { get; set; }

    // Navegación hacia Equipo
    public Equipo Equipo { get; set; }

    // Navegación hacia Torneo
    public Torneos Torneo { get; set; }
}



}