using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string Image { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public Product() { }
        public Product(string name,double price,string image)
        {
            Name = name;
            Price = price;
            Image = image;
        }
    }
}
