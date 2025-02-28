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
        private string baseUrl = "http://localhost:5145";
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
        public void Test_Booking_Success_Without_Login()
        {
            // Kiểm tra xem đã chuyển hướng đến trang chính chưa
            element = driver.FindElement(By.Id("btn-booking"));
            element.Click();
            IWebElement nameElement = driver.FindElement(By.XPath("//*[@id='name']"));
            nameElement.SendKeys("Đức Anh");
            IWebElement phoneElement = driver.FindElement(By.XPath("//*[@id='phone']"));
            phoneElement.SendKeys("987654321");

            // Checkin - Checkout
            IWebElement checkInElement = driver.FindElement(By.Id("CheckInDate"));
            checkInElement.Clear();  // Xóa giá trị cũ
            checkInElement.SendKeys("2025-03-28T21:15"); // Nhập theo định dạng 24 giờ

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.getElementById('CheckInDate').value = '2025-03-28T21:15';");
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", checkInElement);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", checkInElement);

            IWebElement body = driver.FindElement(By.TagName("body"));
            body.Click(); // Nhấp ra ngoài để mất focus

            Thread.Sleep(3000);

            IWebElement checkOutElement = driver.FindElement(By.Id("CheckOutDate"));
            checkOutElement.Clear();
            checkOutElement.SendKeys("2025-03-30T09:15");
            js.ExecuteScript("document.getElementById('CheckOutDate').value = '2025-03-30T09:15';");
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", checkOutElement);
            js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", checkOutElement);

            body.Click(); // Nhấp ra ngoài để mất focus

            Thread.Sleep(5000);

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("btn-add-3")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement submitButton2 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton2);

            try
            {
                submitButton2.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton2);
            }

            Thread.Sleep(10000);
            Console.WriteLine("Success - Đã chuyển đến trang thanh toán VNPay");

            // Bắt đầu xử lý trên trang VNPay

            // Chọn hình thức thanh toán
            IWebElement paymentMethodElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]")));
            try
            {
                paymentMethodElement.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", paymentMethodElement);
            }

            // Chọn ngân hàng NCB
            IWebElement ncbBankElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/div[1]/div[15]/button[1]/div[1]")));
            try
            {
                ncbBankElement.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ncbBankElement);
            }

            Thread.Sleep(5000); // Delay 5 giây

            // Số thẻ
            IWebElement cardNumberElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("card_number_mask")));
            cardNumberElement.SendKeys("9704198526191432198");
            Thread.Sleep(2000); // Delay 2 giây

            // Tên thẻ
            IWebElement cardHolderElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cardHolder")));
            cardHolderElement.SendKeys("NGUYEN VAN A");
            Thread.Sleep(2000); // Delay 2 giây

            // Ngày phát hành
            IWebElement cardDateElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cardDate")));
            cardDateElement.SendKeys("07/15");
            Thread.Sleep(2000); // Delay 2 giây

            // Nhấn nút Tiếp tục
            IWebElement continueButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
            try
            {
                continueButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", continueButton);
            }
            Thread.Sleep(2000); // Delay 2 giây

            // Nhấn nút Đồng ý
            IWebElement agreeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("btnAgree")));
            try
            {
                agreeButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", agreeButton);
            }
            Thread.Sleep(5000); // Delay 5 giây

            // Nhập OTP
            IWebElement otpElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("otpvalue")));
            otpElement.SendKeys("123456");
            Thread.Sleep(2000); // Delay 2 giây

            // Nhấn nút Thanh toán
            IWebElement confirmButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
            try
            {
                confirmButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", confirmButton);
            }
            Thread.Sleep(5000); // Delay 10 giây để đợi chuyển hướng

            // Kiểm tra xem đã chuyển hướng đến trang PaymentSuccess chưa
            string expectedUrl = "http://localhost:5145/booking/PaymentSuccess";
            string actualUrl = driver.Url;

            if (actualUrl.Contains("PaymentSuccess"))
            {
                Console.WriteLine("Thanh toán thành công! Đã chuyển hướng đến: " + actualUrl);
                Assert.Pass("Test passed: Đã chuyển hướng đến trang PaymentSuccess.");
            }
            else
            {
                Console.WriteLine("Thanh toán thất bại! URL hiện tại: " + actualUrl);
                Assert.Fail("Test failed: Không chuyển hướng đến trang PaymentSuccess.");
            }
        }
    }
}
