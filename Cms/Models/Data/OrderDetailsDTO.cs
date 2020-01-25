using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    [Table("tblOrderDetails")]
    public class OrderDetailsDTO
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderDTO Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductsDTO Products { get; set; }
    }
}