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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();

            try
            {
                Globals.dbContext = new FurnitureDbContext();

                FindCurrentUser();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        public void FindCurrentUser()
        {
            var currentUser = Globals.dbContext.Users.Where(u => u.UserId == Globals.userId).FirstOrDefault();
            if (currentUser == null) return;

            TbxName.Content = currentUser.UserName;
            TbxEmail.Content = currentUser.Email;
            var password = currentUser.Password;
        }

        private void OpenEditUserDialog_Click(object sender, RoutedEventArgs e)
        {
            EditUserProfileDialog editUserProfileDialog = new EditUserProfileDialog();
            editUserProfileDialog.Owner = this;
            editUserProfileDialog.ShowDialog();

            FindCurrentUser();
        }
    }
}
