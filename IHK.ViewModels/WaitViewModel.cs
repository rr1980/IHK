using IHK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.ViewModels
{
    public class WaitViewModel
    {
        public IMultiUserBlockViewModel MubBlock { get; set; }
        public int Position { get; set; }
    }
}
