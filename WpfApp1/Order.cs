using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
        //public DateTime OrderDate
        //{
        //    get
        //    {
        //        return _orderDate;
        //    }
        //    set
        //    {
        //        _orderDate = DateTime.Now;
        //    }
        //} 
       
        [Required]
        public double TotalPaied { get; set; }

        public Order() { }
        public Order(int customerId, int userId, double totalPaied)
        {
            CustomerId = customerId;
            UserId = userId;
            TotalPaied = totalPaied;
            OrderDate = (DateTime)DateTime.Now;

        }
    }
}
