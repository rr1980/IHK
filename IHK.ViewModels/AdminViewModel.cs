using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserItemViewModel> Users { get; set; }
        public UserItemViewModel CurrentUser { get; set; }
        public List<string> Errors { get; set; }
    }
}
