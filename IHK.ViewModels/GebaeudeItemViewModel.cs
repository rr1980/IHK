using System.Collections.Generic;

namespace IHK.ViewModels
{
    public class GebaeudeItemViewModel
    {
        public int Etagen { get; set; }
        public int Gaerten { get; set; }

        public List<WohnungItemViewModel> Wohnungen { get; set; }
        public AdressenItemViewModel Adresse { get; set; }
    }
}
