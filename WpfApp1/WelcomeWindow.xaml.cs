using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
       
        public WelcomeWindow()
        {
            InitializeComponent();
 
            Globals.dbContext = new FurnitureDbContext();

            //User user = new User("user1", "123", "user1@gmail.com");
            //Globals.dbContext.Users.Add(user);
            //Globals.dbContext.SaveChanges();

            Order order = new Order(1, 1, 12.5);
            Globals.dbContext.Orders.Add(order);
            Globals.dbContext.SaveChanges();
        }


        private void BtnToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lWin = new LoginWindow();
            lWin.Owner = this;
            lWin.Show();
        }
    }
}
