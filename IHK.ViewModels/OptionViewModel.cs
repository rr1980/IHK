using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class OptionViewModel
    {
        public UserViewModel CurrentUser { get; set; }
        public List<LayoutThemeViewModel> LayoutThemeViewModels { get; set; }
    }
}
