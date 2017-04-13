using IHK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class GebaeudeViewModel
    {
        public IUserItemViewModel CurrentUser { get; set; }
        public GebaeudeItemViewModel Gebaeude { get; set; }
        public List<string> Errors { get; set; }
    }
}
