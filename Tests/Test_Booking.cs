using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    [TestFixture]
    public class Test_Booking
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5000";
        private IWebElement element;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Url = baseUrl;
            driver.Navigate();
            //Thread.Sleep(3000);
            //Login("admin", "admin");
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
        private void Login(string username, string password, bool rememberMe = false)
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            // Nhập username và password
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Nếu chọn "Remember Me"
            if (rememberMe)
            {
                driver.FindElement(By.Name("RememberMe")).Click();
            }

            // Nhấn nút "Login"
            driver.FindElement(By.Id("submit-login")).Click();
        }


        [Test]
        public void Test_Booking_Success()
        {
            // Kiểm tra xem đã chuyển hướng đến trang chính chưa
            element= driver.FindElement(By.Id("btn-booking"));
            element.Click();
            Thread.Sleep(3000);
        }
    }
}
