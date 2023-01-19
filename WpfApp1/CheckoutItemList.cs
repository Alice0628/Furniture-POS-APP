using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class CheckoutItemList
    {
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }

        public CheckoutItemList() {  }

        public CheckoutItemList (int id, double uPrice, int quantity)
        {
            ProductId = id;
            UnitPrice = uPrice;
            Quantity = quantity;
            Price = UnitPrice * Quantity;
        }
    }
}
