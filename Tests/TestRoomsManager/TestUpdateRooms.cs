using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest;


[TestFixture]
public class TestUpdateRooms
{


    private IWebDriver driver;
    private string baseUrl = "http://localhost:5145";
    private XLWorkbook workbook;
    private IXLWorksheet worksheet;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl($"{baseUrl}/Admin/roommanager/create");

        workbook = new XLWorkbook("../../../Data/TestAddRooms.xlsx");
        worksheet = workbook.Worksheet("UpdateRooms");

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

        driver.FindElement(By.Id("submit-login")).Click();
    }
    [Test]
    public void UpdateRoomsSuccess()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 2; i <= 2; i++) 
        {
            var name = worksheet.Cell(i, 3).GetString();  
            var type = worksheet.Cell(i, 4).GetString();  
            var floorNumber = worksheet.Cell(i, 5).GetString();  
            var price = worksheet.Cell(i, 6).GetString();  
            var dap = worksheet.Cell(i, 7).GetString();  
            var map = worksheet.Cell(i, 8).GetString();  
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until( SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }

    [Test]
    public void TestUpdateRoomsCode()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 3; i <= 3; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 4; i <= 4; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 5; i <= 5; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 6; i <= 6; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 7; i <= 7; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 8; i <= 8; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 9; i <= 9; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode8()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 10; i <= 10; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode9()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 11; i <= 11; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsCode10()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 12; i <= 12; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 13; i <= 13; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 14; i <= 14; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 15; i <= 15; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 16; i <= 16; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Cập nhập phòng thành công";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 17; i <= 17; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 18; i <= 18; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ?  "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateRoomsType7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 19; i <= 19; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 20; i <= 20; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 21; i <= 21; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 22; i <= 22; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 23; i <= 23; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 23; i <= 23; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 24; i <= 24; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 25; i <= 25; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice8()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 26; i <= 26; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice9()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 27; i <= 27; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice10()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 28; i <= 28; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePrice11()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 29; i <= 29; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 30; i <= 30; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 31; i <= 31; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 32; i <= 32; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 33; i <= 33; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 34; i <= 34; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 35; i <= 35; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 36; i <= 36; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber8()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 37; i <= 37; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdateFloornumber10()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 38; i <= 38; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 39; i <= 39; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 40; i <= 40; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 41; i <= 41; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 42; i <= 42; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 43; i <= 43; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 44; i <= 44; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 45; i <= 45; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople8()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 46; i <= 46; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TestUpdatePeople9()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 47; i <= 47; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 48; i <= 48; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople2()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 49; i <= 49; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople3()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 50; i <= 50; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople4()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 51; i <= 51; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople5()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 52; i <= 52; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople6()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 53; i <= 53; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople7()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 54; i <= 54; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople8()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 55; i <= 55; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople9()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 56; i <= 56; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [Test]
    public void TextUpdateMaxPeople10()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl("http://localhost:5145/Admin");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        IWebElement updateLink = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("update")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateLink);
        Thread.Sleep(4000);
        for (int i = 57; i <= 57; i++)
        {
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var expectedMsg = worksheet.Cell(i, 11).GetString();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys(name);
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys(type);
            driver.FindElement(By.Id("FloorNumber")).Clear();
            driver.FindElement(By.Id("FloorNumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("Price")).Clear();
            driver.FindElement(By.Id("Price")).SendKeys(price);
            driver.FindElement(By.Id("DAP")).Clear();
            driver.FindElement(By.Id("DAP")).SendKeys(dap);
            driver.FindElement(By.Id("MAP")).Clear();
            driver.FindElement(By.Id("MAP")).SendKeys(map);
            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
            try
            {
                saveButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", saveButton);
            }
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
            Thread.Sleep(2000);
        }
        workbook.Save();
    }
    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000);
        driver.Quit();
    }
}

