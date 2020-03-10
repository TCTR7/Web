using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Mời nhập user name")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Mời nhập password")]
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}