using WpfApp1;

namespace FurnitureMSTest
{
    [TestClass]
    public class ProductTest
    {
               
        [TestMethod]
        public void AddProduct()
        {
            /* ----- 1. Wrong Input w/ wrong Name(NOT be 2-100 characters long) ----- */
            Product p1 = new Product();
            p1.Name = "S";
            p1.Quantity = 1;
            p1.Price = 1205;
            Assert.AreEqual(true, p1.AddProduct());

            /* ----- 2. Wrong Input w/ wrong Price(NOT double) ----- */
            //Product p2 = new Product();
            //p2.Name = "Sofa";
            //p2.Quantity = 1;
            //p2.Price = double.Parse("A2");
            //Assert.AreEqual(true, p2.AddProduct());
            /* ----- 3. Wrong Input w/ wrong Quantity(NOT integer) ----- */
            //Product p3 = new Product();
            //p3.Name = "Sofa";
            //p3.Quantity = int.Parse("A1");
            //p3.Price = 1205;
            //Assert.AreEqual(true, p3.AddProduct());
        }
    }
}