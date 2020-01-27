using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Models.ViewModels.Pages
{
    public class AdvertisementItemViewModel
    {
        public AdvertisementItemViewModel() { }
        public AdvertisementItemViewModel(AdvertisementItemDTO rows) {
            Id = rows.Id;
            IdAvertisement = rows.IdAvertisement;
           
            Image = rows.Image;
            Create = rows.Create;
            Update = rows.Update;
            LeadText = rows.LeadText;
            LinkTo = rows.LinkTo;

        }
             public int Id { get; set; }
        public int IdAvertisement { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Obraz/Reklama główna")]
      
        [Display(Name = "Zdjęcie")]
        public string Image { get; set; }
        [Display(Name = "Utworzono")]
        public DateTime Create { get; set; }
        [Display(Name = "Zmodyfikowano")]
        public DateTime Update { get; set; }
        [Display(Name = "Tekst prowadzący")]
        [AllowHtml]
        public string LeadText { get; set; }
        [Display(Name = "Odwołuje do")]
        public int LinkTo { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }


    }
}