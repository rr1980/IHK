using System.Collections.Generic;

namespace IHK.Common
{
    public interface IUserItemViewModel
    {
        int UserId { get; set; }
        int Anrede { get; set; }

        string Name { get; set; }
        string Vorname { get; set; }

        string Username { get; set; }
        string ShowName { get; set; }
        string Password { get; set; }
        string Postleitzahl { get; set; }
        string Stadt { get; set; }
        string Strasse { get; set; }
        string Hausnummer { get; set; }
        string Telefon { get; set; }
        string Email { get; set; }

        ILayoutThemeItemViewModel LayoutThemeViewModel { get; set; }
        IEnumerable<int> Roles { get; set; }
    }
}
