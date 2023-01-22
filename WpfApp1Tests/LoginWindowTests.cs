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
    public class LoginWindowTests
    {
        
        [TestMethod()]
        public void LoginWindowTest()
        {
            string inputEmail = "john@gmail.com";
            string password = "123";
          

            User user = new User("John Smith", "123", "john@gmail.com");
            LoginWindow lgWindow = new LoginWindow();
            lgWindow.BtnSignin_Click(user, inputEmail, password);
            Assert.Fail();
        }
    }
}