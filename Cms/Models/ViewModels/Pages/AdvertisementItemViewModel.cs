using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Pages
{
    public class AdvertisementItemViewModel
    {
        public AdvertisementItemViewModel() { }
        public AdvertisementItemViewModel(AdvertisementItemDTO rows) {
            Id = rows.Id;
            IdAvertisement = rows.IdAvertisement;
            Primary = rows.Primary;
            Image = rows.Image;
            Create = rows.Create;
            Update = rows.Update;
            LeadText = rows.LeadText;
            LinkTo = rows.LinkTo;

        }
             public int Id { get; set; }
        public int IdAvertisement { get; set; }
        public bool Primary { get; set; }
        public string Image { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string LeadText { get; set; }
        public string LinkTo { get; set; }
   
        
    }
}