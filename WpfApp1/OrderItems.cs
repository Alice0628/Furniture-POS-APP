﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class OrderItems
    {
        public int OrderId { get; set; }
        public int ProductId { get;set; }
        public string ProductName { get; set; }
        public string ProductQuantity { get; set; }
        public string ProductPrice { get; set; }

    }
}