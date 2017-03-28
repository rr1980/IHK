using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class MieterItemViewModel
    {
        public int Id { get; set; }
        public int Anrede { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Telefon { get; set; }
        public string WbsNummer { get; set; }
        public string Wohnungsnummer { get; set; }
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
    }
}
