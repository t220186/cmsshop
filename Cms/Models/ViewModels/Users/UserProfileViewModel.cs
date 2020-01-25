using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Users
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel() { }
        //Costructor=>DTO
        public UserProfileViewModel(UsersDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Email = row.Email;
            Username = row.Username;
            Password = row.Password;

        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }
       
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Display(Name = "Powtórz hasło")]
        public string ConfirmPassword { get; set; }

    }
}