using System.Collections.Generic;

namespace IHK.ViewModels
{
    public class GebaeudeItemViewModel
    {
        public string KoPath { get; set; }
        public int Id { get; set; }
        public int Etagen { get; set; }
        public int Gaerten { get; set; }

        public List<WohnungItemViewModel> Wohnungen { get; set; }
        public AdressenItemViewModel Adresse { get; set; }
    }
}
