using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ClosedXML.Excel;

namespace DemoHotelBooking.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver;
        private string baseUrl = "http://localhost:5145";
        private XLWorkbook workbook;
        private IXLWorksheet worksheet;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            workbook = new XLWorkbook("../../../Data/TestData.xlsx");
            worksheet = workbook.Worksheet("LoginData");
        }

        [Test]
        public void Test_Login()
        {
            for (int i = 2; i <= 29; i++)
            {
                var username = worksheet.Cell(i, 1).GetString();
                var password = worksheet.Cell(i, 2).GetString();
                var expected = worksheet.Cell(i, 3).GetString();

                PerformLogin(username, password);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                bool isRedirected = false;
                try
                {
                    isRedirected = wait.Until(d => d.Url.Contains("/Home"));
                }
                catch (WebDriverTimeoutException)
                {
                    isRedirected = false;
                }

                if (isRedirected)
                {
                    worksheet.Cell(i, 4).Value = "Success";
                    worksheet.Cell(i, 5).Value = (expected == "Success") ? "Pass" : "Fail";
                    driver.Navigate().GoToUrl("${baseUrl}/Account/Login");
                }
                else
                {
                    string errorMessage = GetErrorMessage();
                    worksheet.Cell(i, 4).Value = errorMessage;
                    worksheet.Cell(i, 5).Value = (expected == errorMessage) ? "Pass" : "Fail";
                }
            }
            workbook.Save();
        }

        private void PerformLogin(string username, string password)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            var usernameField = wait.Until(d => d.FindElement(By.Id("username")));
            usernameField.Clear();
            usernameField.SendKeys(username);

            var passwordField = wait.Until(d => d.FindElement(By.Id("password")));
            passwordField.Clear();
            passwordField.SendKeys(password);

            var submitButton = wait.Until(d => d.FindElement(By.Id("submit-login")));
            submitButton.Click();
        }

        private string GetErrorMessage()
        {
            try
            {
                var errorElements = driver.FindElements(By.CssSelector("span.text-danger"));
                foreach (var errorElement in errorElements)
                {
                    if (!string.IsNullOrEmpty(errorElement.Text))
                    {
                        return errorElement.Text;
                    }
                }
                return "Unknown error";
            }
            catch
            {
                return "No error message found";
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
