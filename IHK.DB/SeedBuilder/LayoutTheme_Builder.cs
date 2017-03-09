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
            _build("default", "lib/bootstrap-4/default/css/bootstrap.css");
            _build("darkly", "lib/bootswatch-4/darkly/bootstrap.css");
            _build("cerulan", "lib/bootswatch-4/cerulan/bootstrap.css");
            _build("cosmo", "lib/bootswatch-4/cosmo/bootstrap.css");
            _build("cyborg", "lib/bootswatch-4/cyborg/bootstrap.css");
            _build("custom", "lib/bootswatch-4/custom/bootstrap.css");
            _build("flatly", "lib/bootswatch-4/flatly/bootstrap.css");
            _build("journal", "lib/bootswatch-4/journal/bootstrap.css");
            _build("litera", "lib/bootswatch-4/litera/bootstrap.css");
            _build("lumen", "lib/bootswatch-4/lumen/bootstrap.css");
            _build("lux", "lib/bootswatch-4/lux/bootstrap.css");
            _build("materia", "lib/bootswatch-4/materia/bootstrap.css");
            _build("minty", "lib/bootswatch-4/minty/bootstrap.css");
            _build("pulse", "lib/bootswatch-4/pulse/bootstrap.css");
            _build("sandstone", "lib/bootswatch-4/sandstone/bootstrap.css");
            _build("simplex", "lib/bootswatch-4/simplex/bootstrap.css");
            _build("sketchy", "lib/bootswatch-4/sketchy/bootstrap.css");
            _build("slate", "lib/bootswatch-4/slate/bootstrap.css");
            _build("solar", "lib/bootswatch-4/solar/bootstrap.css");
            _build("spacelab", "lib/bootswatch-4/spacelab/bootstrap.css");
            _build("superhero", "lib/bootswatch-4/superhero/bootstrap.css");
            _build("united", "lib/bootswatch-4/united/bootstrap.css");
            _build("yeti", "lib/bootswatch-4/yeti/bootstrap.css");

            context.SaveChanges();
        }

        private static void _build(string name, string link)
        {
            LayoutTheme lt;
            lt = new LayoutTheme()
            {
                Name = name,
                Link = link
            };
            _context.LayoutThemes.Add(lt);
        }
    }
}
