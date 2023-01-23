using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;
using Window = Microsoft.Office.Interop.Excel.Window;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : System.Windows.Window
    {
        string fileName = Directory.GetCurrentDirectory() + "\\OrdersList.xlsx";
        public OrdersWindow()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new FurnitureDbContext(); // Exceptions
                LvOrders.ItemsSource = Globals.dbContext.Orders.ToList(); // equivalent of SELECT * FROM people
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                // Close();
                Environment.Exit(1);
            }
        }


        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            List<Order> oList = Globals.dbContext.Orders.ToList();

            if (oList.Count == 0)
            {
                MessageBox.Show(this, "No selectd items!", "File export error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                //Create an object of Excel application
                Microsoft.Office.Interop.Excel.Application excelAPP = null;
                Microsoft.Office.Interop.Excel.Workbook WB = null;
                Microsoft.Office.Interop.Excel.Worksheet WS = null;
                //Microsoft.Office.Interop.Excel.Range Range = null;

                //Create an object of Workbook
                excelAPP = new Microsoft.Office.Interop.Excel.Application();
                //WB = excelAPP.Workbooks.Add(fileName);
                //WB = OpenExcelApp(fileName, 1);
                //WB = excelAPP.Workbooks.Add("D:\\MyExcel.xlsx");
                WB = excelAPP.Workbooks.Open(fileName);
                WS = (Microsoft.Office.Interop.Excel.Worksheet)WB.Sheets[1];
                
                //CreateHeader;
                WS.Cells[1, 1] = "OrderId";
                WS.Cells[1, 2] = "CustomerId";
                WS.Cells[1, 3] = "UserId";
                WS.Cells[1, 4] = "OrderDate";
                WS.Cells[1, 5] = "TotalPaied";

                //InsertData;
                int ind = 2;
                foreach (Order order in oList)
                {
                    //DataRow DR = DRV.Row;
                    //for (int ind1 = 0; ind1 < oList.Count; ind1++)
                    //{
                    WS.Cells[1][ind] = order.OrderId;
                    WS.Cells[2][ind] = order.CustomerId;
                    WS.Cells[3][ind] = order.UserId;
                    WS.Cells[4][ind] = order.OrderDate;
                    WS.Cells[5][ind] = order.TotalPaied;
                    //}
                    ind++;
                  
                }

                //CloseExcelApp();
                if (excelAPP.ActiveWorkbook != null)
                    excelAPP.ActiveWorkbook.Save();
                if (excelAPP != null)
                {
                    if (WB != null)
                    {
                        if (WS != null)
                            Marshal.ReleaseComObject(WS);
                        WB.Close(false, Type.Missing, Type.Missing);
                        Marshal.ReleaseComObject(WB);
                    }
                    excelAPP.Quit();
                    Marshal.ReleaseComObject(excelAPP);
                }
                MessageBox.Show(this, "Export success!\n"+fileName, "Writing Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            catch (IOException ex )
            {
                MessageBox.Show(this, "Failed to Export to Excel File\n" + ex.Message, "Inner error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void LvOrders_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Order currSelOrder = LvOrders.SelectedItem as Order;
            if (currSelOrder == null) return;

            var currentOrderItem = Globals.dbContext.OrderItems.Where(o => o.OrderId == currSelOrder.OrderId).ToList();

            if (currentOrderItem == null) return;

            OrderDetailDialogue orderDetailDialogue = new OrderDetailDialogue();
            orderDetailDialogue.Owner = this;
            orderDetailDialogue.LvOrderDetail.ItemsSource = currentOrderItem;
            orderDetailDialogue.LblTotal.Content = currSelOrder.TotalPaied.ToString();
            orderDetailDialogue.ShowDialog();
        }



    }

}
