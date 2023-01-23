using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Customer
    {
        public bool EmailIsValid(string email)
        {
            string pattern = @"^[A-Za-z\d_.]{1,10}@[A-Za-z\d]{1,10}.[A-Za-z\d]{1,10}$";
            return Regex.IsMatch(email, pattern);
        }
        public bool PhoneIsValid(string phone)
        {
            string pattern = @"^1\d{10}$";
            return Regex.IsMatch(phone, pattern);
        }

        public bool FullAddressIsValid(string address)
        {
            if (address == null || address == "") return true;
            else
            {
                string[] data = address.Split(',');
                if (data.Length != 5) return false;
                else return true;
            }
        }

        public bool AddCustomer()
        {
            if (EmailIsValid(Email) && PhoneIsValid(Phone) && FullAddressIsValid(FullAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
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
            get
            {
                return _phone;
            }
            set
            {
                Regex regex = new Regex(@"^1\d{10}$");
                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException(@"Phone number must start with '1', and totally exactly 10 digis");
                }
                _phone = value;
            }
        }

        [StringLength(100)]
        public string FullAddress { get; set; }


        public Customer() { }
        public Customer(string fullName, string email, string phone, string fullAddress)
        {
            FullName = fullName;
            Email = email;
            Phone = phone;
            FullAddress = fullAddress;
        }

        public Customer(string fullName, string email, string phone)
        {
            FullName = fullName;
            Email = email;
            Phone = phone;
        }
    }
}
