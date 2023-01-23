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
        public void VerifyIdentityTest()
        {
            Assert.Fail(); string email = "john@gmail.com";
            string pass = "123";

            WpfApp1.LoginWindow lgWindow = new WpfApp1.LoginWindow();
            bool isFound = lgWindow.VerifyIdentity(email, pass);

            Assert.IsTrue(isFound);

           
        }
    }
}