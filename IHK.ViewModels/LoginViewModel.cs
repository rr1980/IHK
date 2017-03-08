using IHK.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IHK.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Username " + ErrorMessage.MINMAXLENGTH)]
        public string Username { get; set; } = "rr1980";
        [DataType(DataType.Password)]
        public string Password { get; set; } = "12003";
        [Required]
        public string ReturnUrl { get; set; }

        public UserViewModel CurrentUser { get; set; }
    }
}
