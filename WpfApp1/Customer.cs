using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp1
{
   public class Customer
    {
        public int CustomerId{ get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]

        private string _email;
        public string Email {
            get
            { 
                return _email;
            } set
            {
                Regex regex = new Regex(@"^[A-Za-z\d_.]{1,10}@[A-Za-z\d]{1,10}.[A-Za-z\d]{1,10}$");
                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException("Invalid email format");
                }
                _email = value;
            }
        }
        [Required]

        private string _phone;
        public string Phone
        {
            get {
                return _phone;
            }
            set {
                Regex regex = new Regex(@"^1\d{10}$");
                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException(@"Phone number must start with '1', and totally exactly 10 digis");
                }
                _phone = value;
            }
        }
        
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string Province { get; set; }

        public Customer(string lastName, string firstName, string email, string phone, string street, string city, string postalCode, string country, string province)
        {
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Phone = phone;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
            Province = province;
        }

        public Customer(string lastName, string firstName, string email, string phone)
        {
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Phone = phone;
        }
    }
}
