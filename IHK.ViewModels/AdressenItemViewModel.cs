using System.Collections.Generic;

namespace IHK.ViewModels
{
    public class AdressenItemViewModel
    {
        public string KoPath { get; set; }
        public int Id { get; set; }
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }

        public List<GebaeudeItemViewModel> Gebaeude { get; set; }
    }
}
