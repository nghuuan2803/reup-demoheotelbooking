using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics.CodeAnalysis;

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
            // Khởi tạo driver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); // Tăng thời gian chờ lên 30 giây
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

        // Đọc dữ liệu từ sheet Manager_Booking
        private Dictionary<string, string> ReadTestDataFromExcel(string sheetName)
        {
            string excelPath = "../../../Data/Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                var data = new Dictionary<string, string>();
                int rowCount = worksheet.RowsUsed().Count();

                Console.WriteLine($"Tổng số hàng trong sheet {sheetName}: {rowCount}");

                // Đọc từ dòng 2 (bỏ qua header)
                for (int row = 2; row <= rowCount; row++)
                {
                    string testCase = worksheet.Cell(row, 1).GetString(); // Cột A (Testcase)
                    Console.WriteLine($"Kiểm tra hàng {row}, Testcase: '{testCase}', Tên Đăng Nhập: '{worksheet.Cell(row, 2).GetString()}', Mật Khẩu: '{worksheet.Cell(row, 3).GetString()}', Expected Result (D{row}): '{worksheet.Cell(row, 4).Value.ToString()}'");

                    if (testCase == "TC001")
                    {
                        data["Testcase"] = testCase;
                        data["Tên Đăng Nhập"] = worksheet.Cell(row, 2).GetString(); // Cột B (Tên Đăng Nhập)
                        data["Mật Khẩu"] = worksheet.Cell(row, 3).GetString(); // Cột C (Mật Khẩu)
                        data["Expected Result"] = worksheet.Cell(row, 4).Value.ToString().Trim(); // Cột D (Expected Result)
                        Console.WriteLine($"Đã tìm thấy TC001 tại hàng {row}, Expected Result: '{data["Expected Result"]}'");
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

                Console.WriteLine($"Tìm test case: {testCase}, Tổng số hàng: {rowCount}");

                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 (bỏ header)
                {
                    string cellValue = worksheet.Cell(row, 1).GetString(); // Cột A (Testcase)
                    Console.WriteLine($"Kiểm tra hàng {row}, Giá trị cột A: {cellValue}");

                    if (cellValue == testCase)
                    {
                        Console.WriteLine($"Tìm thấy test case {testCase} tại hàng {row}");
                        worksheet.Cell(row, 5).Value = actualResult; // Cột E (Actual Result, index 5)
                        worksheet.Cell(row, 6).FormulaA1 = $"=IF(TRIM(CLEAN(D{row}))=TRIM(CLEAN(E{row})), \"Pass\", \"Fail\")"; // So sánh cột D và E
                        Console.WriteLine($"Ghi Actual Result: {actualResult} vào cột E, hàng {row}");

                        // Lấy giá trị Expected Result từ cột D
                        string expectedResult = worksheet.Cell(row, 4).Value.ToString().Trim();
                        Console.WriteLine($"Ghi Pass/Fail công thức vào cột F, hàng {row} với Expected: '{expectedResult}', Actual: '{actualResult}'");
                        Console.WriteLine($"Độ dài Expected: {expectedResult.Length}, Độ dài Actual: {actualResult.Length}");
                        break;
                    }
                }

                workbook.Save();
                Console.WriteLine($"Đã lưu file Excel: {excelPath}");
            }
        }

        // Hàm an toàn để nhấp chuột
        private void SafeClick(IWebElement element)
        {
            try
            {
                // Kiểm tra trạng thái element
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
        public void Test_Info_Booking()
        {
            // Đọc dữ liệu từ sheet Manager_Booking trong Excel
            var testData = ReadTestDataFromExcel("Manager_Booking");

            // Lọc test case (dùng TC001 cho đăng nhập admin)
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
                // 1. Đăng nhập bằng hàm Login
                Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
                Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");
                Console.WriteLine($"URL sau khi đăng nhập: {driver.Url}");

                // 2. Điều hướng trực tiếp đến trang quản lý
                if (!driver.Url.Contains("/admin/Roommanager/roomstatus"))
                {
                    driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                    wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                    Console.WriteLine($"URL sau khi điều hướng: {driver.Url}");
                }

                // Chờ thêm để đảm bảo trang tải hoàn toàn
                Thread.Sleep(5000);

                // Khai báo IJavaScriptExecutor một lần duy nhất
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // 3. Kiểm tra sự tồn tại và trạng thái của nút "DS Đặt phòng" trên sidebar
                try
                {
                    IWebElement bookingListLinkCheck = driver.FindElement(By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]"));
                    Console.WriteLine("Nút 'DS Đặt phòng' tồn tại trên sidebar.");
                    string displayStyle = (string)js.ExecuteScript("return window.getComputedStyle(arguments[0]).display;", bookingListLinkCheck);
                    string opacity = (string)js.ExecuteScript("return window.getComputedStyle(arguments[0]).opacity;", bookingListLinkCheck);
                    Console.WriteLine($"Trạng thái hiển thị: display={displayStyle}, opacity={opacity}");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Nút 'DS Đặt phòng' không tồn tại trên sidebar.");
                    throw new Exception("Không tìm thấy nút 'DS Đặt phòng' trên sidebar. Kiểm tra selector hoặc role người dùng.");
                }

                // Nhấn vào element "DS Đặt phòng" trên sidebar
                WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60)); // Tăng thời gian chờ lên 60 giây
                IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")));

                // Kiểm tra trạng thái element
                Console.WriteLine($"Trạng thái nút DS Đặt phòng - Displayed: {bookingListLink.Displayed}, Enabled: {bookingListLink.Enabled}");

                // Cuộn đến element và nhấp
                js.ExecuteScript("arguments[0].scrollIntoView(true);", bookingListLink);
                SafeClick(bookingListLink);
                Console.WriteLine("Đã nhấn vào 'DS Đặt phòng' để vào danh sách đặt phòng.");

                // Chờ trang danh sách đặt phòng tải
                wait.Until(d => d.Url.Contains("/invoice/bookinglist"));
                Console.WriteLine($"URL danh sách đặt phòng: {driver.Url}");
                
                // 4. Lấy thông tin của phòng cuối cùng trong danh sách
                IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                if (rows.Count > 0)
                {
                    IWebElement lastRow = rows[rows.Count - 1]; // Lấy hàng cuối cùng
                    IList<IWebElement> cells = lastRow.FindElements(By.CssSelector("td"));

                    if (cells.Count >= 5) // Đảm bảo có đủ cột
                    {
                        string customer = cells[0].Text.Trim();
                        string phoneNumber = cells[1].Text.Trim();
                        string checkinDate = DateTime.Parse(cells[2].Text).ToString("M/d/yyyy h:mm:ss tt"); // Chuẩn hóa định dạng
                        string checkoutDate = DateTime.Parse(cells[3].Text).ToString("M/d/yyyy h:mm:ss tt"); // Chuẩn hóa định dạng
                        string status = cells[4].Text.Trim();

                        // Làm sạch chuỗi, loại bỏ ký tự không mong muốn
                        actualResult = Regex.Replace($"Khách hàng: {customer}, Số điện thoại: {phoneNumber}, Ngày nhận phòng: {checkinDate}, Ngày trả phòng: {checkoutDate}, Trạng thái: {status}", @"\s+", " ").Trim();
                        Console.WriteLine($"Thông tin phòng cuối cùng (sau làm sạch): {actualResult}");
                    }
                    else
                    {
                        throw new Exception("Số cột trong hàng cuối không đủ để lấy thông tin.");
                    }
                }
                else
                {
                    throw new Exception("Không tìm thấy hàng nào trong bảng.");
                }

                // Ghi kết quả vào Excel
                actualUrl = driver.Url;
                WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho {testData["Testcase"]}");

                Console.WriteLine($"Kiểm thử {testData["Testcase"]} hoàn tất: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    Console.WriteLine($"Lỗi khi chạy test case {testData["Testcase"]}: {ex.Message}");
                    WriteTestResultToExcel(testData["Testcase"], actualResult, actualUrl);
                    Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho {testData["Testcase"]} do ngoại lệ");
                    Assert.Fail($"Kiểm thử {testData["Testcase"]} thất bại do ngoại lệ: {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Bỏ qua ngoại lệ sau khi test đã hoàn thành: {ex.Message}");
                }
            }
        }

    }
}