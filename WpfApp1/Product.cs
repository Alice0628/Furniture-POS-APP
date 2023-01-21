using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public byte[] Image { get; set; }

        public int Quantity { get; set; }

        public Product(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public Product() { }
        public Product(string name, double price, byte[] image, int quantity)
        {
            Name = name;
            Price = price;
            Image = image;
            Quantity = quantity;
        }

        public static bool IsNameValid(string name, out string error)// Name must be 2-50 characters or number long, no special characters 
        {

            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9]{2,50}$"))
            {
                error = "Name must be 2-100 characters long, no special characters";
                return false;
            }
            error = null;
            return true;

        }

        public static bool IsPriceValid(string strPrice, out string error)// Price must be digits 
        {

            if (!double.TryParse(strPrice, out double price))
            {
                error = "Price must be an double value";
                return false;
            }
            error = null;
            return true;
        }

        public static bool IsQuantityValid(string strQuantity, out string error)// Quantity must be digits 
        {

            if (!int.TryParse(strQuantity, out int quantity))
            {
                error = "Quantity must be an integer value";
                return false;
            }
            error = null;
            return true;

        }

    }
}
