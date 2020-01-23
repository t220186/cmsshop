using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Users
{
    public class LoginUserViewModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika lub Email")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
      
        [Display(Name = "Zapamiętaj mnie")]
        public bool Remember { get; set; }
    }
}