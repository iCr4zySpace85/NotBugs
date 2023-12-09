// IMenuBuilderStrategy.cs
public interface IMenuBuilderStrategy
{
    List<string> BuildMenu();
}

// AdministradorMenuBuilder.cs
public class AdministradorMenuBuilder : IMenuBuilderStrategy
{
    public List<string> BuildMenu()
    {
        return new List<string> { "Torneos", "Coaches", "Árbitros", "Equipo", "Contabilidad", "Noticias" };
    }
}

// CoauchMenuBuilder.cs
public class CoauchMenuBuilder : IMenuBuilderStrategy
{
    public List<string> BuildMenu()
    {
        return new List<string> { "Equipo", "Jugadores", "Partidos", "Resultados" };
    }
}

// ContadorMenuBuilder.cs
public class ContadorMenuBuilder : IMenuBuilderStrategy
{
    public List<string> BuildMenu()
    {
        return new List<string> { "Inicio" };
    }
}

// ArbitroMenuBuilder.cs
public class ArbitroMenuBuilder : IMenuBuilderStrategy
{
    public List<string> BuildMenu()
    {
        return new List<string> { "Equipos", "Partidos", "Resultados" };
    }
}

// DefaultMenuBuilder.cs
public class DefaultMenuBuilder : IMenuBuilderStrategy
{
    public List<string> BuildMenu()
    {
        return new List<string>(); // Implementación predeterminada, puede ser una lista vacía o personalizada según necesidades.
    }
}
