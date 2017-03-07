using IHK.Models;

namespace IHK.DB.SeedBuilder
{
    public static class LayoutTheme_Builder
    {
        private static DataContext _context;

        internal static void Create(DataContext context)
        {
            _context = context;
            //_build("default", "//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css");
            _build("default", "lib/bootstrap/dist/css/bootstrap.css");
            _build("amelia", "//bootswatch.com/amelia/bootstrap.min.css");
            _build("cerulean", "//bootswatch.com/cerulean/bootstrap.min.css");
            _build("cosmo", "//bootswatch.com/cosmo/bootstrap.min.css");
            _build("cyborg", "//bootswatch.com/cyborg/bootstrap.min.css");
            _build("flatly", "//bootswatch.com/flatly/bootstrap.min.css");
            _build("journal", "//bootswatch.com/journal/bootstrap.min.css");
            _build("readable", "//bootswatch.com/readable/bootstrap.min.css");
            _build("simplex", "//bootswatch.com/simplex/bootstrap.min.css");
            _build("slate", "//bootswatch.com/slate/bootstrap.min.css");
            _build("spacelab", "//bootswatch.com/spacelab/bootstrap.min.css");
            _build("united", "//bootswatch.com/united/bootstrap.min.css");

            context.SaveChanges();
        }

        private static void _build(string name, string link)
        {
            LayoutTheme lt;
            lt = new LayoutTheme();
            lt.Name = name;
            lt.Link = link;

            _context.LayoutThemes.Add(lt);
        }
    }
}
