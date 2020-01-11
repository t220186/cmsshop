using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    
    [Table("tblSideBar")]
    public class SideBarDTO
    {
        [Key]
        public int Id { get; set; }
       
        public string Body { get; set; }

    }
}