using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class MieterViewModel
    {
        public MUBBlockViewModel MubBlock { get; set; }
        public UserItemViewModel CurrentUser { get; set; }
        public MieterItemViewModel Mieter { get; set; }
        //public WohnungItemViewModel Wohnung { get; set; }
        //public GebaeudeItemViewModel Gebaeude { get; set; }
        //public AdressenItemViewModel Adresse { get; set; }
        public List<string> Errors { get; set; }
    }
}
