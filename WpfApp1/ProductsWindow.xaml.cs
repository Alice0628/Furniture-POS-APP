using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        string filename;
        public ProductsWindow()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new FurnitureDbContext(); // Exceptions
                LvProducts.ItemsSource = Globals.dbContext.Products.ToList(); // equivalent of SELECT * FROM people
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                // Close();
                Environment.Exit(1);
            }
        }

        private void ResetFields()
        {
            TbxName.Text = "";
            TbxPrice.Text = "";
            TbxQuantity.Text = "";
            ImageBox.Source = null;
            LblImage.Visibility = Visibility.Hidden;
        }


        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            List<Product> pList = LvProducts.SelectedItems.Cast<Product>().ToList();

            if (pList.Count == 0)
            {
                MessageBox.Show(this, "No selectd items!", "File export error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<string> linesList = new List<String>();
            foreach (Product product in pList)
            {
                linesList.Add($"{product.Name};{product.Price};{product.Quantity}");
            }
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"D:\Full Stack Developer\(Course 14)Application Development I\Project\AppDev1Project\WpfApp1";
                saveFileDialog.Filter = "Text file (*.txt)|*.txt|Data file (*.data)|*.data|All Files| *.* ";//*.*, *.txt, *.data

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllLines(saveFileDialog.FileName, linesList);
                    MessageBox.Show(this, "Seletced Data saved to file", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    Status.Text = "Seletced Data saved to file Successfully!";
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error writing to file \n" + ex.Message, "File write error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void BtnAddImage_Click(object sender, RoutedEventArgs e)
        {
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Png file (*.png)|*.png|Bmp file (*.bmp)|*.bmp|All file (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    filename = openFileDialog.FileName;
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    ImageBox.Source = new BitmapImage(fileUri);
                    LblImage.Visibility = Visibility.Visible;
                    Status.Text = "Loaded image successfully!";
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                byte[] data = ImageToByteArray(System.Drawing.Image.FromFile(filename));

                if (!AreProductInputsValid()) return;
                string name = TbxName.Text;
                // NOTE: Some would say this validation is optional since we just validated it a moment ago
                double.TryParse(TbxPrice.Text, out double price);
                int.TryParse(TbxQuantity.Text, out int quantity);
                Product p1 = new Product { Name = name, Image = data, Quantity = quantity, Price = price };
                Globals.dbContext.Products.Add(p1);
                Globals.dbContext.SaveChanges();
                LvProducts.ItemsSource = Globals.dbContext.Products.ToList();
                LvProducts.Items.Refresh(); // tell ListView data has changed
                ResetFields();
                Status.Text = "Added product successfully!";
                MessageBox.Show(this, "Add success!", "write ", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product currSelProduct = LvProducts.SelectedItem as Product;
                var result = MessageBox.Show(this, "Are you sure you want to delete this item?\n" + currSelProduct.Name, "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                // delete - fetch then schedule for deletion, then save changes
                Product t3 = (from t in Globals.dbContext.Products where t.Id == currSelProduct.Id select t).FirstOrDefault<Product>();
                if (t3 != null)
                { // found the record to delete
                    Globals.dbContext.Products.Remove(t3); // schedule for deletion in the database
                    Globals.dbContext.SaveChanges(); // update the database to synchronize entities in memory with the database
                    LvProducts.ItemsSource = Globals.dbContext.Products.ToList();
                    Status.Text = "Deleted product successfully!";
                }
                else
                {
                    MessageBox.Show(this, "record to delete not found", "read error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Database operation failed: " + ex.Message, "write error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                MessageBox.Show(this, "Delete success!", "write ", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product currSelProduct = LvProducts.SelectedItem as Product;
                // UPDATE: equivalent of update - fetch then modify, save changes
                // FirstOrDefault will either return Person or null
                Product p2 = (from p in Globals.dbContext.Products where p.Id == currSelProduct.Id select p).FirstOrDefault<Product>();
                if (p2 != null)
                {
                    if (!AreProductInputsValid()) return;
                    p2.Name = TbxName.Text;
                    // NOTE: Some would say this validation is optional since we just validated it a moment ago
                    p2.Price = double.Parse(TbxPrice.Text);
                    p2.Quantity = int.Parse(TbxQuantity.Text);
                    byte[] data = ImageToByteArray(System.Drawing.Image.FromFile(filename));
                    p2.Image = data;
                    Globals.dbContext.SaveChanges(); // update the database to ;synchronize entities in memory with the database                 
                    LvProducts.ItemsSource = Globals.dbContext.Products.ToList();
                    ResetFields();
                    Status.Text = "Updated product successfully!";
                    MessageBox.Show(this, "Update success!", "write ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this, "record to update not found", "read error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Database operation failed:" + ex.Message, "write error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void LvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product currSelProduct = LvProducts.SelectedItem as Product;
            // Either this or Binding in XAML to enable/disable buttons
            BtnUpdate.IsEnabled = (currSelProduct != null);
            BtnDelete.IsEnabled = (currSelProduct != null);
            if (currSelProduct == null)
            {
                ResetFields();
            }
            else
            {
                TbxName.Text = currSelProduct.Name;
                TbxPrice.Text = currSelProduct.Price.ToString();
                TbxQuantity.Text = currSelProduct.Quantity.ToString();
                LblImage.Visibility = Visibility.Visible;
                System.Drawing.Image image = ByteArrayToImage(currSelProduct.Image);
                Random random = new Random();

                filename = "d:\\test" + random.Next(1000) + ".png";
                image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);

                ImageSourceConverter imgs = new ImageSourceConverter();
                ImageBox.SetValue(Image.SourceProperty, imgs.ConvertFromString(filename.ToString()));
            }
        }

        private bool AreProductInputsValid()
        {
            if (!Product.IsNameValid(TbxName.Text, out string errorName))
            {
                MessageBox.Show(this, errorName, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Product.IsPriceValid(TbxPrice.Text, out string errorPrice))
            {
                MessageBox.Show(this, errorPrice, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Product.IsQuantityValid(TbxQuantity.Text, out string errorQuantity))
            {
                MessageBox.Show(this, errorQuantity, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                return ms.ToArray();

            }
        }

        public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = System.Drawing.Image.FromStream(ms);
                return returnImage;
            }
        }

    }
}
