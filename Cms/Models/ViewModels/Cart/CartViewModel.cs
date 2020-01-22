using System.ComponentModel.DataAnnotations;

namespace Cms.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "Nazwa")]
        public string ProductName { get; set; }
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
        [Display(Name = "Cena")]

        public decimal Price { get; set; }

        [Display(Name = "Razem Cena/Ilość ")]
        public decimal Total { get { return Quantity * Price; } }
        [Display(Name = "Zdjęcie artykułu")]
        public string Image { get; set; }

    }
}