using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class NavbarViewModel
    {
        public bool ShowSidebar { get; set; }
        public UserItemViewModel CurrentUser { get; set; }
        public string activeTab { get; set; }
    }
}
