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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
   
    public partial class CheckoutWindow : Window
    {
        string fileName = "";
        List<CheckoutItemList> items = new List<CheckoutItemList>();
        double totalPrice = 0;
        int orderIdForPrint = 0;

        public CheckoutWindow()
        {
            InitializeComponent();

            LvCheckoutList.ItemsSource = items;
        }

        private void btnCheckoutExport_Click(object sender, RoutedEventArgs e)
        {
            // prepare string list
            List<string> data = new List<string>();
            
            data.Add("Custemor Id: " + TbxCustomerId.Text);

            data.Add("Issue date: " + DateTime.Now.ToShortDateString());
            data.Add("---------------------------------------------------------------------------------");
            data.Add(String.Format("{0,6} {1,25} {1,25} {1,25} {1,25}\n\n", "Product Id", "Name", "Unitptice", "Quantity", "Price"));
            var allItems = GetAllSelectedItems();
            if (allItems == null) return;
            foreach (var item in allItems)
            {
                data.Add(String.Format("{0,6} {1,25} {1,25} {1,25} {1,25}\n\n", item.ProductId.ToString() + item.ProductName + item.UnitPrice.ToString() +  item.Quantity.ToString()+  item.Price.ToString()));
            }
            data.Add("---------------------------------------------------------------------------------");
            data.Add("Total Price: $" + LbTotalPrice.Content);
            string text = "";
            foreach (string d in data)
            {
                text += d + "\n";
            }

            // save to file
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
                                    
                                    x.Spacing(40);
                                    x.Item().Text(text);
                                    
                                });

                            page.Footer()
                                .AlignCenter()
                                .Text(DateTime.Now.ToShortDateString());
                        });
                    }).GeneratePdf(fileName + ".pdf");
                    MessageBox.Show(this, "Export to " + fileName +".pdf is successful!", "Success information", MessageBoxButton.OK, MessageBoxImage.Information);
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

            // claculate the price)
            foreach (var item in lvItemsList)
            {
                totalPrice += item.Price;
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
                orderIdForPrint = order.OrderId;
                fileName = "Order - " + orderIdForPrint.ToString();  
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

