using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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

        public bool VerifyIdentity(string email, string pass)
        {
            // look for the mathed user in database
            try
            {
                User user = (from u in Globals.dbContext.Users where u.Email == email && u.Password == pass select u).FirstOrDefault<User>();
                if (user == null)
                {
                    MessageBox.Show(this, "Email or password not correct, please try aigin!", "Indentity verification failed", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

                //Globals.userId = user.UserId;
                //MenuWindow mWin = new MenuWindow(user.UserName);
                //mWin.Owner = this;
                //mWin.Show();
                return true;
            }
            catch(SystemException ex)
            {
                MessageBox.Show(this, "Fail to read database!" + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void BtnSignin_Click(object sender, RoutedEventArgs e)
        {
            string email = tbxEmail.Text;
            string pass = tbxPass.Password;
            VerifyIdentity(email, pass);

            User user = (from u in Globals.dbContext.Users where u.Email == email && u.Password == pass select u).FirstOrDefault<User>();

            Globals.userId = user.UserId;
            this.Visibility = Visibility.Collapsed;
            MenuWindow mWin = new MenuWindow(user.UserName);
            mWin.Owner = this;
            mWin.ShowDialog();

            Visibility = Visibility.Visible;
        }
    }
}
