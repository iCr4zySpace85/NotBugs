
namespace CrafterCodes.Models.ViewModels
{
    public class PageInfoViewModel
    {
        public IEnumerable<Torneos> Torneos { get; set; }
        public IEnumerable<Jugador> Jugador { get; set; }
        public IEnumerable<Equipo> Equipo { get; set; }

        public IEnumerable<Noticia> Noticias { get; set; }
    }
}