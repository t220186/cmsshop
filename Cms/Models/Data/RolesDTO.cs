using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblRoles")]
    public class RolesDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }





    }
}