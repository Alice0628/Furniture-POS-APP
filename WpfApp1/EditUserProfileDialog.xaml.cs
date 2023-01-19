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
    /// Interaction logic for EditUserProfileDialog.xaml
    /// </summary>
    public partial class EditUserProfileDialog : Window
    {
        public EditUserProfileDialog()
        {
            InitializeComponent();
            Globals.dbContext = new FurnitureDbContext();

        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Globals.dbContext.Users.Where(u => u.UserId == 1).FirstOrDefault();
            if (currentUser == null) return;

            currentUser.UserName = TbxName.Text;
            currentUser.Email = TbxEmail.Text;

            if (TbxPassword.Text != currentUser.Password)
            {
                MessageBox.Show("Please check your password", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Globals.dbContext.SaveChanges();
                Close();
            }


        }
    }
}
