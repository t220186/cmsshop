using Cms.Models.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cms.Models.ViewModels.Shop
{
    public class ProductsViewModel
    {
        //proiducts vm constructor
        public ProductsViewModel() { }

        public ProductsViewModel(ProductsDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            Slug = row.Slug;
            Price = row.Price;
            CategoriesId = row.CategoriesId;
            ImageName = row.ImageName;


        }


        public int Id { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Nazwa Produktu")]
        public string Name { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Opis produktu")]
        public string Description { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Display(Name = "Kategorie")]
        public int CategoriesId { get; set; }
        public string ImageName { get; set; }


        //list Categories - Wybrana kategoria dla produktu
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }


    }
}