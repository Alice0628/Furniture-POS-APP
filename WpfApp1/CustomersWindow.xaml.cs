using Microsoft.Win32;
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
        public CustomerWindow()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new FurnitureDbContext();
                LvCustomers.ItemsSource = Globals.dbContext.Customers.ToList();
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
                string fullName = TbxFirstName.Text + "," + TbxLastName.Text;
                string email = TbxEmail.Text;
                string phone = TbxPhone.Text;
                string fullAddress;
                if (TbxStreet.Text == "")
                {
                    fullAddress = null;
                }
                else
                {
                    fullAddress = TbxStreet.Text + "," + TbxCity.Text + "," + TbxProvince.Text + "," + TbxPostalCode.Text + "," + TbxCountry.Text;
                }

                Customer customer = new Customer(fullName, email, phone, fullAddress);
                Globals.dbContext.Customers.Add(customer);
                Globals.dbContext.SaveChanges();

                LvCustomers.ItemsSource = Globals.dbContext.Customers.ToList();
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

        private void LvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)LvCustomers.SelectedItem;
            if (selectedCustomer == null)
            {
                Status.Text = "Customer Unselected";
                ResetFileds();
            }
            else
            {
                // separate Full Name
                string fullName = selectedCustomer.FullName;
                string[] nameData = fullName.Split(',');
                if (nameData.Length != 2)
                {
                    TbxFirstName.Text = nameData[0];
                    TbxLastName.Text = null;
                }
                else
                {
                    TbxFirstName.Text = nameData[0];
                    TbxLastName.Text = nameData[1];
                }

                TbxEmail.Text = selectedCustomer.Email;
                TbxPhone.Text = selectedCustomer.Phone;

                // separate Full Address
                string fullAddress = selectedCustomer.FullAddress;
                if (fullAddress == null)
                {
                    TbxStreet.Text = null;
                    TbxCity.Text = null;
                    TbxProvince.Text = null;
                    TbxPostalCode.Text = null;
                    TbxCountry.Text = null;
                }
                else
                {
                    string[] addressData = fullAddress.Split(',');
                    TbxStreet.Text = addressData[0];
                    TbxCity.Text = addressData[1];
                    TbxProvince.Text = addressData[2];
                    TbxPostalCode.Text = addressData[3];
                    TbxCountry.Text = addressData[4];
                }
                Status.Text = "Customer Selected";
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)LvCustomers.SelectedItem;
            if (selectedCustomer == null) return;

            try
            {
                int id = selectedCustomer.CustomerId;
                var item = Globals.dbContext.Customers.Find(id);
                if (item == null)
                {
                    MessageBox.Show(this, "Not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                selectedCustomer.FullName = TbxFirstName.Text + "," + TbxLastName.Text;
                selectedCustomer.Email = TbxEmail.Text;
                selectedCustomer.Phone = TbxPhone.Text;
                if (TbxStreet.Text == "")
                {
                    selectedCustomer.FullAddress = null;
                }
                else
                {
                    selectedCustomer.FullAddress = TbxStreet.Text + "," + TbxCity.Text + "," + TbxProvince.Text + "," + TbxPostalCode.Text + "," + TbxCountry.Text;
                }
                Globals.dbContext.SaveChanges();

                LvCustomers.ItemsSource = Globals.dbContext.Customers.ToList();
                LvCustomers.SelectedIndex = -1;
                Status.Text = "Updated Successfully.";
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = LvCustomers.SelectedItems;
            if (selectedCustomer == null) return;

            var result = MessageBox.Show(this, "Are you sure to delete?\n", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            foreach (Customer c in selectedCustomer)
            {
                Globals.dbContext.Customers.Remove(c);
            }
            Globals.dbContext.SaveChanges();

            LvCustomers.ItemsSource = Globals.dbContext.Customers.ToList();
            LvCustomers.SelectedIndex = -1;
            Status.Text = "Deleted Successfully.";
            ResetFileds();
        }

        public void SaveToFile()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Data files (*.data)|*.data|Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

                if (saveFileDialog.ShowDialog() == true)
                {
                    List<string> data = new List<string>();
                    var customerList = LvCustomers.SelectedItems;
                    foreach (Customer c in customerList)
                    {
                        data.Add($"{c.FullName};{c.Email};{c.Phone};{c.FullAddress}");
                    }
                    File.WriteAllLines(saveFileDialog.FileName, data);
                    Status.Text = "Save Successfully";
                }
                else
                {
                    Status.Text = "Save Failed";
                    return;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("System error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
