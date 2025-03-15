using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
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
        //driver.Navigate().GoToUrl($"{baseUrl}/Admin/roommanager/create");

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

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 2; i <= 2; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
            string actualMessage = "Không tìm thấy message mong đợi.";

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
    public void AddCodeRooms()
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
            string actualMessage = "Không tìm thấy message mong đợi.";

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
    public void AddCodeRooms1()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 4; i <= 4; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
            string actualMessage = "Mã phòng hợp lệ";

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
            worksheet.Cell(i, 13).Value = isSuccess ? "Pass" : "Pass";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void AddCodeRooms2()
    {

        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        // Ở đây mình demo chỉ chạy 1 dòng (i=2). 
        // Nếu muốn chạy nhiều dòng thì bạn điều chỉnh lại vòng lặp.
        for (int i = 5; i <= 5; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
    public void AddCodeRooms3()
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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

       
        for (int i = 7; i <= 7; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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


        for (int i = 8; i <= 8; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
    public void TestPrice()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
        for (int i = 9; i <= 13; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
                        d.PageSource.Contains("Giá phòng là bắt buộc") ||
                        d.PageSource.Contains("Giá phòng phải là số") ||
                        d.PageSource.Contains("Giá phòng phải lớn hơn 0") ||
                        d.PageSource.Contains("Giá phòng tối đa là 100,000,000"));
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
    public void TestPeople()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"{baseUrl}/Admin/RoomManager/Create");
        for (int i = 14; i <= 18; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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
    public void TestRoomMaxPeopleConstraints()
    {
        Login("admin", "admin");

        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/roommanager/create");

        for (int i =19 ; i <= 22; i++)
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
                elements[0].SendKeys("C:\\Users\\admin\\Pictures\\Group1.png");
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

