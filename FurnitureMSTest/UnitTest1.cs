using WpfApp1;

namespace FurnitureMSTest
{
    [TestClass]
    public class UnitTest1
    {

        // Customer Test
        //[TestMethod]
        //public void AddCustomer()
        //{
            /* ---------  1. Right Input w/ No Address -------- */
            //Customer c1 = new Customer();
            //c1.FullName = "John Smith";
            //c1.Email = "J1@gmail.com";
            //c1.Phone = "12223334444";
            //c1.FullAddress = null;
            //Assert.AreEqual(true, c1.AddCustomer());

            /* --------- 2. Wrong Input (Wrong Email) --------- */
            //Customer c2 = new Customer();
            //c2.FullName = "John Smith";
            //c2.Email = "J1gmail.com";
            //c2.Phone = "2223334444";
            //c2.FullAddress = null;
            //Assert.AreEqual(true, c2.AddCustomer());

            /* --------- 3. Wrong Input (Wrong Phone) --------- */
            //Customer c3 = new Customer();
            //c3.FullName = "John Smith";
            //c3.Email = "J1@gmail.com";
            //c3.Phone = "2223334444";
            //c3.FullAddress = null;
            //Assert.AreEqual(true, c3.AddCustomer());

            /* ------- 4. Right Input w/ right FullAddress(5 fields) ------- */
            //Customer c4 = new Customer();
            //c4.FullName = "John Smith";
            //c4.Email = "J1@gmail.com";
            //c4.Phone = "12223334444";
            //c4.FullAddress = "AA,AA,AA,AA,AA";
            //Assert.AreEqual(true, c4.AddCustomer());

            /* ----- 5. Wrong Input w/ wrong FullAddress(NOT 5 fields) ----- */
            //Customer c5 = new Customer();
            //c5.FullName = "John Smith";
            //c5.Email = "J1@gmail.com";
            //c5.Phone = "12223334444";
            //c5.FullAddress = "AA,AA,AA,AA";
            //Assert.AreEqual(true, c5.AddCustomer());


            //}

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