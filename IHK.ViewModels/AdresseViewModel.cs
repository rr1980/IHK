using IHK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class AdresseViewModel
    {
        public IUserItemViewModel CurrentUser { get; set; }
        public AdressenItemViewModel Adresse { get; set; }
        public List<string> Errors { get; set; }
    }
}
