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
        //driver.Navigate().GoToUrl($"{baseUrl}/Admin/roommanager/create");

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
        // Đăng nhập với quyền Admin
        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 2; i <= 2; i++)
        {
            
            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID
           

            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateCodeRooms()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 3; i <= 3; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateCodeRooms1()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 4; i <= 4; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateCodeRooms2()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 5; i <= 5; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateCodeRooms3()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 6; i <= 6; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateRoomsType()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 7; i <= 7; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdateRoomsType2()
    {

        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        // Ví dụ duyệt qua 1 dòng (i=2); nếu cần test nhiều dòng thì điều chỉnh vòng lặp.
        for (int i = 8; i <= 8; i++)
        {

            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();

            // Điều hướng đến trang Update dựa theo RoomID


            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Điền form Update: sử dụng Clear() để xóa giá trị cũ, sau đó SendKeys.
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

            driver.FindElement(By.Id("Introduce")).Clear();
            driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys(description);

            // Submit form Update
            // Lưu ý: trên view, nút submit có id="submit-update"
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

            // Ghi Actual và Pass/Fail vào Excel
            worksheet.Cell(i, 11).Value = actualMessage;
            worksheet.Cell(i, 12).Value = isSuccess ? "Pass" : "Fail";

            // Quay về trang Admin (hoặc trang create) để chuẩn bị cho case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
        }

        workbook.Save();
    }
    [Test]
    public void UpdatePrice()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        for (int i = 9; i <= 13; i++)
        {
            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
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

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys(description);


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
                worksheet.Cell(i, 11).Value = isSuccess ? "Success" : "fail.";
                worksheet.Cell(i, 12).Value = (expected == actualMessage && isSuccess) || (expected == "Fail" && !isSuccess) ? "Pass" : "Fail";
            }
            catch (Exception ex)
            {
                worksheet.Cell(i, 11).Value = "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 12).Value = "Fail";
            }

            // Điều hướng lại trang để thêm phòng mới
            driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
            workbook.Save();

        }

    }
    
    [Test]
    public void TestPeople()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        for (int i = 14; i <= 18; i++)
        {
            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
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

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys(description);


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
                worksheet.Cell(i, 11).Value = isSuccess ? "Success" : "fail.";
                worksheet.Cell(i, 12).Value = (expected == actualMessage && isSuccess) || (expected == "Fail" && !isSuccess) ? "Pass" : "Fail";
            }
            catch (Exception ex)
            {
                worksheet.Cell(i, 11).Value = "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 12).Value = "Fail";
            }

            // Điều hướng lại trang để thêm phòng mới
            driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
            workbook.Save();

        }
    }
    [Test]
    public void TestRoomMaxPeopleConstraints()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
        for (int i = 19; i <= 22; i++)
        {
            var name = worksheet.Cell(i, 2).GetString();
            var type = worksheet.Cell(i, 3).GetString();
            var floorNumber = worksheet.Cell(i, 4).GetString();
            var price = worksheet.Cell(i, 5).GetString();
            var dap = worksheet.Cell(i, 6).GetString();
            var map = worksheet.Cell(i, 7).GetString();
            var introduce = worksheet.Cell(i, 8).GetString();
            var description = worksheet.Cell(i, 9).GetString();
            var expected = worksheet.Cell(i, 10).GetString();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
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

                driver.FindElement(By.Id("Introduce")).Clear();
                driver.FindElement(By.Id("Introduce")).SendKeys(introduce);

                driver.FindElement(By.Id("Description")).Clear();
                driver.FindElement(By.Id("Description")).SendKeys(description);


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
                worksheet.Cell(i, 11).Value = isSuccess ? "Success" : "fail.";
                worksheet.Cell(i, 12).Value = (expected == actualMessage && isSuccess) || (expected == "Fail" && !isSuccess) ? "Pass" : "Fail";
            }
            catch (Exception ex)
            {
                worksheet.Cell(i, 11).Value = "Không tìm thấy message mong đợi.";
                worksheet.Cell(i, 12).Value = "Fail";
            }

            // Điều hướng lại trang để thêm phòng mới
            driver.Navigate().GoToUrl($"http://localhost:5145/Admin/RoomManager/Update/171");
            workbook.Save();

        }
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000);
        driver.Quit();
    }
}

