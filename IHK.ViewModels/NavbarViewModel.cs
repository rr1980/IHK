using IHK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class NavbarViewModel
    {
        public bool ShowSidebar { get; set; }
        public IUserItemViewModel CurrentUser { get; set; }
        public string ActiveTab { get; set; }
    }
}
