using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cms.Models.ViewModels.Users
{
    public class OrdersForUserViewModel
    {
        [Display(Name = "Numer zamówienia")]
        public int OrdersNumber { get; set; }
        [Display(Name = "Wartość zamówienia")]
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQuantity { get; set; }
        [Display(Name = "Zamówienie z dnia")]
        public DateTime CreatedD { get; set; }

    }
}