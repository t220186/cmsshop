using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cms.Models.ViewModels.Shop
{
    public class OrderViewModel
    {
        public OrderViewModel() { }
        public OrderViewModel(OrderDTO row) {
            OrderId = row.OrderId;
            UserId = row.UserId;
            CreatedD = row.CreatedD;
        }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedD { get; set; }


    }
    /**
     * Obszar adm order
     * */
    
    #region Admin
    public class OrderViewModelUse
    {
        //  OrderViewModel ViewModel = new OrderViewModel();
        public int OrdersNumber { get; set; }

        public string UserName { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQuantity { get; set; }
        public DateTime CreatedD { get; set; }



    }


    #endregion
}