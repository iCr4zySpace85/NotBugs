

namespace CrafterCodes.Models
{
    public class NoticiaEquipo
{
    public int ID_noticia_equipo { get; set; }
    public int ID_noticia { get; set; }
    public int ID_equipo { get; set; }

    public Noticia Noticia { get; set; }
    public Equipo Equipo { get; set; }
}

}