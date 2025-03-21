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
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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
            string excelPath = "../../../Data/Booking_data.xlsx";
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
            string excelPath = "../../../Data/Booking_data.xlsx";
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
                // Chỉ chạy test khi TestCase là TC001
                if (!testData["TestCase"].Equals("TC001", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Console.WriteLine($"Chạy trường hợp kiểm thử: {testData["TestCase"]}");

                string infoBooking = "Chưa xác định";
                string infoTest = "Chưa xác định";
                string actualUrl = string.Empty;
                bool testCompleted = false;

                try
                {
                    // Bước 1: Thực hiện quy trình đặt phòng mà không đăng nhập
                    driver.Navigate().GoToUrl(baseUrl);
                    // Mở rộng cửa sổ trình duyệt để đảm bảo giao diện responsive hiển thị nút "ĐẶT PHÒNG"
                    driver.Manage().Window.Maximize();
                    Console.WriteLine($"URL hiện tại sau khi điều hướng: {driver.Url}");

                    // Nếu phần tử có class "d-none", loại bỏ class đó bằng JavaScript
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    // Kiểm tra xem phần tử có tồn tại hay không trước khi cố gắng loại bỏ class
                    if (driver.FindElements(By.Id("btn-booking")).Count > 0)
                    {
                        js.ExecuteScript("document.getElementById('btn-booking').classList.remove('d-none');");
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy phần tử btn-booking để loại bỏ class 'd-none'.");
                    }

                    // Kiểm tra sự tồn tại của nút btn-booking và chờ đến khi có thể click được
                    IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    SafeClick(element, js);

                    // Điền thông tin vào form và log để kiểm tra
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).SendKeys(testData["Name"]);
                    driver.FindElement(By.Id("phone")).SendKeys(testData["Phone"]);
                    Console.WriteLine($"Điền vào form - Name: {testData["Name"]}, Phone: {testData["Phone"]}");

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

        // Hàm helper lấy giá trị hiện tại của Info_Booking (cột L) từ file Excel cho testCase hiện tại
        private string GetExistingInfoBooking(string testCase)
        {
            string excelPath = "../../../Data/Booking_data.xlsx";
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Test_Booking_And_Verify_Info");
                int rowCount = worksheet.RowsUsed().Count();
                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 (bỏ header)
                {
                    string cellValue = worksheet.Cell(row, 1).GetString(); // Cột A (TestCase)
                    if (cellValue.Equals(testCase, StringComparison.OrdinalIgnoreCase))
                    {
                        return worksheet.Cell(row, 12).GetString(); // Lấy giá trị của cột L
                    }
                }
            }
            return string.Empty;
        }

        [Test]
        public void Test_Booking_And_CheckIn()
        {
            var testDataList = ReadTestDataFromExcel();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver; // Khởi tạo js

            foreach (var testData in testDataList)
            {
                if (testData["TestCase"] != "TC002") continue; // Chỉ chạy TC002

                Console.WriteLine($"Chạy trường hợp kiểm thử: {testData["TestCase"]}");

                // Không cần ghi thông tin đặt phòng, chỉ lấy kết quả check-in
                string actualResult = "Chưa xác định";
                string actualUrl = string.Empty;
                bool testCompleted = false;

                try
                {
                    // Bước 1: Đặt phòng (không cần đăng nhập)
                    driver.Navigate().GoToUrl(baseUrl);
                    Console.WriteLine($"URL hiện tại sau khi điều hướng: {driver.Url}");

                    // Nhấn nút đặt phòng
                    IWebElement bookingButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    SafeClick(bookingButton, js);

                    // Điền thông tin vào form
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).SendKeys(testData["Name"]);
                    driver.FindElement(By.Id("phone")).SendKeys(testData["Phone"]);
                    Console.WriteLine($"Điền vào form - Name: {testData["Name"]}, Phone: {testData["Phone"]}");

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
                    IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", submitButton);
                    SafeClick(submitButton, js);

                    // Chuyển sang trang thanh toán VNPay
                    wait.Until(d => d.Url.Contains("vnpay"));
                    Console.WriteLine("Thành công - Đã chuyển đến trang thanh toán VNPay");

                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Bước 2: Thanh toán
                    IWebElement paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                    SafeClick(paymentMethodElement, js);
                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

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

                    // Xác nhận chuyển đến trang PaymentSuccess
                    wait.Until(d => d.Url.Contains("PaymentSuccess"));
                    actualUrl = driver.Url;
                    if (!actualUrl.Contains("PaymentSuccess"))
                        throw new Exception("Thanh toán thất bại! URL hiện tại: " + actualUrl);

                    // Tăng thời gian chờ để đồng bộ dữ liệu
                    Thread.Sleep(10000);
                    driver.Navigate().GoToUrl(baseUrl);

                    // Bước 3: Đăng nhập và kiểm tra thông tin đặt phòng
                    Login(testData["Tên Đăng Nhập"], testData["Mật Khẩu"]);
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

                    // --- Bắt đầu phần Check-in bằng cách chọn phần tử cuối cùng trong danh sách ---
                    wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("table.table tbody tr")));
                    IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.table")));
                    IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));
                    if (rows.Count == 0)
                        throw new Exception("Không tìm thấy hàng nào trong bảng.");

                    IWebElement lastRow = rows[rows.Count - 1];
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'end' });", lastRow);
                    Thread.Sleep(1000); // Chờ 1 giây để đảm bảo phần tử được hiển thị

                    // Nhấn vào nút "Xem chi tiết" từ hàng cuối cùng
                    IWebElement detailLink = lastRow.FindElement(By.XPath(".//a[contains(text(),'Xem chi tiết')]"));
                    SafeClick(detailLink, js);
                    Console.WriteLine("Đã nhấn vào 'Xem chi tiết' của booking cuối cùng.");
                    wait.Until(d => d.Url.Contains("BookingDetails"));

                    // Nhấn nút "Check in" trên trang chi tiết
                    string checkinXPath = "//a[contains(@href, '/invoice/checkin/') and contains(@onclick, \"confirm('Xác nhận')\")]";
                    IWebElement checkinButton = extendedWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checkinXPath)));
                    SafeClick(checkinButton, js);
                    Console.WriteLine("Đã nhấn vào nút 'Check in'.");
                    Thread.Sleep(2000);

                    // Chờ alert hiện ra và chấp nhận
                    WebDriverWait alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    alertWait.Until(ExpectedConditions.AlertIsPresent());
                    driver.SwitchTo().Alert().Accept();
                    Console.WriteLine("Đã nhấn OK trên alert.");
                    Thread.Sleep(4000);

                    // Kiểm tra xem nút "Trả phòng" có xuất hiện không
                    string checkoutXPath = "//a[contains(@href, '/Invoice/Checkout/') and contains(text(), 'Trả phòng')]";
                    bool isCheckoutVisible = false;
                    try
                    {
                        IWebElement checkoutButton = extendedWait.Until(ExpectedConditions.ElementExists(By.XPath(checkoutXPath)));
                        isCheckoutVisible = checkoutButton.Displayed;
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Không tìm thấy nút 'Trả phòng' sau khi Check in.");
                    }

                    actualResult = isCheckoutVisible ? "Check-in succeed" : "Check-in fail";
                    Console.WriteLine($"Kết quả Check-In: {actualResult}");
                    // --- Kết thúc phần Check-in ---

                    // Lấy giá trị hiện tại của Info_Booking (cột L) từ file Excel cho test case hiện tại
                    string existingInfoBooking = GetExistingInfoBooking(testData["TestCase"]);

                    // Ghi kết quả vào Excel, truyền giá trị existingInfoBooking để không thay đổi cột L
                    WriteTestResultToExcel(testData["TestCase"], existingInfoBooking, actualResult, actualUrl);
                    Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho {testData["TestCase"]}");

                    Console.WriteLine($"Kiểm thử {testData["TestCase"]} hoàn tất.");
                    testCompleted = true;
                }
                catch (Exception ex)
                {
                    if (!testCompleted)
                    {
                        string infoBooking = $"Lỗi: {ex.Message}";
                        actualResult = "Không xác định";
                        Console.WriteLine($"Lỗi khi chạy test case {testData["TestCase"]}: {ex.Message}");
                        // Lấy giá trị hiện tại của Info_Booking để không ghi đè cột L
                        string existingInfoBooking = GetExistingInfoBooking(testData["TestCase"]);
                        WriteTestResultToExcel(testData["TestCase"], existingInfoBooking, actualResult, actualUrl);
                        Console.WriteLine($"Đã ghi Info_Booking: {existingInfoBooking}, Actual Result: {actualResult} vào file Excel cho {testData["TestCase"]} do ngoại lệ");
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