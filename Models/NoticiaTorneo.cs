using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class NoticiaTorneo
{
    [Key]
    public int ID_noticia_torneo { get; set; }
    public int ID_noticia { get; set; }
    public int ID_torneo { get; set; }

    public Noticia Noticia { get; set; }
    public Torneos Torneo { get; set; }
}


}