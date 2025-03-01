

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using System;
using System.IO;
using System.Threading;

namespace HoaiNghiaTestSelenium
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5000"; 

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private void Login(string username, string password, bool rememberMe = false)
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            if (rememberMe)
            {
                driver.FindElement(By.Name("RememberMe")).Click();
            }

            driver.FindElement(By.Id("submit")).Click();
        }

        [Test]
        public void TestCreateRoom()
        {
            Login("admin", "admin"); 

            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            driver.FindElement(By.Id("name")).SendKeys("STU005");
            driver.FindElement(By.Id("type")).SendKeys("Deluxe");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("dap")).SendKeys("1200000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            if (File.Exists(imagePath))
            {
                driver.FindElement(By.Id("imageUpload")).SendKeys(imagePath);
            }
            else
            {
                Assert.Fail("Ảnh không tồn tại, kiểm tra lại đường dẫn!");
            }

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            Assert.IsTrue(driver.Url.Contains("http://localhost:5000/Admin"));

        }
        [Test]
        public void TestImgNotNull()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
            driver.FindElement(By.Id("name")).SendKeys("STU009");
            driver.FindElement(By.Id("type")).SendKeys("Deluxe");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("dap")).SendKeys("1200000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");
            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }
            var errorMessage = driver.FindElement(By.XPath("//h4[@class='text-danger']"));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi chưa upload ảnh.");
            Assert.AreEqual("Thông tin phòng không hợp lệ", errorMessage.Text, "Thông báo lỗi không đúng.");

          
        }
        [Test]
        public void TestImgFormat()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
            driver.FindElement(By.Id("name")).SendKeys("STU009");
            driver.FindElement(By.Id("type")).SendKeys("Deluxe");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("dap")).SendKeys("1200000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");
            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement fileInput = driver.FindElement(By.Id("imageUpload")); 
            string acceptTypes = fileInput.GetAttribute("accept");
            var errorMessage = driver.FindElement(By.XPath("//h4[@class='text-danger']"));

            Assert.AreEqual("Thông tin phòng không hợp lệ", errorMessage.Text, "Thông báo lỗi không đúng.");




          
        }
        [Test]
        public void TestRoomTypeRequired()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";

            driver.FindElement(By.Id("name")).SendKeys("STU009");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("dap")).SendKeys("1200000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload")); 
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='text-danger field-validation-error' and @data-valmsg-for='Type']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi loại phòng để trống.");
            Assert.AreEqual("The Loại phòng field is required.", errorMessage.Text, "Thông báo lỗi không đúng.");
        }


        public void TestInvalidRoomType()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";

            driver.FindElement(By.Id("name")).SendKeys("STU010");
            driver.FindElement(By.Id("type")).SendKeys("XYZ"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("5");
            driver.FindElement(By.Id("price")).SendKeys("2000000");
            driver.FindElement(By.Id("dap")).SendKeys("2200000");
            driver.FindElement(By.Id("map")).SendKeys("2100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng cao cấp, nội thất hiện đại");
            driver.FindElement(By.Id("description")).SendKeys("Không gian rộng rãi, view biển");

            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload")); 
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h4[@class='text-danger' and text()='Thông tin phòng không hợp lệ']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi nhập loại phòng không hợp lệ.");
            Assert.AreEqual("Thông tin phòng không hợp lệ", errorMessage.Text, "Thông báo lỗi không đúng.");
        }
        [Test]
        public void TestRoomCodeRequired()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";

            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1500000");
            driver.FindElement(By.Id("dap")).SendKeys("1600000");
            driver.FindElement(By.Id("map")).SendKeys("1550000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng tiện nghi, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Nội thất hiện đại, không gian thoải mái");

            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload")); 
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='text-danger field-validation-error' and @data-valmsg-for='Name']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi mã phòng để trống.");
            Assert.AreEqual("The Mã phòng field is required.", errorMessage.Text, "Thông báo lỗi không đúng.");
        }
        [Test]
        public void TestRoomCodeLength()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            string invalidRoomCode = "STD12345678"; 

            IWebElement nameField = driver.FindElement(By.Id("name"));
            nameField.Clear();
            nameField.SendKeys(invalidRoomCode);

            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1500000");
            driver.FindElement(By.Id("dap")).SendKeys("1600000");
            driver.FindElement(By.Id("map")).SendKeys("1550000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng tiện nghi, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Nội thất hiện đại, không gian thoải mái");

            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='text-danger field-validation-error' and @data-valmsg-for='Name']")));

            Assert.AreEqual("The Mã phòng field is required.", errorMessage.Text, "Thông báo lỗi không đúng.");
        }

        [Test]
        public void TestRoomPriceMustBeNumber()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";

            driver.FindElement(By.Id("name")).SendKeys("STD001");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("0sdsdsdsd");
            driver.FindElement(By.Id("dap")).SendKeys("1200000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='text-danger field-validation-error' and @data-valmsg-for='Price']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông tin phòng không hợp lệ.");
            Assert.AreEqual("The value '00sdsdsdsd' is not valid for Giá phòng.", errorMessage.Text, "Thông báo lỗi không đúng.");
        }
        [Test]
        public void TestRequiredNumberOfPeople()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            driver.FindElement(By.Id("name")).SendKeys("STD101");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");

            IWebElement dapInput = driver.FindElement(By.Id("dap"));
            dapInput.Clear();  

            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi số người quy định để trống.");
            Assert.AreEqual("The value '' is invalid.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");
        }

        [Test]
     
        public void TestInvalidRoomNameFormat()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("STD@101");

            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='name']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi tên phòng chứa ký tự đặc biệt.");
            Assert.AreEqual("Tên phòng không được chứa ký tự đặc biệt.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("XYZ101"); 

            submitButton.Click();

            errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='name']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi tên phòng không bắt đầu bằng tiền tố hợp lệ.");
            Assert.AreEqual("Tên phòng phải bắt đầu bằng tiền tố hợp lệ của loại phòng.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("STD"); 

            submitButton.Click();

            errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='name']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi tên phòng không có hậu tố mã số.");
            Assert.AreEqual("Tên phòng phải có hậu tố là mã số phòng.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");
        }

        [Test]
        public void TestUniqueRoomCode()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("STD101"); 

            driver.FindElement(By.Id("type")).SendKeys("STD");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='name']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi nhập mã phòng đã tồn tại.");
            Assert.AreEqual("Mã phòng đã tồn tại, vui lòng chọn mã khác.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");
        }
        [Test]
        public void TestRequiredRoomPrice()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD102");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("price")).Clear(); 
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='Price']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi không nhập giá phòng.");
            Assert.AreEqual("The value '' is invalid.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng.");
        }
        [Test]
        public void TestRoomPriceValidation()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD103");
            driver.FindElement(By.Id("type")).SendKeys("STD");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            driver.FindElement(By.Id("price")).Clear();
            driver.FindElement(By.Id("price")).SendKeys("0");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMessageZero = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='Price']")));
            Assert.IsTrue(errorMessageZero.Displayed, "Thông báo lỗi không xuất hiện khi giá phòng <= 0.");
            Assert.AreEqual("Giá phòng phải lớn hơn 0.", errorMessageZero.Text.Trim(), "Thông báo lỗi không đúng khi giá phòng <= 0.");

            driver.FindElement(By.Id("price")).Clear();
            driver.FindElement(By.Id("price")).SendKeys("100000001");

            submitButton.Click();

            IWebElement errorMessageMax = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='Price']")));
            Assert.IsTrue(errorMessageMax.Displayed, "Thông báo lỗi không xuất hiện khi giá phòng vượt quá 100,000,000.");
            Assert.AreEqual("Giá phòng tối đa là 100,000,000 một ngày.", errorMessageMax.Text.Trim(), "Thông báo lỗi không đúng khi giá phòng vượt quá giới hạn.");
        }

        [Test]
        public void TestNumberOfPeopleRange()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD105");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("0"); 
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("map")).SendKeys("1100000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            IWebElement errorMinMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")));
            Assert.IsTrue(errorMinMessage.Displayed, "Thông báo lỗi không xuất hiện khi số người quy định nhỏ hơn 1.");
            Assert.AreEqual("Số người quy định phải từ 1 đến 5.", errorMinMessage.Text.Trim(), "Thông báo lỗi không đúng khi số người < 1.");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("6"); 

            submitButton.Click();

            IWebElement errorMaxMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")));
            Assert.IsTrue(errorMaxMessage.Displayed, "Thông báo lỗi không xuất hiện khi số người quy định lớn hơn 5.");
            Assert.AreEqual("Số người quy định phải từ 1 đến 5.", errorMaxMessage.Text.Trim(), "Thông báo lỗi không đúng khi số người > 5.");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("3");

            submitButton.Click();

            bool isErrorMessagePresent = driver.FindElements(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")).Count > 0;
            Assert.IsFalse(isErrorMessagePresent, "Không nên có thông báo lỗi khi số người hợp lệ.");
        }
        [Test]
        public void TestRequiredDepositAmount()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD107");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("3"); 
            driver.FindElement(By.Id("price")).SendKeys("1000000");
            driver.FindElement(By.Id("map")).Clear(); 
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng sang trọng, view đẹp");
            driver.FindElement(By.Id("description")).SendKeys("Phòng rộng rãi, tiện nghi hiện đại");

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='MAP']")));

            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi giá đặt cọc bị bỏ trống.");
            Assert.AreEqual("The value '' is invalid.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng khi giá đặt cọc bị bỏ trống.");


           
        }
        [Test]
        public void TestMinMaxNumberOfPeople()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD108");
            driver.FindElement(By.Id("type")).SendKeys("STD");
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("price")).SendKeys("1500000");
            driver.FindElement(By.Id("map")).SendKeys("500000");
            driver.FindElement(By.Id("introduce")).SendKeys("Phòng rộng rãi, thoải mái");
            driver.FindElement(By.Id("description")).SendKeys("Đầy đủ tiện nghi, view đẹp");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("1");
            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi số người nhỏ hơn 2.");
            Assert.AreEqual("Số người phải từ 2 đến 10.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng khi số người nhỏ hơn 2.");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("11"); 
            submitButton.Click();

            errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không xuất hiện khi số người lớn hơn 10.");
            Assert.AreEqual("Số người phải từ 2 đến 10.", errorMessage.Text.Trim(), "Thông báo lỗi không đúng khi số người lớn hơn 10.");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("2");
            submitButton.Click();

            bool isErrorMessagePresent = driver.FindElements(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")).Count > 0;
            Assert.IsFalse(isErrorMessagePresent, "Không nên có thông báo lỗi khi số người là 2 (hợp lệ).");

            driver.FindElement(By.Id("dap")).Clear();
            driver.FindElement(By.Id("dap")).SendKeys("10");
            submitButton.Click();

            isErrorMessagePresent = driver.FindElements(By.XPath("//span[contains(@class, 'text-danger') and @data-valmsg-for='DAP']")).Count > 0;
            Assert.IsFalse(isErrorMessagePresent, "Không nên có thông báo lỗi khi số người là 10 (hợp lệ).");
        }

        [Test]
        public void TestIntroduceFieldIsOptional()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD109");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("price")).SendKeys("1500000");
            driver.FindElement(By.Id("map")).SendKeys("500000");

            driver.FindElement(By.Id("introduce")).Clear();

            driver.FindElement(By.Id("description")).SendKeys("Đầy đủ tiện nghi, view đẹp");
            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }     
        }
        [Test]
        public void TestDescriptionFieldIsOptional()
        {
            Login("admin", "admin");
            driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

            string imagePath = "G:\\Downloads - 2\\anhnhocuc.png";
            IWebElement uploadElement = driver.FindElement(By.Id("imageUpload"));
            uploadElement.SendKeys(imagePath);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("name")).SendKeys("STD109");
            driver.FindElement(By.Id("type")).SendKeys("STD"); 
            driver.FindElement(By.Id("floornumber")).SendKeys("3");
            driver.FindElement(By.Id("dap")).SendKeys("2");
            driver.FindElement(By.Id("price")).SendKeys("1500000");
            driver.FindElement(By.Id("map")).SendKeys("500000");

            driver.FindElement(By.Id("description")).Clear(); 

            driver.FindElement(By.Id("introduce")).SendKeys("Đầy đủ tiện nghi, view đẹp");
            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
