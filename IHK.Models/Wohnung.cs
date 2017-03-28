using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.Models
{
    public class Wohnung : BaseModel
    {
        public Gebaeude Gebaeude { get; set; }
    }
}
