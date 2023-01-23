using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Tests
{
    [TestClass()]
    public class CheckoutWindowTests
    {
        [TestMethod()]
        public void ExportTest()
        {
            WpfApp1.CheckoutWindow ctWindow = new WpfApp1.CheckoutWindow();
            List<string> data = new List<string>();

            data.Add("Custemor Id: " + "1");

            data.Add("Issue date: " + DateTime.Now.ToShortDateString());
            data.Add("---------------------------------------------------------------------------------");
            data.Add(String.Format("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}\n\n", "Product Id", "Name", "Unitptice", "Quantity", "Price"));
            List<CheckoutItemList> allItems = new List<CheckoutItemList>();
            allItems.Add(new CheckoutItemList(1, "chair", 12.5, 4));
            allItems.Add(new CheckoutItemList(1, "bed", 12.5, 2));
            allItems.Add(new CheckoutItemList(1, "sofa", 12.5, 1));
            if (allItems == null) return;
            foreach (var item in allItems)
            {
                data.Add(String.Format("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}\n\n", item.ProductId.ToString(), item.ProductName, item.UnitPrice.ToString(), item.Quantity.ToString(), item.Price.ToString()));
            }
            data.Add("---------------------------------------------------------------------------------");
            data.Add("Total Price: $" + 87.5);
            string text = "";
            foreach (string d in data)
            {
                text += d + "\n";
            }
            bool ifExport = ctWindow.Export(text);
            Assert.AreEqual(true, ifExport, "Shoule succeeds.");
        }
    }
}