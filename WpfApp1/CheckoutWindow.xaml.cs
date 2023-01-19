using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        List<CheckoutItemList> items = new List<CheckoutItemList>();
        double totalPrice = 0;
        
        public CheckoutWindow()
        {
            InitializeComponent();
            
            LvCheckoutList.ItemsSource= items;
        }

        private void btnCheckoutExport_Click(object sender, RoutedEventArgs e)
        {

        }


        private int GetProductId ()
        {
            if (!int.TryParse(TbxProductId.Text, out int productId))
            {
                throw new FormatException("Please enter an integer");
            }
            return productId;
        }
        private Product GetCurrentProduct(int productId)
        {
                
                
                Product currentProduct = (from p in Globals.dbContext.Products where p.Id == productId select p).FirstOrDefault<Product>();
                if (currentProduct == null)
                {
                    throw new SystemException("Failed to read from database");
                }
                if (currentProduct == null)
                {
                    throw new SystemException("Invalid Product Item");
                }
                return currentProduct;
            
        }

        private List<CheckoutItemList> GetAllSelectedItems()
        {
            LvCheckoutList.SelectAll();
            var lvItemsList = LvCheckoutList.SelectedItems.Cast<CheckoutItemList>().ToList();
            return lvItemsList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

                // get the input data 
             try {
                int productId = GetProductId();
                    if (!int.TryParse(TbxQuantity.Text, out int quantity))
                    {
                        throw new FormatException("Please enter an integer");
                    }

                    // find the current product info
                    Product currentProduct = GetCurrentProduct(productId);

            // instantize a checkoutItemList object and add to items
               CheckoutItemList cItem = new CheckoutItemList(currentProduct.Id, currentProduct.Price, quantity);
               
               items.Add(cItem);

            // refresh CheckoutListView
               LvCheckoutList.Items.Refresh();
            }
            catch(FormatException ex)
            {
                MessageBox.Show(this, ex.Message, "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(SystemException ex)
            {
                MessageBox.Show(this, ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
               
  
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            // get all listview items
            List<CheckoutItemList> lvItemsList = GetAllSelectedItems();

            // claculate the total price)
            foreach (var item in lvItemsList )
            {
                totalPrice += item.UnitPrice;
            }
            LbTotalPrice.Content = totalPrice;
           
            // save to Orders table
            if (TbxCustomerId.Text == "")
            {
                // do not save to Orders table
                return;
            }
            Order order;
            try
            {
              // save to Orders list
            if (!int.TryParse(TbxCustomerId.Text, out int customerId))
            {
                throw new FormatException ("Invalid customer Id");
               
            }
              order = new Order(customerId, Globals.userId, totalPrice);
              Globals.dbContext.Orders.Add(order);
              Globals.dbContext.SaveChanges();
              // save to OrderItems table
              Product currentProduct;
            foreach (var item in lvItemsList)
                {
                    currentProduct = GetCurrentProduct(item.ProductId);
                    OrderItem orderItem = new OrderItem(order.OrderId, item.ProductId, currentProduct.Name, item.Quantity, totalPrice);
                    Globals.dbContext.OrderItems.Add(orderItem);
                    Globals.dbContext.SaveChanges();
                }
            } 
            catch (FormatException ex)
            {
                MessageBox.Show(this,"Customer Id should be an integer" + ex.Message, "Ivalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Failed to create order" + ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
