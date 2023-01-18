using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get;set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }

        public OrderItem(int orderItemId, int productId, string productName, int productQuantity, double productPrice)
        {
            OrderItemId = orderItemId;
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            ProductPrice = productPrice;
        }
    }
}
