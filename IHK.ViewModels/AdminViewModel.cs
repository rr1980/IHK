using IHK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<IUserItemViewModel> Users { get; set; }
        public IUserItemViewModel CurrentUser { get; set; }
        public List<string> Errors { get; set; }
    }
}
