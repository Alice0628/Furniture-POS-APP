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
            Product p = new Product("chair", 12.3);
            User u = new User("Marry");
            dbContext.Products.Add(p);
            dbContext.Users.Add(u);
            dbContext.SaveChanges();
        }

    }
}
