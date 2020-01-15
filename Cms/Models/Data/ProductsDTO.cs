using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cms.Models.Data
{
    //table
    [Table("tblProducts")]
    public class ProductsDTO
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        //description
        public string Description { get; set; }

        public string Slug { get; set; }
        public decimal Price { get; set; }

        public int CategoriesId { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("CategoriesId")]
        public virtual CategoriesDTO Categories { get; set; }

    }
}