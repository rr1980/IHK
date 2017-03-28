using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class OptionViewModel
    {
        public UserItemViewModel CurrentUser { get; set; }
        public List<LayoutThemeItemViewModel> LayoutThemeViewModels { get; set; }
    }
}
