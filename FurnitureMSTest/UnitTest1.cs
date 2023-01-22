using WpfApp1;

namespace FurnitureTestQZ1
{
    [TestClass]
    public class UnitTest1
    {

        // Customer Test
        [TestMethod]
        public void AddCustomer()
        {
            //Customer c1 = new Customer();
            //c1.FullName = "John Smith";
            //c1.Email = "J1@gmail.com";
            //c1.Phone = "12223334444";
            //c1.FullAddress = null;
            //Assert.AreEqual(true, c1.AddCustomer());

            //Customer c2 = new Customer();
            //c2.FullName = "John Smith";
            //c2.Email = "J1gmail.com";
            //c2.Phone = "2223334444";
            //c2.FullAddress = null;
            //Assert.AreEqual(true, c2.AddCustomer());

            //Customer c3 = new Customer();
            //c3.FullName = "John Smith";
            //c3.Email = "J1@gmail.com";
            //c3.Phone = "2223334444";
            //c3.FullAddress = null;
            //Assert.AreEqual(true, c3.AddCustomer());

            Customer c4 = new Customer();
            c4.FullName = "John Smith";
            c4.Email = "J1@gmail.com";
            c4.Phone = "12223334444";
            c4.FullAddress = "AA,AA,AA,AA,AA";
            Assert.AreEqual(true, c4.AddCustomer());

        
        }
    }
}