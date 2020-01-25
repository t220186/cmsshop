using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblOrders")]
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedD { get; set; }
        [ForeignKey("UserId")]
        public virtual UsersDTO Users { get; set; }
    }
}