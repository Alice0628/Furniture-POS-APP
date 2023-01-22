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
    /// Interaction logic for OrderDetailDialog.xaml
    /// </summary>
    public partial class OrderDetailDialog : Window
    {
        public OrderDetailDialog()
        {
            InitializeComponent();
            Globals.dbContext = new FurnitureDbContext();
           

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(LvOrderDetail, "My First Print Job");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
