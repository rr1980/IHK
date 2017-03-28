using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.Models
{
    public class Adresse : BaseModel
    {
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
    }
}
