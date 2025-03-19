using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    [TestFixture]
    internal class Manager_Booking
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5145"; // Điều chỉnh URL nếu cần

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        private void Login(string username, string password, bool rememberMe = false)
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username"))).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            if (rememberMe) driver.FindElement(By.Name("RememberMe")).Click();
            driver.FindElement(By.Id("submit-login")).Click();
        }

        // Đọc dữ liệu từ sheet Manager_Booking với test case cụ thể
        private Dictionary<string, string> ReadTestDataFromExcel(string sheetName, string testCaseId)
        {
            string excelPath = "../../../Data/Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                var data = new Dictionary<string, string>();
                int rowCount = worksheet.RowsUsed().Count();

                Console.WriteLine($"Tổng số hàng trong sheet {sheetName}: {rowCount}");

                for (int row = 2; row <= rowCount; row++)
                {
                    string testCase = worksheet.Cell(row, 1).GetString(); // Cột A (Testcase)
                    if (testCase == testCaseId)
                    {
                        data["Testcase"] = testCase;
                        data["Tên Đăng Nhập"] = worksheet.Cell(row, 2).GetString(); // Cột B (Username)
                        data["Mật Khẩu"] = worksheet.Cell(row, 3).GetString(); // Cột C (Password)
                        data["BookingID"] = worksheet.Cell(row, 4).GetString(); // Cột D (BookingID)
                        data["Expected Result"] = worksheet.Cell(row, 5).Value.ToString().Trim(); // Cột E (Expected Result)
                        Console.WriteLine($"Đã tìm thấy {testCaseId} tại hàng {row}, BookingID: {data["BookingID"]}, Expected Result: {data["Expected Result"]}");
                        break;
                    }
                }
                return data;
            }
        }

        // Ghi kết quả vào sheet Manager_Booking
        private void WriteTestResultToExcel(string testCase, string actualResult, string actualUrl)
        {
            string excelPath = "../../../Data/Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Manager_Booking");
                int rowCount = worksheet.RowsUsed().Count();

                for (int row = 2; row <= rowCount; row++)
                {
                    string cellValue = worksheet.Cell(row, 1).GetString(); // Cột A (Testcase)
                    if (cellValue == testCase)
                    {
                        worksheet.Cell(row, 6).Value = actualResult; // Cột F (Actual Result)
                        worksheet.Cell(row, 7).FormulaA1 = $"=IF(TRIM(CLEAN(E{row}))=TRIM(CLEAN(F{row})), \"Pass\", \"Fail\")"; // Cột G (Pass/Fail)
                        Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào hàng {row}, cột F");
                        break;
                    }
                }
                workbook.Save();
                Console.WriteLine($"Đã lưu file Excel: {excelPath}");
            }
        }

        private void SafeClick(IWebElement element)
        {
            try
            {
                if (!element.Displayed || !element.Enabled)
                {
                    throw new Exception("Element không hiển thị hoặc không thể nhấp được.");
                }
                element.Click();
            }
            catch (Exception)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                js.ExecuteScript("arguments[0].click();", element);
            }
        }

        [Test]
        public void Test_CheckIn_Succees()
        {
            var testData = ReadTestDataFromExcel("Manager_Booking", "TC001");

            if (!testData.ContainsKey("Testcase") || testData["Testcase"] != "TC001")
            {
                Assert.Fail("Không tìm thấy test case TC001 trong sheet Manager_Booking.");
                return;
            }

            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testData["Testcase"]}");
            Console.WriteLine($"Expected Result: {testData["Expected Result"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false;

            try
            {
                Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
                Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");

                driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                Console.WriteLine($"URL sau khi điều hướng: {driver.Url}");

                Thread.Sleep(5000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")
                ));
                SafeClick(bookingListLink);
                Console.WriteLine("Đã nhấn vào 'DS Đặt phòng'");
                wait.Until(d => d.Url.Contains("/invoice/bookinglist"));

                IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                if (rows.Count == 0) throw new Exception("Không tìm thấy hàng nào trong bảng.");

                IWebElement lastRow = rows[rows.Count - 1];
                IList<IWebElement> cells = lastRow.FindElements(By.CssSelector("td"));
                if (cells.Count < 5) throw new Exception("Số cột trong hàng cuối không đủ.");

                string bookingId = testData["BookingID"];
                Console.WriteLine($"Booking ID: {bookingId}");

                IWebElement detailLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//a[@href='/Invoice/BookingDetails/{bookingId}']")
                ));
                SafeClick(detailLink);
                Console.WriteLine($"Đã nhấn vào 'Xem chi tiết' của booking {bookingId}");
                wait.Until(d => d.Url.Contains($"/Invoice/BookingDetails/{bookingId}"));

                string checkinXPath = $"//a[@href='/invoice/checkin/{bookingId}' and contains(@onclick, \"confirm('Xác nhận')\")]";
                IWebElement checkinButton = extendedWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checkinXPath)));
                SafeClick(checkinButton);
                Console.WriteLine("Đã nhấn vào nút 'Checkin'.");
                Thread.Sleep(2000);

                WebDriverWait alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                alertWait.Until(ExpectedConditions.AlertIsPresent());
                driver.SwitchTo().Alert().Accept();
                Console.WriteLine("Đã nhấn OK trên alert.");
                Thread.Sleep(4000);

                string checkoutXPath = $"//a[@href='/Invoice/Checkout/{bookingId}' and contains(text(), 'Trả phòng')]";
                bool isCheckoutVisible = false;
                try
                {
                    IWebElement checkoutButton = extendedWait.Until(ExpectedConditions.ElementExists(By.XPath(checkoutXPath)));
                    isCheckoutVisible = checkoutButton.Displayed;
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Không tìm thấy nút 'Trả phòng' sau khi Check-in.");
                }

                actualResult = isCheckoutVisible ? "Check-in succeed" : "Check-in fail";
                Console.WriteLine($"Kết quả Check-In: {actualResult}");

                actualUrl = driver.Url;
                WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                    Console.WriteLine($"Lỗi: {actualResult}");
                    Assert.Fail($"Kiểm thử thất bại: {ex.Message}");
                }
            }
        }

        [Test]
        public void Test_Cancel_CheckIn()
        {
            var testData = ReadTestDataFromExcel("Manager_Booking", "TC002");

            if (!testData.ContainsKey("Testcase") || testData["Testcase"] != "TC002")
            {
                Assert.Fail("Không tìm thấy test case TC002 trong sheet Manager_Booking.");
                return;
            }

            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testData["Testcase"]}");
            Console.WriteLine($"Expected Result: {testData["Expected Result"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false;

            try
            {
                Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
                Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");

                driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                Console.WriteLine($"URL sau khi điều hướng: {driver.Url}");

                Thread.Sleep(5000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")
                ));
                SafeClick(bookingListLink);
                Console.WriteLine("Đã nhấn vào 'DS Đặt phòng'");
                wait.Until(d => d.Url.Contains("/invoice/bookinglist"));

                IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                if (rows.Count == 0) throw new Exception("Không tìm thấy hàng nào trong bảng.");

                IWebElement lastRow = rows[rows.Count - 1];
                IList<IWebElement> cells = lastRow.FindElements(By.CssSelector("td"));
                if (cells.Count < 5) throw new Exception("Số cột trong hàng cuối không đủ.");

                string bookingId = testData["BookingID"];
                Console.WriteLine($"Booking ID: {bookingId}");

                IWebElement detailLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//a[@href='/Invoice/BookingDetails/{bookingId}']")
                ));
                SafeClick(detailLink);
                Console.WriteLine($"Đã nhấn vào 'Xem chi tiết' của booking {bookingId}");
                wait.Until(d => d.Url.Contains($"/Invoice/BookingDetails/{bookingId}"));

                string checkinXPath = $"//a[@href='/invoice/checkin/{bookingId}' and contains(@onclick, \"confirm('Xác nhận')\")]";
                IWebElement checkinButton = extendedWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checkinXPath)));
                SafeClick(checkinButton);
                Console.WriteLine("Đã nhấn vào nút 'Checkin'.");
                Thread.Sleep(2000);

                WebDriverWait alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                alertWait.Until(ExpectedConditions.AlertIsPresent());
                driver.SwitchTo().Alert().Dismiss();
                Console.WriteLine("Đã nhấn Cancel trên alert.");
                Thread.Sleep(4000);

                string checkoutXPath = $"//a[@href='/Invoice/Checkout/{bookingId}' and contains(text(), 'Trả phòng')]";
                bool isCheckoutVisible = false;
                try
                {
                    IWebElement checkoutButton = extendedWait.Until(ExpectedConditions.ElementExists(By.XPath(checkoutXPath)));
                    isCheckoutVisible = checkoutButton.Displayed;
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Không tìm thấy nút 'Trả phòng' sau khi hủy Check-in.");
                }

                actualResult = isCheckoutVisible ? "Check-in succeed" : "Check-in fail";
                Console.WriteLine($"Kết quả Cancel Check-In: {actualResult}");

                actualUrl = driver.Url;
                WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                    Console.WriteLine($"Lỗi: {actualResult}");
                    Assert.Fail($"Kiểm thử thất bại: {ex.Message}");
                }
            }
        }

        [Test]
        public void Test_CheckIn_Before_And_After()
        {
            // Lấy dữ liệu cho TC003
            var testDataTC003 = ReadTestDataFromExcel("Manager_Booking", "TC003");
            if (!testDataTC003.ContainsKey("Testcase") || testDataTC003["Testcase"] != "TC003")
            {
                Assert.Fail("Không tìm thấy test case TC003 trong sheet Manager_Booking.");
                return;
            }

            // Lấy dữ liệu cho TC004
            var testDataTC004 = ReadTestDataFromExcel("Manager_Booking", "TC004");
            if (!testDataTC004.ContainsKey("Testcase") || testDataTC004["Testcase"] != "TC004")
            {
                Assert.Fail("Không tìm thấy test case TC004 trong sheet Manager_Booking.");
                return;
            }

            // Thực hiện kiểm thử cho TC003
            Console.WriteLine($"Bắt đầu kiểm thử cho TC003:");
            PerformCheckInTest(testDataTC003);

            // Thực hiện kiểm thử cho TC004
            Console.WriteLine($"Bắt đầu kiểm thử cho TC004:");
            PerformCheckInTest(testDataTC004);
        }

        private void PerformCheckInTest(Dictionary<string, string> testData)
        {
            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testData["Testcase"]}");
            Console.WriteLine($"Expected Result: {testData["Expected Result"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false;

            try
            {
                Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
                Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");

                driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                Console.WriteLine($"URL sau khi điều hướng: {driver.Url}");

                Thread.Sleep(5000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")
                ));
                SafeClick(bookingListLink);
                Console.WriteLine("Đã nhấn vào 'DS Đặt phòng'");
                wait.Until(d => d.Url.Contains("/invoice/bookinglist"));

                IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                if (rows.Count == 0) throw new Exception("Không tìm thấy hàng nào trong bảng.");

                string bookingId = testData["BookingID"];
                Console.WriteLine($"Booking ID: {bookingId}");

                IWebElement detailLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//a[@href='/Invoice/BookingDetails/{bookingId}']")
                ));
                SafeClick(detailLink);
                Console.WriteLine($"Đã nhấn vào 'Xem chi tiết' của booking {bookingId}");
                wait.Until(d => d.Url.Contains($"/Invoice/BookingDetails/{bookingId}"));

                string checkinXPath = $"//a[@href='/invoice/checkin/{bookingId}' and contains(@onclick, \"confirm('Xác nhận')\")]";
                IWebElement checkinButton = extendedWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checkinXPath)));
                SafeClick(checkinButton);
                Console.WriteLine("Đã nhấn vào nút 'Checkin'.");
                Thread.Sleep(2000);

                WebDriverWait alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                alertWait.Until(ExpectedConditions.AlertIsPresent());
                driver.SwitchTo().Alert().Accept();
                Console.WriteLine("Đã nhấn OK trên alert.");
                Thread.Sleep(4000);

                string checkoutXPath = $"//a[@href='/Invoice/Checkout/{bookingId}' and contains(text(), 'Trả phòng')]";
                bool isCheckoutVisible = false;
                try
                {
                    IWebElement checkoutButton = extendedWait.Until(ExpectedConditions.ElementExists(By.XPath(checkoutXPath)));
                    isCheckoutVisible = checkoutButton.Displayed;
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Không tìm thấy nút 'Trả phòng' sau khi Check-in.");
                }

                actualResult = isCheckoutVisible ? "Check-in succeed" : "Check-in fail";
                Console.WriteLine($"Kết quả Check-In: {actualResult}");

                actualUrl = driver.Url;
                WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                    Console.WriteLine($"Lỗi: {actualResult}");
                    Assert.Fail($"Kiểm thử thất bại: {ex.Message}");
                }
            }
        }

        [Test]
        public void Test_Status_Check_In()
        {
            // Đọc dữ liệu cho TC005
            var testDataTC005 = ReadTestDataFromExcel("Manager_Booking", "TC005");
            if (!testDataTC005.ContainsKey("Testcase") || testDataTC005["Testcase"] != "TC005")
            {
                Assert.Fail("Không tìm thấy test case TC005 trong sheet Manager_Booking.");
                return;
            }

            // Đọc dữ liệu cho TC006
            var testDataTC006 = ReadTestDataFromExcel("Manager_Booking", "TC006");
            if (!testDataTC006.ContainsKey("Testcase") || testDataTC006["Testcase"] != "TC006")
            {
                Assert.Fail("Không tìm thấy test case TC006 trong sheet Manager_Booking.");
                return;
            }

            // Thực hiện kiểm thử cho TC005
            Console.WriteLine("Bắt đầu kiểm thử cho TC005:");
            PerformStatusCheckTest(testDataTC005);

            // Thực hiện kiểm thử cho TC006
            Console.WriteLine("Bắt đầu kiểm thử cho TC006:");
            PerformStatusCheckTest(testDataTC006);
        }

        private void PerformStatusCheckTest(Dictionary<string, string> testData)
        {
            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testData["Testcase"]}");
            Console.WriteLine($"Expected Result: {testData["Expected Result"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false;

            try
            {
                Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
                Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");

                driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                Console.WriteLine($"URL sau khi điều hướng: {driver.Url}");

                Thread.Sleep(5000);
                WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")
                ));
                SafeClick(bookingListLink);
                Console.WriteLine("Đã nhấn vào 'DS Đặt phòng'");
                wait.Until(d => d.Url.Contains("/invoice/bookinglist"));

                string bookingId = testData["BookingID"];
                Console.WriteLine($"Booking ID: {bookingId}");

                IWebElement detailLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath($"//a[@href='/Invoice/BookingDetails/{bookingId}']")
                ));
                SafeClick(detailLink);
                Console.WriteLine($"Đã nhấn vào 'Xem chi tiết' của booking {bookingId}");
                wait.Until(d => d.Url.Contains($"/Invoice/BookingDetails/{bookingId}"));

                // Trích xuất trạng thái dựa trên tiêu đề 'Status'
                IWebElement statusElement = extendedWait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//dt[contains(text(), 'Status')]/following-sibling::dd[1]")
                ));
                string statusText = statusElement.Text.Trim();
                Console.WriteLine($"Nội dung trạng thái: '{statusText}'");

                actualResult = $"Status: {statusText}";
                Console.WriteLine($"Kết quả thực tế: {actualResult}");

                actualUrl = driver.Url;
                WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                    Console.WriteLine($"Lỗi: {actualResult}");
                    Assert.Fail($"Kiểm thử thất bại: {ex.Message}");
                }
            }
        }
    }
}