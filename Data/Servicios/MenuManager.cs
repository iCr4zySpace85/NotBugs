// MenuManager.cs
public class MenuManager
{
    private Dictionary<string, IMenuBuilderStrategy> menuStrategies;

    public MenuManager()
    {
        menuStrategies = new Dictionary<string, IMenuBuilderStrategy>
        {
            { "Administrador", new AdministradorMenuBuilder() },
            { "Coauch", new CoauchMenuBuilder() },
            { "Contador", new ContadorMenuBuilder() },
            { "Arbitro", new ArbitroMenuBuilder() }
        };
    }

    public List<string> GetMenuForUserRole(string userRole)
    {
        if (string.IsNullOrEmpty(userRole))
        {
            // Tratar el caso de sesión nula o sin roles según tus necesidades.
            // En este ejemplo, simplemente se devuelve un menú predeterminado.
            return new DefaultMenuBuilder().BuildMenu();
        }

        return menuStrategies.GetValueOrDefault(userRole, new DefaultMenuBuilder()).BuildMenu();
    }
}