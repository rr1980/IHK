using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class MieterViewModel
    {
        public UserItemViewModel CurrentUser { get; set; }
        public MieterItemViewModel Mieter { get; set; }
        public List<string> Errors { get; set; }
    }
}
