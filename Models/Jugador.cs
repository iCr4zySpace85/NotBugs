

namespace CrafterCodes.Models
{
    public class Jugador
{
    public int ID_jugador { get; set; }
    public string IMG_jugador { get; set; }
    public string Nombre { get; set; }
    public int ID_equipo { get; set; }
    public string Posicion { get; set; }
    public int Edad { get; set; }
    public int Numero { get; set; }
    public string Descripcion { get; set; }

    // Navegación hacia Equipo
    public Equipo Equipo { get; set; }
}



}