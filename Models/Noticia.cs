using System.ComponentModel.DataAnnotations;

namespace CrafterCodes.Models
{
    public class Noticia
{
    [Key]
    public int ID_noticia { get; set; }
    public string Titulo { get; set; }
    public string Contenido { get; set; }
    public DateTime Fecha_publicacion { get; set; }
    public int ID_autor { get; set; }

    // Relaciones
    public List<NoticiaEquipo> NoticiasEquipos { get; set; }
    public List<NoticiaTorneo> NoticiasTorneos { get; set; }
}

}