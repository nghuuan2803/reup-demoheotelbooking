using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ClosedXML.Excel;

namespace DemoHotelBooking.Tests
{
    [TestFixture]
    public class RegisterTests
    {
        private IWebDriver driver;
        private string baseUrl = "http://localhost:5145"; // URL cơ sở của ứng dụng
        private XLWorkbook workbook;
        private IXLWorksheet worksheet;

        [SetUp]
        public void SetUp()
        {
            // Khởi tạo ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Register");

            // Khởi tạo workbook và worksheet từ file Excel
            workbook = new XLWorkbook("../../../Data/TestData_Register.xlsx");
            worksheet = workbook.Worksheet("RegisterData");
        }

        [Test]
        public void Test_Register()
        {
            for (int i = 2; i <= 29; i++)
            {
                var phoneNumber = worksheet.Cell(i, 1).GetString();
                var fullName = worksheet.Cell(i, 2).GetString();
                var email = worksheet.Cell(i, 3).GetString();
                var password = worksheet.Cell(i, 4).GetString();
                var confirmPassword = worksheet.Cell(i, 5).GetString();
                var expected = worksheet.Cell(i, 6).GetString();

                // Kiểm tra email trước khi submit
                if (!IsEmailValid(email))
                {
                    worksheet.Cell(i, 7).Value = "Invalid email format";
                    worksheet.Cell(i, 8).Value = (expected == "Invalid email format") ? "Pass" : "Fail";
                    continue;
                }

                Register(phoneNumber, fullName, email, password, confirmPassword);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                bool isRedirected = false;
                try
                {
                    isRedirected = wait.Until(d => d.Url.Contains("/Room"));
                }
                catch (WebDriverTimeoutException)
                {
                    isRedirected = false;
                }

                if (isRedirected)
                {
                    worksheet.Cell(i, 7).Value = "Success";
                    worksheet.Cell(i, 8).Value = (expected == "Success") ? "Pass" : "Fail";
                    driver.Navigate().GoToUrl($"{baseUrl}/Account/Register");
                }
                else
                {
                    string errorMessage = GetErrorMessage(wait);
                    worksheet.Cell(i, 7).Value = errorMessage;
                    worksheet.Cell(i, 8).Value = (expected == errorMessage) ? "Pass" : "Fail";
                }
            }
            workbook.Save();
        }



        // Hàm hỗ trợ thực hiện đăng ký
        private void Register(string phoneNumber, string fullName, string email, string password, string confirmPassword)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Nhập dữ liệu vào các trường với ID thực tế
            var phoneField = wait.Until(d => d.FindElement(By.Id("phoneNum")));
            phoneField.Clear();
            phoneField.SendKeys(phoneNumber);

            var nameField = wait.Until(d => d.FindElement(By.Id("fullName")));
            nameField.Clear();
            nameField.SendKeys(fullName);

            var emailField = wait.Until(d => d.FindElement(By.Id("email")));
            emailField.Clear();
            emailField.SendKeys(email);

            var passField = wait.Until(d => d.FindElement(By.Id("password")));
            passField.Clear();
            passField.SendKeys(password);

            var confirmPassField = wait.Until(d => d.FindElement(By.Id("confirmPass")));
            confirmPassField.Clear();
            confirmPassField.SendKeys(confirmPassword);

            var submitButton = wait.Until(d => d.FindElement(By.Id("submit-Register")));
            submitButton.Click();
        }

        private bool IsEmailValid(string email)
        {
            var emailField = driver.FindElement(By.Id("email"));
            emailField.Clear();
            emailField.SendKeys(email);

            // Dùng JavaScript để kiểm tra validity của input email
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            bool isValid = (bool)js.ExecuteScript("return arguments[0].checkValidity();", emailField);

            return isValid;
        }

        // Hàm lấy thông báo lỗi từ các span.text-danger
        private string GetErrorMessage(WebDriverWait wait)
        {
            try
            {
                var errorElements = driver.FindElements(By.CssSelector("span.text-danger.field-validation-error"));
                foreach (var errorElement in errorElements)
                {
                    if (!string.IsNullOrEmpty(errorElement.Text))
                    {
                        return errorElement.Text; // Trả về lỗi đầu tiên tìm thấy
                    }
                }
                return "Unknown error"; // Nếu không tìm thấy lỗi cụ thể
            }
            catch
            {
                return "No error message found";
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Đóng trình duyệt
            driver.Quit();
        }
    }
}