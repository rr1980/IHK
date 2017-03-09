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
            _build("darkly", "lib/bootswatch-4/darkly/bootstrap.css");

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
