using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblAdvertisement")]
    public class AdvertisementDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string Description { get; set; }

    }
}