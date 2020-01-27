using Cms.Models.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cms.Models.ViewModels.Pages
{
    public class AdvertisementViewModel
    {
        public AdvertisementViewModel() { }

        public AdvertisementViewModel(AdvertisementDTO rows)
        {
            Id = rows.Id;
            Name = rows.Name;
            Create = rows.Create;
            Update = rows.Update;
            Description = rows.Description;

        }
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Nazwa reklamy")]
        public string Name { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
       
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Opis własny - niewymagany")]
        public string Description { get; set; }



    }
}