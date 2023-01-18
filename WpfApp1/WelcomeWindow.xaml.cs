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

            //// Default user
            //User user1 = new User();
            //user1.UserName = "Admin";
            //user1.Password = "abcdefgA1!";
            //user1.Email = "admin@test.com";
            //Globals.dbContext.Users.Add(user1);

            //User user2 = new User();
            //user2.UserName = "Employee";
            //user2.Password = "abcdefgA2@";
            //user2.Email = "employee@test.com";
            //Globals.dbContext.Users.Add(user2);

            //Globals.dbContext.SaveChanges();

        }

    }
}
