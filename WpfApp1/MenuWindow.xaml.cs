using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public string UserName { get; set; }
        public MenuWindow(string username)
        {
            InitializeComponent();
            UserName = username;
            tbxUser.Text = UserName;
        }

        
      

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow pWin = new ProfileWindow();
            pWin.Owner = this;
            pWin.Show();

        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow cWin = new CustomerWindow();
            cWin.Owner = this;
            cWin.Show();

        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow pdWin = new ProductsWindow();
            pdWin.Owner = this;
            pdWin.Show();

        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow oWin = new OrdersWindow();
            oWin.Owner = this;
            oWin.Show();

        }

        private void btnChecout_Click(object sender, RoutedEventArgs e)
        {
            CheckoutWindow pWin = new CheckoutWindow();
            pWin.Owner = this;
            pWin.Show();

        }
    }
}
