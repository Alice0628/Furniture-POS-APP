using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

using System.ComponentModel;
using System.Drawing;


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

            LvCheckoutList.ItemsSource = items;
        }

        private void btnCheckoutExport_Click(object sender, RoutedEventArgs e)
        {
            // prepare string list
            List<string> data = new List<string>();
            if (TbxCustomerId.Text != "")
            {
                data.Add("Custemor Id: " + TbxCustomerId.Text);
            }
            var allItems = GetAllSelectedItems();
            if (allItems == null) return;
            foreach (var item in allItems)
            {
                data.Add(item.ProductId.ToString() + "          " + item.UnitPrice.ToString() + "          " + item.Quantity.ToString() + "          " + item.Price.ToString());
            }
            data.Add("---------------------------------------------------------------------------------");
            data.Add("Total Price: $" + LbTotalPrice.Content);
            // instantiate a saveFileDialog object

            // save to file
            try
            {
                using (PdfDocument document = new PdfDocument())
                {
                    //Add a page to the document.
                    PdfPage page = document.Pages.Add();
                    //Create PDF graphics for a page.
                    PdfGraphics graphics = page.Graphics;
                    //Set the standard font.
                    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
                    //Draw the text.
                    foreach (string d in data)
                    {
                        graphics.DrawString(d, font, PdfBrushes.Black, new PointF(0, 0));
                    }
                    //Save the document.
                    document.Save("test1.pdf");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "Failed to export to pdf file" + ex.Message, "Inner error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }


        private int GetProductId()
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


        private void ResetFields()
        {
            TbxProductId.Text = "";
            TbxQuantity.Text = "";
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            // get the input data 
            try
            {
                if (!int.TryParse(TbxQuantity.Text, out int quantity))
                {
                    throw new FormatException("Please enter an integer");
                }
                int productId = GetProductId();
                foreach (CheckoutItemList item in items)
                {
                    if (item.ProductId == productId)
                    {
                        
                        item.Quantity += quantity;
                        item.Price = item.UnitPrice * item.Quantity;
                        LvCheckoutList.Items.Refresh();
                        LvCheckoutList.SelectedIndex = -1;
                        return;
                    }
                }

                // find the current product info
                Product currentProduct = GetCurrentProduct(productId);


                // instantize a checkoutItemList object and add to items
                CheckoutItemList cItem = new CheckoutItemList(currentProduct.Id, currentProduct.Name, currentProduct.Price, quantity);

                items.Add(cItem);

                // refresh CheckoutListView
                LvCheckoutList.Items.Refresh();
                ResetFields();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, ex.Message, "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }


        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            // get all listview items
            List<CheckoutItemList> lvItemsList = GetAllSelectedItems();

            // claculate the total price)
            foreach (var item in lvItemsList)
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
                    throw new FormatException("Invalid customer Id");

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
                MessageBox.Show(this, "Customer Id should be an integer" + ex.Message, "Ivalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Failed to create order" + ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            CheckoutItemList selectedItem = LvCheckoutList.SelectedItem as CheckoutItemList;
            if (selectedItem == null) return;
            CheckoutItemList curItem = items.Find(p => p.ProductId == selectedItem.ProductId);
            try
            {
                if (!int.TryParse(TbxProductId.Text, out int productId))
                {
                    throw new FormatException("Product Id should be an integer");
                }
                if (!int.TryParse(TbxQuantity.Text, out int quantity))
                {
                    throw new FormatException("Product Id should be an integer");
                }

                foreach (CheckoutItemList item in items)
                {
                    if(item.ProductId == selectedItem.ProductId)
                    {
                        item.ProductId = productId;
                        item.Quantity = quantity;
                        item.Price = item.UnitPrice * item.Quantity;
                        LvCheckoutList.SelectedIndex = -1;
                    }
                }
                LvCheckoutList.Items.Refresh();
                ResetFields();
                LvCheckoutList.SelectedIndex = -1;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, ex.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LvOnChanged_Click(object sender, SelectionChangedEventArgs e)
        {
            CheckoutItemList selectedItem = LvCheckoutList.SelectedItem as CheckoutItemList;
            if (selectedItem == null) return;
            TbxProductId.Text = selectedItem.ProductId.ToString();
            TbxQuantity.Text = selectedItem.Quantity.ToString();
        }



        private void lstItems_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source != null)
                {
                    CheckoutItemList selectedItem = (CheckoutItemList)LvCheckoutList.SelectedItem;

                    DragDrop.DoDragDrop(LvCheckoutList, selectedItem, DragDropEffects.Move);
                }
            }
        }

        private void BtnDelete_Drop(object sender, DragEventArgs e)
        {
            Type myType = typeof(CheckoutItemList);
            string modelns = myType.FullName;

            //CheckoutItemList selectedItem = e.Data.GetData(modelns) as CheckoutItemList;
            CheckoutItemList selectedItem = LvCheckoutList.SelectedItem as CheckoutItemList;
            var result = MessageBox.Show(this, "Are you sure you want to delete " + selectedItem.ProductId.ToString(), "Deletion conformation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result != MessageBoxResult.OK) return;

            items.Remove(selectedItem);
            LvCheckoutList.Items.Refresh();


        }

        private void lb_mouseUp(object sender, MouseButtonEventArgs e)
        {
            CustomerWindow cWin = new CustomerWindow();
            cWin.Owner = this;
            cWin.Show();
        }
    }
}

