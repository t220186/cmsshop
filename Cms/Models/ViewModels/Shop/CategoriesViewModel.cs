using Cms.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cms.Models.ViewModels.Shop
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel() { }
        public CategoriesViewModel(CategoriesDTO row)
        {
            ID = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }

        public int ID { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Nazwa Kategorii")]
        public string Name { get; set; }
        [Display(Name = "Unikalny adres kasategori (dodawany automatycznie)")]
        public string Slug { get; set; }
        [Display(Name = "Kolejność wyświetlania(dodawany automatycznie)")]
        public int Sorting { get; set; }
    }
}