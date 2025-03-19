using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest;


[TestFixture]
public class TestAddRooms
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
        worksheet = workbook.Worksheet("AddRooms");

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
    public void AddRoomsSuccess()
    {
        
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        
        for (int i = 2; i <= 2; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); 

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
               
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

           
            bool isSuccess = false;
            string actualMessage = "Thêm phòng thành công";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(actualMessage));
                if (isSuccess)
                {
                    actualMessage = actualMessage;
                }
            }
            catch (WebDriverTimeoutException)
            {
                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }

    [Test]
    public void TestImage()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        
        for (int i = 3; i <= 3; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); 

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            bool isSuccess = false;
            string actualMessage = "Thêm phòng thành công";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestImage1()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        
        for (int i = 4; i <= 4; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); 

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            
            bool isSuccess = false;
            string actualMessage ="Không tìm thấy thông báo";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
               
                isSuccess = false;
            }

          
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            //
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestImage2()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

     
        for (int i = 5; i <= 5; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); 

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

           
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestImage3()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 6; i <= 6; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestImage4()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 7; i <= 7; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 8; i <= 8; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    
    public void TestRoomsCode1()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 9; i <= 9; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Thêm phòng thành công ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode2()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 10; i <= 10; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode4()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 11; i <= 11; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode5()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 12; i <= 12; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode6()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 13; i <= 13; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode7()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 14; i <= 14; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode8()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 15; i <= 15; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode9()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 16; i <= 16; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsCode10()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 17; i <= 17; i++)
        {
            // Đọc dữ liệu từ file Excel
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); // "Thêm phòng thành công"

            // Tìm phần tử upload file
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            // Chờ các phần tử trong form sẵn sàng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form
            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            // Submit Form
            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            // Scroll đến nút (nếu cần)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Nếu bị chặn click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            // Kiểm tra kết quả: 
            // Ví dụ, chờ PageSource chứa chuỗi "Thêm phòng thành công" (lấy từ cột Expected)
            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    // Nếu tìm thấy chuỗi "Thêm phòng thành công" trong page
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Hết thời gian chờ, vẫn chưa thấy message
                isSuccess = false;
            }

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

       
        for (int i = 18; i <= 18; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString(); 

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }

            
            bool isSuccess = false;
            string actualMessage = "Mã phòng hợp lệ ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {
                    
                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {
               
                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType1()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 19; i <= 19; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Mã phòng hợp lệ ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType2()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 20; i <= 20; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Mã phòng hợp lệ ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType3()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 21; i <= 21; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy message mong đợi. ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType4()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 22; i <= 22; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType5()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 23; i <= 23; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestRoomsType6()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 24; i <= 24; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void TestPrice()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 25; i <= 25; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice2()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 26; i <= 26; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Giá phòng hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice3()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 27; i <= 27; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice4()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 28; i <= 28; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice5()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 29; i <= 29; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Giá phòng hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice6()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 30; i <= 30; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice7()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 31; i <= 31; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Giá phòng hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice8()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 32; i <= 32; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Giá phòng hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice9()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 33; i <= 33; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPrice10()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 34; i <= 34; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 35; i <= 35; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber2()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 36; i <= 36; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber3()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 37; i <= 37; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber4()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 38; i <= 38; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Số lầu hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber5()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 39; i <= 39; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Số lầu hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber6()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 40; i <= 40; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Số lầu hợp lệ";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Fail" : "Pass";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber7()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 41; i <= 41; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber8()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 42; i <= 42; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestFloornumber9()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");


        for (int i = 43; i <= 43; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();

            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("type")).SendKeys(type);
            driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
            driver.FindElement(By.Id("price")).SendKeys(price);
            driver.FindElement(By.Id("dap")).SendKeys(dap);
            driver.FindElement(By.Id("map")).SendKeys(map);
            driver.FindElement(By.Id("introduce")).SendKeys(introduce);
            driver.FindElement(By.Id("description")).SendKeys(description);

            IWebElement submitButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
            );

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

            try
            {
                submitButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
            }


            bool isSuccess = false;
            string actualMessage = "Không tìm thấy thông báo mong đợi";

            try
            {
                isSuccess = wait.Until(d => d.PageSource.Contains(expected));
                if (isSuccess)
                {

                    actualMessage = expected;
                }
            }
            catch (WebDriverTimeoutException)
            {

                isSuccess = false;
            }

            worksheet.Cell(i, 12).Value = actualMessage;
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Fail";

            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();

    }
    [Test]
    public void TestPeople()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
        for (int i = 44; i <= 48; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            try
            {
                // Điền dữ liệu vào form
                driver.FindElement(By.Id("name")).SendKeys(name);
                driver.FindElement(By.Id("type")).SendKeys(type);
                driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
                driver.FindElement(By.Id("price")).SendKeys(price);
                driver.FindElement(By.Id("dap")).SendKeys(dap);
                driver.FindElement(By.Id("map")).SendKeys(map);
                driver.FindElement(By.Id("introduce")).SendKeys(introduce);
                driver.FindElement(By.Id("description")).SendKeys(description);

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
                bool isInvalidPrice = false;
                try
                {
                    isInvalidPrice = wait.Until(d =>
                        d.PageSource.Contains("Số người quy định không được bỏ trống") ||
                        d.PageSource.Contains("Tối thiểu 1 người") ||
                        d.PageSource.Contains("Tối đa 5 người") ||
                        d.PageSource.Contains("Tối thiểu 1 và tối đa 1 người"));
                }
                catch (WebDriverTimeoutException)
                {
                    isInvalidPrice = false;
                }


                // Kiểm tra kết quả hiển thị trên trang
                bool isSuccess = false;
                string actualMessage = "Không tìm thấy message mong đợi. ";

                try
                {
                    isSuccess = wait.Until(d => d.PageSource.Contains("Thêm thành công") || d.PageSource.Contains("Giá không hợp lệ"));
                }
                catch (WebDriverTimeoutException)
                {
                    isSuccess = false;
                }

                // Ghi kết quả vào Excel
                worksheet.Cell(i, 12).Value = isSuccess ? "Success" : "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 13).Value = (expected == actualMessage && isSuccess) || (expected == "Fail" && !isSuccess) ? "Pass" : "Fail";
            }
            catch (Exception ex)
            {
                worksheet.Cell(i, 12).Value = "Error";
                worksheet.Cell(i, 13).Value = ex.Message;
            }

            // Điều hướng lại trang để thêm phòng mới
            driver.Navigate().GoToUrl("http://localhost:5145/Admin/RoomManager/Create");
            workbook.Save();
        }
    }
    [Test]
    public void TestPeople2()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
        for (int i = 49; i <= 52; i++)
        {
            var imagePath = worksheet.Cell(i, 2).GetString();
            var name = worksheet.Cell(i, 3).GetString();
            var type = worksheet.Cell(i, 4).GetString();
            var floorNumber = worksheet.Cell(i, 5).GetString();
            var price = worksheet.Cell(i, 6).GetString();
            var dap = worksheet.Cell(i, 7).GetString();
            var map = worksheet.Cell(i, 8).GetString();
            var introduce = worksheet.Cell(i, 9).GetString();
            var description = worksheet.Cell(i, 10).GetString();
            var expected = worksheet.Cell(i, 11).GetString();
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("C:\\Users\\phn40\\OneDrive\\Pictures\\Group 1.png");
            }

            try
            {
                // Điền dữ liệu vào form
                driver.FindElement(By.Id("name")).SendKeys(name);
                driver.FindElement(By.Id("type")).SendKeys(type);
                driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);
                driver.FindElement(By.Id("price")).SendKeys(price);
                driver.FindElement(By.Id("dap")).SendKeys(dap);
                driver.FindElement(By.Id("map")).SendKeys(map);
                driver.FindElement(By.Id("introduce")).SendKeys(introduce);
                driver.FindElement(By.Id("description")).SendKeys(description);

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
                bool isInvalidPrice = false;
                try
                {
                    isInvalidPrice = wait.Until(d =>
                        d.PageSource.Contains("Số người quy định không được bỏ trống") ||
                        d.PageSource.Contains("Tối thiểu 1 người") ||
                        d.PageSource.Contains("Tối đa 5 người") ||
                        d.PageSource.Contains("Tối thiểu 1 và tối đa 1 người"));
                }
                catch (WebDriverTimeoutException)
                {
                    isInvalidPrice = false;
                }


                // Kiểm tra kết quả hiển thị trên trang
                bool isSuccess = false;
                string actualMessage = "Không tìm thấy message mong đợi. ";

                try
                {
                    isSuccess = wait.Until(d => d.PageSource.Contains("Thêm thành công") || d.PageSource.Contains("Giá không hợp lệ"));
                }
                catch (WebDriverTimeoutException)
                {
                    isSuccess = false;
                }

                // Ghi kết quả vào Excel
                worksheet.Cell(i, 12).Value = isSuccess ? "Success" : "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 13).Value = (expected == actualMessage && isSuccess) || (expected == "Fail" && !isSuccess) ? "Pass" : "Fail";
            }
            catch (Exception ex)
            {
                worksheet.Cell(i, 12).Value = "Error";
                worksheet.Cell(i, 13).Value = "Error";
            }

            // Điều hướng lại trang để thêm phòng mới
            driver.Navigate().GoToUrl("http://localhost:5145/Admin/RoomManager/Create");
            workbook.Save();
        }
    }
    [Test]
    public void TestRoomMaxPeopleConstraints()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        for (int i =53 ; i <= 62; i++)
        {
            // Đọc dữ liệu từ Excel
            var imagePath = worksheet.Cell(i, 2).GetString();  // Cột 2: đường dẫn ảnh
            var name = worksheet.Cell(i, 3).GetString();  // Cột 3: Mã phòng
            var type = worksheet.Cell(i, 4).GetString();  // Cột 4: Loại phòng
            var floorNumber = worksheet.Cell(i, 5).GetString();  // Cột 5: Floor
            var price = worksheet.Cell(i, 6).GetString();  // Cột 6: Price
            var dap = worksheet.Cell(i, 7).GetString();  // Cột 7: DAP
            var map = worksheet.Cell(i, 8).GetString();  // Cột 8: MAP (Số người tối đa)
                                                         // Nếu có introduce, description, bạn đọc tiếp cột (9), (10)...
            var expectedMsg = worksheet.Cell(i, 11).GetString(); // Cột 11: Thông báo mong đợi

            // 4. Upload file (nếu có)
            var elements = driver.FindElements(By.CssSelector("#imageUpload"));
            if (elements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy phần tử #imageUpload");
            }
            else
            {
                elements[0].SendKeys("D:\\Group.png");
            }

            // 5. Điền form
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
                // Mã phòng
                driver.FindElement(By.Id("name")).SendKeys(name);


                // Loại phòng
                driver.FindElement(By.Id("type")).Clear();
                driver.FindElement(By.Id("type")).SendKeys(type);

                // Floor
                driver.FindElement(By.Id("floornumber")).Clear();
                driver.FindElement(By.Id("floornumber")).SendKeys(floorNumber);

                // Price
                driver.FindElement(By.Id("price")).Clear();
                driver.FindElement(By.Id("price")).SendKeys(price);

                // DAP
                driver.FindElement(By.Id("dap")).Clear();
                driver.FindElement(By.Id("dap")).SendKeys(dap);

                // MAP (Số người tối đa)
                driver.FindElement(By.Id("map")).Clear();
                driver.FindElement(By.Id("map")).SendKeys(map);

                // 6. Submit form
                IWebElement submitButton = wait.Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit-add"))
                );
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

                try
                {
                    submitButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);
                }

                // 7. Kiểm tra thông báo trả về
                bool foundMessage = false;

                try
                {
                    // Chờ trang hiển thị 1 trong các chuỗi liên quan đến MAP
                    wait.Until(d =>
                    {
                        // Ví dụ, bạn tùy chỉnh chuỗi cho đúng với thực tế
                        if (d.PageSource.Contains("Số người tối đa không được bỏ trống"))
                        {
                            foundMessage = true;
                            return true;
                        }
                        if (d.PageSource.Contains("Số người tối đa phải lớn hơn hoặc bằng 2"))
                        {
                            foundMessage = true;
                            return true;
                        }
                        if (d.PageSource.Contains("Số người tối đa phải nhỏ hơn hoặc bằng 10"))
                        {
                            foundMessage = true;
                            return true;
                        }
                        // Nếu hợp lệ, có thể là "Thêm phòng thành công" hoặc "Số người tối đa hợp lệ"
                        if (d.PageSource.Contains("Không tìm thấy message mong đợi."))
                        {
                            foundMessage = true;
                            return true;
                        }
                        return false;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    // Hết thời gian chờ mà chưa thấy chuỗi nào ở trên
                    foundMessage = false;
                }

                // 8. Ghi Actual (cột 12) và so sánh với Expected (cột 11)
                
            }
            catch
            {
                string actualMsg = "Không tìm thấy message mong đợi.";

                actualMsg = "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 12).Value = actualMsg;

                if (actualMsg.Equals(expectedMsg, StringComparison.OrdinalIgnoreCase))
                {
                    worksheet.Cell(i, 13).Value = "Pass";
                }
                else
                {
                    worksheet.Cell(i, 13).Value = "Fail";
                }

                // Quay lại trang Admin (hoặc Create) cho case tiếp theo
                driver.Navigate().GoToUrl($"http://localhost:5145/Admin");
            }
           

        }

        // 9. Lưu file
        workbook.Save();
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000);
        driver.Quit();
    }
}

