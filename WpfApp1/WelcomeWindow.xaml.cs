using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        FurnitureDbContext dbContext = new FurnitureDbContext();

        public WelcomeWindow()
        {
            InitializeComponent();
            try
            {
            FurnitureDbContext dbContext = new FurnitureDbContext();
            Product p = new Product("chair", 12.3);
            Customer c = new Customer("Zhang", "Xiao", "zxiao0628@gmail.com", "15145628870");
            OrderItem item = new OrderItem(1, 1, "chair", 1, 12.3);
            dbContext.Products.Add(p);
            dbContext.Customers.Add(c);
            dbContext.OrderItems.Add(item);
            Order order = new Order(1, 1, 12.3);
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //catch (ArgumentException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //catch(SystemException ex) 
            //{
            //    MessageBox.Show(ex.Message);
            //}
           

        }

    }
}
