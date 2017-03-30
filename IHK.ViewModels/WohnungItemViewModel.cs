using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class WohnungItemViewModel
    {
        public string Wohnungsnummer { get; set; }
        public int Etage { get; set; }
        public bool Keller { get; set; }
        public bool Garage { get; set; }
        public bool Balkon { get; set; }
        public bool Garten { get; set; }
        public int Raeume { get; set; }
        public decimal Qm { get; set; }

        public List<MieterItemViewModel> Wohnungen { get; set; }
        public GebaeudeItemViewModel Gebaeude { get; set; }
    }
}
