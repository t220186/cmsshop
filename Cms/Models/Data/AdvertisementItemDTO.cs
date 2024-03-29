﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblAdvertisementItem")]
    public class AdvertisementItemDTO
    {
        [Key]
        public int Id { get; set; }
        public int IdAvertisement { get; set; }
       
        public string Image { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string LeadText { get; set; }
        public int LinkTo { get; set; }

        [ForeignKey("IdAvertisement")]
        public virtual AdvertisementDTO Advertisement { get; set; }
        [ForeignKey("LinkTo")]
        public virtual ProductsDTO Products { get; set; }

    }


}