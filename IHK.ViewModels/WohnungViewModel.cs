using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class WohnungViewModel
    {
        public UserItemViewModel CurrentUser { get; set; }
        public WohnungItemViewModel Wohnung { get; set; }
        public GebaeudeItemViewModel Gebaeude { get; set; }
        public AdressenItemViewModel Adressen { get; set; }
        public List<string> Errors { get; set; }
    }
}
