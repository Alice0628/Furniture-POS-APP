using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
       
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ProductId { get;set; }
        
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public int ProductQuantity { get; set; }

        [Required] 
        public double ProductPrice { get; set; }

        //test

        OrderItem() { }
        public OrderItem(int orderId,int productId, string productName, int productQuantity, double productPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            ProductPrice = productPrice;
        }
    }
}
