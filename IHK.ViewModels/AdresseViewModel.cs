using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class AdresseViewModel
    {
        public UserItemViewModel CurrentUser { get; set; }
        public AdressenItemViewModel Adresse { get; set; }
        public List<string> Errors { get; set; }
    }
}
