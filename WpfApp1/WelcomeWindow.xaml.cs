using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
       
        public WelcomeWindow()
        {
            InitializeComponent();
 
            Globals.dbContext = new FurnitureDbContext();

            Product product = new Product("chair", 12.5, 10);
            Globals.dbContext.Products.Add(product);
            Product product2 = new Product("table", 60.0, 20);
            Globals.dbContext.Products.Add(product2);
           

            User user = new User("John Smith", "123", "john@gmail.com");
            Globals.dbContext.Users.Add(user);
          

            Customer customer = new Customer("Smith John", "john@gmail.com", "11234567890");
            Globals.dbContext.Customers.Add(customer);
            Globals.dbContext.SaveChanges();
            //Customer customer = new Customer("Smith", "john@gmail.com", "11234567890");
            //Globals.dbContext.Customers.Add(customer);
           

        }


        private void BtnToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lWin = new LoginWindow();
            lWin.Owner = this;
            lWin.Show();
        }
    }
}
