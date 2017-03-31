using IHK.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IHK.ViewModels
{
    public class MieterItemViewModel
    {
        public int Id { get; set; }
        public int Anrede { get; set; }
        [Required(ErrorMessage = "Name " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name " + ErrorMessage.MINMAXLENGTH)]
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Telefon { get; set; }
        public string WbsNummer { get; set; }

        public WohnungItemViewModel Wohnung { get; set; }
        //public string Wohnungsnummer { get; set; }
        //public string Postleitzahl { get; set; }
        //public string Stadt { get; set; }
        //public string Strasse { get; set; }
        //public string Hausnummer { get; set; }
    }
}
