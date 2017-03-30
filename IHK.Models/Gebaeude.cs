using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IHK.Models
{
    public class Gebaeude : BaseModel
    {
        public Adresse Adresse { get; set; }
        public int Etagen { get; set; }
        public int Gaerten { get; set; }
        public int Wohnungen { get; set; }
    }
}
