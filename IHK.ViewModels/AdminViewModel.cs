using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public UserViewModel CurrentUser { get; set; }
        public List<string> Errors { get; set; }
    }
}
