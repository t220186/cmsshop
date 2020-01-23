using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblUsersRole")]
    public class UsersRoleDTO
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        //referencvja tabeli 
        [ForeignKey("UserId")]
        public virtual UsersDTO Users { get; set; }
        [ForeignKey("RoleId")]
        public virtual RolesDTO Roles { get; set; }
    }
}