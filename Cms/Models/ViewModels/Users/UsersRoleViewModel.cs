using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Users
{
    public class UsersRoleViewModel
    {
        public UsersRoleViewModel() { }

        public UsersRoleViewModel(UsersRoleDTO row) {
            UserId = row.UserId;
            RoleId = row.RoleId;
        }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}