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

namespace SeleniumTest
{
    internal class Intergration_Test
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5145"; // Điều chỉnh URL nếu cần

        [SetUp]
        public void Setup()
        {
            // Khởi tạo driver
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60)); // Tăng thời gian chờ lên 60 giây
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 800); // Đảm bảo chiều rộng > 992px
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

        private void SafeClick(IWebElement element, IJavaScriptExecutor js)
        {
            try
            {
                if (!element.Displayed || !element.Enabled)
                    throw new Exception("Element không hiển thị hoặc không thể nhấp được.");
                element.Click();
            }
            catch (Exception)
            {
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                js.ExecuteScript("arguments[0].click();", element);
            }
        }

        // Đọc dữ liệu từ sheet Test_Booking_And_Verify_Info
        private List<Dictionary<string, string>> ReadTestDataFromExcel()
        {
            string excelPath = @"C:\Users\phand\OneDrive\Documents\ĐBCLPM\reup-demoheotelbooking\Tests\Data\Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Test_Booking_And_Verify_Info");
                var dataList = new List<Dictionary<string, string>>();
                int rowCount = worksheet.RowsUsed().Count();

                Console.WriteLine($"Tổng số hàng trong sheet Test_Booking_And_Verify_Info: {rowCount}");

                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 (bỏ header)
                {
                    string testCase = worksheet.Cell(row, 1).GetString();
                    if (!string.IsNullOrWhiteSpace(testCase))
                    {
                        var data = new Dictionary<string, string>
                        {
                            ["TestCase"] = testCase,
                            ["Name"] = worksheet.Cell(row, 2).GetString(),
                            ["Phone"] = worksheet.Cell(row, 3).GetString(),
                            ["CheckInDate"] = worksheet.Cell(row, 4).GetString(),
                            ["CheckOutDate"] = worksheet.Cell(row, 5).GetString(),
                            ["CardNumber"] = worksheet.Cell(row, 6).GetString(),
                            ["CardHolder"] = worksheet.Cell(row, 7).GetString(),
                            ["CardDate"] = worksheet.Cell(row, 8).GetString(),
                            ["OTP"] = worksheet.Cell(row, 9).GetString(),
                            ["Tên Đăng Nhập"] = worksheet.Cell(row, 10).GetString(), // Thêm cột Tên Đăng Nhập
                            ["Mật Khẩu"] = worksheet.Cell(row, 11).GetString()      // Thêm cột Mật Khẩu
                        };
                        Console.WriteLine($"Đọc dữ liệu hàng {row}: TestCase={testCase}, Name={data["Name"]}, Phone={data["Phone"]}, CheckInDate={data["CheckInDate"]}, CheckOutDate={data["CheckOutDate"]}, CardNumber={data["CardNumber"]}, CardHolder={data["CardHolder"]}, CardDate={data["CardDate"]}, OTP={data["OTP"]}, Tên Đăng Nhập={data["Tên Đăng Nhập"]}, Mật Khẩu={data["Mật Khẩu"]}");
                        dataList.Add(data);
                    }
                }
                return dataList;
            }
        }

        // Ghi kết quả vào sheet Test_Booking_And_Verify_Info
        private void WriteTestResultToExcel(string testCase, string infoBooking, string infoTest, string actualUrl)
        {
            string excelPath = @"C:\Users\phand\OneDrive\Documents\ĐBCLPM\reup-demoheotelbooking\Tests\Data\Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Test_Booking_And_Verify_Info");
                int rowCount = worksheet.RowsUsed().Count();

                Console.WriteLine($"Tìm test case: {testCase}, Tổng số hàng: {rowCount}");

                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 (bỏ header)
                {
                    string cellValue = worksheet.Cell(row, 1).GetString(); // Cột A (Testcase)
                    if (cellValue == testCase)
                    {
                        Console.WriteLine($"Tìm thấy test case {testCase} tại hàng {row}");
                        worksheet.Cell(row, 12).Value = infoBooking; // Cột L (Info_Booking)
                        worksheet.Cell(row, 13).Value = infoTest;   // Cột M (Info_Test)
                        worksheet.Cell(row, 14).FormulaA1 = $"=IF(TRIM(CLEAN(L{row}))=TRIM(CLEAN(M{row})), \"Pass\", \"Fail\")"; // Cột N (Pass/Fail)
                        Console.WriteLine($"Ghi Info_Booking: {infoBooking} vào cột L, Info_Test: {infoTest} vào cột M, hàng {row}");

                        string expectedResult = worksheet.Cell(row, 12).Value.ToString().Trim();
                        string actualResult = worksheet.Cell(row, 13).Value.ToString().Trim();
                        Console.WriteLine($"So sánh: Expected='{expectedResult}', Actual='{actualResult}'");
                        Console.WriteLine($"Độ dài Expected: {expectedResult.Length}, Độ dài Actual: {actualResult.Length}");
                        break;
                    }
                }

                workbook.Save();
                Console.WriteLine($"Đã lưu file Excel: {excelPath}");
            }
        }

        [Test]
        public void Test_Booking_And_Verify_Info()
        {
            var testDataList = ReadTestDataFromExcel();

            foreach (var testData in testDataList)
            {
                Console.WriteLine($"Chạy trường hợp kiểm thử: {testData["TestCase"]}");

                string infoBooking = "Chưa xác định";
                string infoTest = "Chưa xác định";
                string actualUrl = string.Empty;
                bool testCompleted = false;

                try
                {
                    // Bước 1: Thực hiện quy trình đặt phòng mà không đăng nhập
                    driver.Navigate().GoToUrl(baseUrl); // Thêm điều hướng đến trang chính
                    Console.WriteLine($"URL hiện tại sau khi điều hướng: {driver.Url}");

                    // Kiểm tra sự tồn tại của btn-booking
                    IWebElement element;
                    try
                    {
                        element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        Console.WriteLine($"Không tìm thấy nút btn-booking sau 60 giây: {ex.Message}");
                        Console.WriteLine($"Nội dung trang: {driver.PageSource}");
                        throw;
                    }

                    SafeClick(element, (IJavaScriptExecutor)driver);

                    // Điền thông tin vào form và log để kiểm tra
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).SendKeys(testData["Name"]);
                    driver.FindElement(By.Id("phone")).SendKeys(testData["Phone"]);
                    Console.WriteLine($"Điền vào form - Name: {testData["Name"]}, Phone: {testData["Phone"]}");

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                    // Xử lý CheckInDate
                    IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                    checkInElement.Clear();
                    string checkInDate = DateTime.Parse(testData["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript(
                        $"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                        $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                    );
                    Console.WriteLine($"Điền CheckInDate: {checkInDate}");

                    // Xử lý CheckOutDate
                    IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                    checkOutElement.Clear();
                    string checkOutDate = DateTime.Parse(testData["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript(
                        $"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                        $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                    );
                    Console.WriteLine($"Điền CheckOutDate: {checkOutDate}");

                    // Kiểm tra giá trị thực tế trong form sau khi điền
                    string actualName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).GetAttribute("value");
                    string actualPhone = driver.FindElement(By.Id("phone")).GetAttribute("value");
                    string actualCheckIn = checkInElement.GetAttribute("value");
                    string actualCheckOut = checkOutElement.GetAttribute("value");
                    Console.WriteLine($"Giá trị thực tế trong form - Name: {actualName}, Phone: {actualPhone}, CheckIn: {actualCheckIn}, CheckOut: {actualCheckOut}");

                    // Thu thập thông tin từ form để ghi vào Info_Booking
                    infoBooking = Regex.Replace(
                        $"Khách hàng: {testData["Name"]}, Số điện thoại: {testData["Phone"]}, Ngày nhận phòng: {DateTime.Parse(testData["CheckInDate"]).ToString("M/d/yyyy h:mm:ss tt")}, Ngày trả phòng: {DateTime.Parse(testData["CheckOutDate"]).ToString("M/d/yyyy h:mm:ss tt")}",
                        @"\s+", " ").Trim();
                    Console.WriteLine($"Thông tin từ form (Info_Booking): {infoBooking}");

                    // Chờ danh sách phòng cập nhật
                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Chọn phòng đầu tiên
                    wait.Until(ExpectedConditions.ElementExists(By.ClassName("addRoomButton")));
                    var addRoomButtons = driver.FindElements(By.ClassName("addRoomButton"));
                    if (addRoomButtons.Count == 0)
                        throw new Exception("Không có phòng nào hiển thị để chọn.");

                    IWebElement firstAddRoomButton = addRoomButtons.First();
                    Console.WriteLine($"Chọn phòng với ID: {firstAddRoomButton.GetAttribute("data-room-id")}");
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", firstAddRoomButton);
                    SafeClick(firstAddRoomButton, js);

                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Nhấn submit booking
                    IWebElement submitButton2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", submitButton2);
                    SafeClick(submitButton2, js);

                    wait.Until(d => d.Url.Contains("vnpay"));
                    Console.WriteLine("Thành công - Đã chuyển đến trang thanh toán VNPay");

                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Chọn phương thức thanh toán
                    IWebElement paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                    SafeClick(paymentMethodElement, js);

                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Chọn ngân hàng NCB
                    IWebElement ncbBankElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-bank-item-inner' and contains(@style, 'ncb.svg')]")));
                    SafeClick(ncbBankElement, js);

                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.tab-pane.tab1.active")));

                    // Nhập thông tin thanh toán
                    IWebElement cardNumberElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                    cardNumberElement.SendKeys(testData["CardNumber"]);

                    IWebElement cardHolderElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                    cardHolderElement.Clear();
                    cardHolderElement.SendKeys(testData["CardHolder"]);

                    IWebElement cardDateElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                    cardDateElement.Clear();
                    cardDateElement.SendKeys(testData["CardDate"]);

                    IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                    SafeClick(continueButton, js);

                    IWebElement agreeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnAgree")));
                    SafeClick(agreeButton, js);

                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue"))).SendKeys(testData["OTP"]);

                    IWebElement confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                    SafeClick(confirmButton, js);

                    wait.Until(d => d.Url.Contains("PaymentSuccess"));
                    actualUrl = driver.Url;

                    if (!actualUrl.Contains("PaymentSuccess"))
                        throw new Exception("Thanh toán thất bại! URL hiện tại: " + actualUrl);

                    // Tăng thời gian chờ để đồng bộ dữ liệu
                    Thread.Sleep(10000); // Chờ 10 giây
                    driver.Navigate().GoToUrl(baseUrl);

                    // Bước 2: Đăng nhập và kiểm tra thông tin phòng
                    Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]); // Sử dụng thông tin từ Excel
                    Console.WriteLine($"Đã đăng nhập với Username: {testData["Tên Đăng Nhập"]}");

                    if (!driver.Url.Contains("/admin/Roommanager/roomstatus"))
                    {
                        driver.Navigate().GoToUrl($"{baseUrl}/admin/Roommanager/roomstatus");
                        wait.Until(d => d.Url.Contains("/admin/Roommanager/roomstatus"));
                    }

                    Thread.Sleep(5000);

                    try
                    {
                        IWebElement bookingListLinkCheck = driver.FindElement(By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]"));
                        Console.WriteLine("Nút 'DS Đặt phòng' tồn tại trên sidebar.");
                    }
                    catch (NoSuchElementException)
                    {
                        throw new Exception("Không tìm thấy nút 'DS Đặt phòng' trên sidebar.");
                    }

                    WebDriverWait extendedWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    IWebElement bookingListLink = extendedWait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//a[@href='/invoice/bookinglist' and .//span[text()='DS Đặt phòng']]")));

                    js.ExecuteScript("arguments[0].scrollIntoView(true);", bookingListLink);
                    SafeClick(bookingListLink, js);

                    // Làm mới trang để đảm bảo lấy dữ liệu mới nhất
                    driver.Navigate().Refresh();
                    wait.Until(d => d.Url.Contains("/invoice/bookinglist"));

                    // Đảm bảo danh sách đã được tải hoàn toàn
                    wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("table.table tbody tr")));
                    IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                    IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                    if (rows.Count > 0)
                    {
                        // Lấy phần tử cuối cùng trong danh sách
                        IWebElement lastRow = rows[rows.Count - 1];
                        // Cuộn đến phần tử cuối cùng để đảm bảo hiển thị
                        js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'end' });", lastRow);
                        Thread.Sleep(1000); // Chờ 1 giây để đảm bảo phần tử được hiển thị
                        IList<IWebElement> cells = lastRow.FindElements(By.CssSelector("td"));
                        if (cells.Count >= 4) // Chỉ cần 4 cột: Khách hàng, Số điện thoại, Ngày nhận phòng, Ngày trả phòng
                        {
                            string customer = cells[0].Text.Trim();
                            string phoneNumber = cells[1].Text.Trim();
                            string checkinDate = DateTime.Parse(cells[2].Text).ToString("M/d/yyyy h:mm:ss tt");
                            string checkoutDate = DateTime.Parse(cells[3].Text).ToString("M/d/yyyy h:mm:ss tt");

                            // Loại bỏ thông tin trạng thái, chỉ lấy 4 trường
                            infoTest = Regex.Replace($"Khách hàng: {customer}, Số điện thoại: {phoneNumber}, Ngày nhận phòng: {checkinDate}, Ngày trả phòng: {checkoutDate}",
                                @"\s+", " ").Trim();
                            Console.WriteLine($"Thông tin phòng cuối cùng (Info_Test): {infoTest}");
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
                    WriteTestResultToExcel(testData["TestCase"], infoBooking, infoTest, actualUrl);
                    Console.WriteLine($"Đã ghi Info_Booking: {infoBooking}, Info_Test: {infoTest} vào file Excel cho {testData["TestCase"]}");

                    Console.WriteLine($"Kiểm thử {testData["TestCase"]} hoàn tất.");
                    testCompleted = true;
                }
                catch (Exception ex)
                {
                    if (!testCompleted)
                    {
                        infoBooking = $"Lỗi: {ex.Message}";
                        infoTest = "Không xác định";
                        Console.WriteLine($"Lỗi khi chạy test case {testData["TestCase"]}: {ex.Message}");
                        WriteTestResultToExcel(testData["TestCase"], infoBooking, infoTest, actualUrl);
                        Console.WriteLine($"Đã ghi Info_Booking: {infoBooking}, Info_Test: {infoTest} vào file Excel cho {testData["TestCase"]} do ngoại lệ");
                        Assert.Fail($"Kiểm thử {testData["TestCase"]} thất bại do ngoại lệ: {ex.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Bỏ qua ngoại lệ sau khi test đã hoàn thành: {ex.Message}");
                    }
                }
            }
        }
    }
}