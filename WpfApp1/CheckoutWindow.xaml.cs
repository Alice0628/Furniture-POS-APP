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
using DataObject = System.Windows.DataObject;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Colors = QuestPDF.Helpers.Colors;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Permissions;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>

    public partial class CheckoutWindow : Window
    {
        string fileName;
        List<CheckoutItemList> items = new List<CheckoutItemList>();
        double totalPrice = 0;
        int orderIdForPrint = 0;

        public CheckoutWindow()
        {
            InitializeComponent();

            LvCheckoutList.ItemsSource = items;
        }

        public bool Export(string text)
        {
            try
            {
                Document.Create(Container =>
                {
                    Container.Page(page =>
                    {
                        page.Size(PageSizes.B4);
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                        //page.PageColor("#43a047");
                        page.DefaultTextStyle(x => x.FontSize(20));

                        //string text = "hello world";
                        page.Header()
                            .Text("Order details")
                            .SemiBold().FontSize(36).FontColor(Colors.Amber.Medium);
                        page.Content()
 
                            .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                            .Column(x =>
                            {

                                x.Spacing(20);
                                x.Item().Text(text);

                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(DateTime.Now.ToShortDateString());
                    });
                }).GeneratePdf(fileName + ".pdf");
                return true;
            }

            catch (IOException ex)
            {
                MessageBox.Show(this, "Failed to export to pdf file" + ex.Message, "Inner error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }


        private void btnCheckoutExport_Click(object sender, RoutedEventArgs e)
        {
            // prepare string list

            string data = "";
            data += "Custemor Id: " + TbxCustomerId.Text + "\n";

            data += "Issue date: " + DateTime.Now.ToShortDateString() + "\n";
            data += "---------------------------------------------------------------------------------" + "\n";
            data += String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15}\n\n", "Product Id", "Name", "Unitptice", "Quantity", "Price");

            
            //data += "Product Id" + "     " + "Name" + "           " + "Unitptice" + "      " + "Quantity" + "       " + "Price" + "\n";
            var allItems = GetAllSelectedItems();
            if (allItems == null) return;
            foreach (var item in allItems)
            {
                data += String.Format("{0,-20:N0} {1,20} {2,-20:N1} {3,-20:N0} {4,-20:N1}\n\n", item.ProductId, item.ProductName, item.UnitPrice, item.Quantity, item.Price );
            }
            data += "---------------------------------------------------------------------------------" + "\n";
            data += "Total Price: $" + LbTotalPrice.Content;
          
          
            bool ifExport = Export(data);
            if (ifExport)
            {
                BtnPrint.IsEnabled = true;
                MessageBox.Show(this, "Export to " + fileName + ".pdf is successful!", "Success information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool CheckIfSufficient(Product currentProduct, int quantity)
        {
            if (quantity > currentProduct.Quantity)
            {

                MessageBox.Show(this, "There are only " + currentProduct.Quantity + " " + currentProduct.Name, "Product items insufficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BtnExport.IsEnabled = false;
            BtnPrint.IsEnabled = false;
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
  
               bool ifSufficient = CheckIfSufficient(currentProduct, quantity);
               
               if(ifSufficient)
                {
                    CheckoutItemList cItem = new CheckoutItemList(currentProduct.Id, currentProduct.Name, currentProduct.Price, quantity);
                    items.Add(cItem);

                // refresh CheckoutListView
                   LvCheckoutList.Items.Refresh();
                   ResetFields();
                }   
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

            // claculate the price and decrease the quantity in products list
            Product currentProduct;
            foreach (var item in lvItemsList)
            {
                totalPrice += item.Price;
                currentProduct = GetCurrentProduct(item.ProductId);
                currentProduct.Quantity -= Convert.ToInt32(item.Quantity);
                Globals.dbContext.SaveChanges();
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
                //Order lastOrder = (from o in Globals.dbContext.Orders where o.CustomerId == customerId && o.UserId == Globals.userId && o.TotalPaied == totalPrice select o).LastOrDefault<Order>();
                //if (lastOrder == null) return; 
                orderIdForPrint = order.OrderId;
                fileName = "Order-" + orderIdForPrint;
                // save to OrderItems table
                Product curProduct;
                foreach (var item in lvItemsList)
                {
                    curProduct = GetCurrentProduct(item.ProductId);
                    OrderItem orderItem = new OrderItem(order.OrderId, item.ProductId, item.ProductName, item.Quantity, totalPrice);
                    Globals.dbContext.OrderItems.Add(orderItem);
                    Globals.dbContext.SaveChanges();
                    BtnExport.IsEnabled = true;
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
                    if (item.ProductId == selectedItem.ProductId)
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
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    if (e.Source != null)
            //    {
            //        CheckoutItemList selectedItem = (CheckoutItemList)LvCheckoutList.SelectedItem;

            //        DragDrop.DoDragDrop(LvCheckoutList, selectedItem, DragDropEffects.Move);
            //    }
            //}

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source != null)
                {
                    //List<DataModel> myList = new List<DataModel>();
                    //foreach (DataModel Item in lvUsers.SelectedItems)
                    //{
                    //    myList.Add(Item);
                    //}
                    var selectedItems = LvCheckoutList.SelectedItems;

                    DataObject dataObject = new DataObject(LvCheckoutList);
                    DragDrop.DoDragDrop(LvCheckoutList, dataObject, DragDropEffects.Move);
                }
            }
        }

        private void BtnDelete_Drop(object sender, DragEventArgs e)
        {
            //Type myType = typeof(CheckoutItemList);
            //string modelns = myType.FullName;

            ////CheckoutItemList selectedItem = e.Data.GetData(modelns) as CheckoutItemList;
            //CheckoutItemList selectedItem = LvCheckoutList.SelectedItem as CheckoutItemList;
            //if (selectedItem == null) return;
            //var result = MessageBox.Show(this, "Are you sure you want to delete " + selectedItem.ProductId.ToString(), "Deletion conformation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            //if (result != MessageBoxResult.OK) return;

            //items.Remove(selectedItem);
            //LvCheckoutList.Items.Refresh();
            //MessageBox.Show(this, selectedItem +  "has been deleted " + selectedItem.ProductId.ToString(), "Deletion conformation", MessageBoxButton.OKCancel, MessageBoxImage.Information);




            Type myType = typeof(List<CheckoutItemList>);

            var selectedItems = LvCheckoutList.SelectedItems;
            if (selectedItems == null) return;
            List<string> nameArr = new List<string>();
            string msg = String.Join(",", nameArr);
            var result = MessageBox.Show(this, "Are you sure you want to delete " + msg, "Deletion conformation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result != MessageBoxResult.OK) return;
            //CheckoutItemList[] sItemsArr = selectedItems.ToArray();

            foreach (CheckoutItemList sm in selectedItems)
            {
                nameArr.Add(sm.ProductName);
                items.Remove(sm);
            }

            LvCheckoutList.Items.Refresh();
            ResetFields();
            MessageBox.Show(this, msg + " have been deleted.", "Deletion conformation", MessageBoxButton.OKCancel, MessageBoxImage.Information);

        }

        private void lb_mouseUp(object sender, MouseButtonEventArgs e)
        {
            CustomerWindow cWin = new CustomerWindow();
            cWin.Owner = this;
            cWin.Show();
        }



        private bool Print(string pathStr)
        {
            try
            {
                if (File.Exists(pathStr) == false)
                    return false;

                var pr = new Process
                {
                    StartInfo =
                    {
                        FileName = pathStr,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Normal,
                        Verb = "Print"
                    }
                };
                pr.Start();
                MessageBox.Show(this, "succeed to print this file", "succession", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "failed to print this file" + ex.Message, "Printing error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            Print(fileName);
        }
    }



}

