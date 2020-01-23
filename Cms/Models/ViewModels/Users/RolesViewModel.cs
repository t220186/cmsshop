using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Users
{
    public class RolesViewModel
    {

        //constructor
        public RolesViewModel() { }

        public RolesViewModel(RolesDTO row) {
            Id = row.Id;
            Name = row.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

    }
}