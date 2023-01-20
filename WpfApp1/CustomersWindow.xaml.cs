using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    
    public partial class CustomerWindow : Window
    {
        List<CustomerConverter> list = new List<CustomerConverter>();
        public CustomerWindow()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new FurnitureDbContext();
                LvCustomers.ItemsSource = list;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error reading from database: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        public void ResetFileds()
        {
            TbxFirstName.Text = null;
            TbxLastName.Text = null;
            TbxEmail.Text = null;
            TbxPhone.Text = null;
            TbxStreet.Text = null;
            TbxCity.Text = null;
            TbxProvince.Text = null;
            TbxPostalCode.Text = null;
            TbxCountry.Text = null;
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // database
                string firstname = TbxFirstName.Text;
                string lastname = TbxLastName.Text;
                string email = TbxEmail.Text;
                string phone = TbxPhone.Text;
                string street = TbxStreet.Text;
                string city = TbxCity.Text;
                string province = TbxProvince.Text;
                string postalcode = TbxPostalCode.Text;
                string country = TbxCountry.Text;

                Customer customer = new Customer(lastname, firstname, email, phone, street, city, postalcode, country, province);
                Globals.dbContext.Customers.Add(customer);
                Globals.dbContext.SaveChanges();

                // converter
                CustomerConverter c = new CustomerConverter();
                c.FullName = firstname + " " + lastname;
                c.FullAddress = street + ", " + city + " " + province + " " + postalcode + ", " + country;
                c.Email = email;
                c.Phone = phone;
                list.Add(c);

                LvCustomers.Items.Refresh();
                LvCustomers.SelectedIndex = -1;

                Status.Text = "Added Successfully.";
                ResetFileds();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void SaveToFile()
        {

        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.dbContext = new FurnitureDbContext();
            var customerList = Globals.dbContext.Customers.ToList();

            foreach ( var customer in customerList )
            {
                CustomerConverter c = new CustomerConverter();
                c.FullName = customer.FirstName + " " + customer.LastName;
                c.Email = customer.Email;
                c.Phone = customer.Phone;
                c.FullAddress = customer.Street + ", " + customer.City + " " + customer.Province + " " + customer.PostalCode + ", " + customer.Country;
                list.Add(c);
            }
            LvCustomers.Items.Refresh();
            LvCustomers.SelectedIndex = -1;
        }
    }
}
