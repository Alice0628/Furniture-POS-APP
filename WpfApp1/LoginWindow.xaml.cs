using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnSignin_Click(object sender, RoutedEventArgs e)
        {
            // get the input value and do validatin
            string email = tbxEmail.Text;
            string pass = tbxPass.Text;

            // look for the mathed user in database
            User user = (from u in Globals.dbContext.Users where u.Email == email && u.Password == pass select u).FirstOrDefault<User>();

            if (user == null)
            {
                MessageBox.Show(this, "Email or password not correct, please try aigin!", "Indentity verification failed", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // if found, go to profile window; if no, show messagebox and stay in this page

            Globals.userId = user.UserId;
            MenuWindow mWin = new MenuWindow(user.UserName);
            mWin.Owner = this;
            mWin.Show();

        }
    }
}
