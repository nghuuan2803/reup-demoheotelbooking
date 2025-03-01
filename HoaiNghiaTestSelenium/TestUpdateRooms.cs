using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace HoaiNghiaTestSelenium
{
    public class TestUpdateRooms
    {
        [TestFixture]
        public class TestsUpdate
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
            
            public void UpdateRoom_Successfully()
            {
                Login("admin", "admin");
                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");


               
                driver.FindElement(By.Id("Type")).Clear();
                driver.FindElement(By.Id("Type")).SendKeys("DEL"); 

                driver.FindElement(By.Id("Name")).Clear();
                driver.FindElement(By.Id("Name")).SendKeys("DELUXE102");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");

                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

                try
                {
                    submitButton.SendKeys(Keys.Enter); 
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
                }
                Thread.Sleep(5000);

               

            }
            [Test]
            public void UpdateRoom_TypeIsRequired()
            {
                Login("admin", "admin");

                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");
                driver.FindElement(By.Id("Type")).SendKeys("DEL");
                driver.FindElement(By.Id("Type")).Clear();


                driver.FindElement(By.Id("Name")).Clear();
                driver.FindElement(By.Id("Name")).SendKeys("DELUXE102");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement typeInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Type")));
                typeInput.Clear();

                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                submitButton.SendKeys(Keys.Enter);

                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Type']")));

                Assert.AreEqual("The value '' is invalid.", errorMessage.Text);

               
            }
            [Test]
            public void UpdateRoom_InvalidRoomType_ShouldFail()
            {
                Login("admin", "admin");

                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");
               
                driver.FindElement(By.Id("Type")).SendKeys("VIP");

                driver.FindElement(By.Id("Name")).Clear();
                driver.FindElement(By.Id("Name")).SendKeys("DELUXE102");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement typeInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Type")));

                typeInput.Clear();
                typeInput.SendKeys("VIP");

                
                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                submitButton.SendKeys(Keys.Enter);

                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Type']")));

                Assert.AreEqual("The value is invalid.", errorMessage.Text);

                
            }
            [Test]
            public void UpdateRoom_EmptyRoomId_ShouldFail()
            {
                Login("admin", "admin");

                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");
                driver.FindElement(By.Id("Type")).Clear();
                driver.FindElement(By.Id("Type")).SendKeys("DEL");

               
                driver.FindElement(By.Id("Name")).SendKeys("DELUXE102");
                driver.FindElement(By.Id("Name")).Clear();

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement roomIdInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Id")));

                roomIdInput.Clear();

                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                submitButton.SendKeys(Keys.Enter);

                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Id']")));

                Assert.AreEqual("The value '' is invalid.", errorMessage.Text);

                
            }

            [Test]
            public void UpdateRoom_RoomCodeTooShort_ShouldFail()
            {
                Login("admin", "admin");

                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                driver.FindElement(By.Id("Type")).Clear();
                driver.FindElement(By.Id("Type")).SendKeys("DEL"); 

                IWebElement nameInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Name")));
                nameInput.Clear();
                nameInput.SendKeys("STD019999999999999999999999999999999");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, wifi miễn phí, TV thông minh.");

                
                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

                try
                {
                    submitButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
                }

                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Name']")));

                Assert.AreEqual("Mã phòng phải có ít nhất 10 ký tự.", errorMessage.Text.Trim());
            }

            [Test]
            public void UpdateRoom_InvalidRoomIdFormat_ShouldFail()
            {
                Login("admin", "admin");

                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement typeInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Type")));
                typeInput.Clear();
                typeInput.SendKeys("DLX");

                IWebElement roomIdInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Name")));
                roomIdInput.Clear();
                roomIdInput.SendKeys("DLX-102!"); 

                driver.FindElement(By.Id("Name")).Clear();
                driver.FindElement(By.Id("Name")).SendKeys("DELUXE102@@@##");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");

               
                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

                try
                {
                    submitButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
                }

                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Id']")));

                Assert.AreEqual("Mã phòng phải bắt đầu bằng loại phòng hợp lệ và chỉ chứa chữ cái và số.", errorMessage.Text);

              
            }

            [Test]
            public void UpdateRoom_DuplicateRoomId_ShouldFail()
            {
                // Đăng nhập với tài khoản admin
                Login("admin", "admin");

                // Điều hướng đến trang cập nhật phòng (id=1 là ví dụ)
                driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Update/1");


                IWebElement typeInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Type")));
                typeInput.Clear();
                typeInput.SendKeys("DLX");

                

                driver.FindElement(By.Id("Name")).Clear();
                driver.FindElement(By.Id("Name")).SendKeys("STD104");

                driver.FindElement(By.Id("FloorNumber")).Clear();
                driver.FindElement(By.Id("FloorNumber")).SendKeys("2");

                driver.FindElement(By.Id("Price")).Clear();
                driver.FindElement(By.Id("Price")).SendKeys("2000000");

                driver.FindElement(By.Id("DAP")).Clear();
                driver.FindElement(By.Id("DAP")).SendKeys("2");

                driver.FindElement(By.Id("MAP")).Clear();
                driver.FindElement(By.Id("MAP")).SendKeys("4");

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys("Phòng sang trọng, đầy đủ tiện nghi, view đẹp.");

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys("Phòng Deluxe có diện tích lớn, bao gồm giường king size, wifi miễn phí, TV thông minh.");



                IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

                try
                {
                    submitButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
                }
                IWebElement errorMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-valmsg-for='Id']")));

                Assert.AreEqual("Mã phòng đã tồn tại trong hệ thống.", errorMessage.Text);

                
            }

            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }


        }

    }
}
