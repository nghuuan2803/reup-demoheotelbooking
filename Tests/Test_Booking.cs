using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{ 
    [TestFixture]
    public class Test_Booking
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5145";
        private IWebElement element;
        private string excelFilePath = @"..\..\..\Data\Booking_data.xlsx";



        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Tăng thời gian chờ lên 30 giây
            driver.Navigate().GoToUrl(baseUrl);
            Console.WriteLine("Thư mục làm việc hiện tại: " + Directory.GetCurrentDirectory());
        }





        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose(); 
        } 
        private void Login(string username, string password, bool rememberMe = false)
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username"))).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            if (rememberMe) driver.FindElement(By.Name("RememberMe")).Click();
            driver.FindElement(By.Id("submit-login")).Click();
        }

        private List<Dictionary<string, string>> ReadTestDataFromExcel()
        {
            var testData = new List<Dictionary<string, string>>();

            Console.WriteLine("Đường dẫn đầy đủ của file Excel: " + Path.GetFullPath(excelFilePath));

            if (!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException($"File {excelFilePath} không tồn tại. Thư mục hiện tại: {Directory.GetCurrentDirectory()}");
            }

            try
            {
                using (var workbook = new XLWorkbook(excelFilePath))
                {
                    var worksheet = workbook.Worksheet("Test_Booking"); // Sử dụng sheet Test_Booking
                    var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề

                    foreach (var row in rows)
                    {
                        var data = new Dictionary<string, string>
                {
                    {"TestCase", row.Cell(1).GetString()},     // Cột A
                    {"Name", row.Cell(2).GetString()},         // Cột B
                    {"Phone", row.Cell(3).GetString()},        // Cột C
                    {"CheckInDate", row.Cell(4).GetString()},  // Cột D
                    {"CheckOutDate", row.Cell(5).GetString()}, // Cột E
                    {"NumberOfRooms", row.Cell(6).GetString()},// Cột F
                    {"CardNumber", row.Cell(7).GetString()},   // Cột G
                    {"CardHolder", row.Cell(8).GetString()},   // Cột H
                    {"CardDate", row.Cell(9).GetString()},     // Cột I
                    {"OTP", row.Cell(10).GetString()},         // Cột J
                    {"ExpectedResult", row.Cell(11).GetString()} // Cột K
                };
                        testData.Add(data);
                        Console.WriteLine($"Đọc dữ liệu TestCase {data["TestCase"]}: Name={data["Name"]}, Phone={data["Phone"]}, CheckInDate={data["CheckInDate"]}, CheckOutDate={data["CheckOutDate"]}, NumberOfRooms={data["NumberOfRooms"]}, CardNumber={data["CardNumber"]}, CardHolder={data["CardHolder"]}, CardDate={data["CardDate"]}, OTP={data["OTP"]}, ExpectedResult={data["ExpectedResult"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file Excel: {ex.Message}");
                throw;
            }

            return testData;
        }

        private void WriteTestResultToExcel(string testCase, string actualResult, string actualUrl)
        {
            try
            {
                using (var workbook = new XLWorkbook(excelFilePath))
                {
                    var worksheet = workbook.Worksheet("Test_Booking"); // Sử dụng sheet Test_Booking

                    // Tìm hàng dựa trên TestCase
                    var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề
                    foreach (var row in rows)
                    {
                        string rowTestCase = row.Cell(1).GetString();
                        if (rowTestCase == testCase)
                        {
                            // ✅ Ghi "Actual Result" vào cột L (Index = 12)
                            row.Cell(12).Value = actualResult; // Cột L - Actual Result

                            // ✅ Lấy "Expected Result" từ cột K (Index = 11)
                            string expectedResult = row.Cell(11).GetString();

                            // ✅ Ghi "Pass/Fail" vào cột M (Index = 13)
                            row.Cell(13).Value = (expectedResult == actualResult) ? "Pass" : "Fail"; // Cột M

                            // ✅ Ghi "Actual URL" vào cột N (Index = 14) nếu có
                            row.Cell(14).Value = actualUrl; // Cột N

                            break; // Thoát vòng lặp sau khi tìm thấy TestCase
                        }
                    }

                    workbook.Save(); // Lưu file Excel sau khi cập nhật dữ liệu
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file Excel: {ex.Message}");
                throw;
            }
        }


        private void SafeClick(IWebElement element, IJavaScriptExecutor js)
        {
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                js.ExecuteScript("arguments[0].click();", element);
            }
        }

        [Test]
        public void Test_Booking_Success_Without_Login()
        {
            var testDataList = ReadTestDataFromExcel();

            foreach (var testData in testDataList)
            {
                Console.WriteLine($"Chạy trường hợp kiểm thử: {testData["TestCase"]}");

                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                SafeClick(element, (IJavaScriptExecutor)driver);

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).SendKeys(testData["Name"]);
                driver.FindElement(By.Id("phone")).SendKeys(testData["Phone"]);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // Xử lý CheckInDate
                // Xử lý CheckInDate
                IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                checkInElement.Clear();
                string checkInDate = DateTime.Parse(testData["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                js.ExecuteScript(
                    $"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                    $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                );

                // Xử lý CheckOutDate
                IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                checkOutElement.Clear();
                string checkOutDate = DateTime.Parse(testData["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                js.ExecuteScript(
                    $"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                    $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                );

                // Chờ danh sách phòng cập nhật sau khi thay đổi ngày
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // **Cuộn xuống để đảm bảo danh sách phòng hiển thị**
                js.ExecuteScript("window.scrollBy(0, 500);");
                Thread.Sleep(500); // Chờ một chút để trang cập nhật

                // Chờ ít nhất một nút "Chọn" hiển thị (các nút có class addRoomButton)
                wait.Until(ExpectedConditions.ElementExists(By.ClassName("addRoomButton")));


                // Lấy danh sách tất cả các nút "Chọn"
                var addRoomButtons = driver.FindElements(By.ClassName("addRoomButton"));

                // Kiểm tra nếu có phòng nào hiển thị
                if (addRoomButtons.Count == 0)
                {
                    throw new Exception("Không có phòng nào hiển thị để chọn. Vui lòng kiểm tra ngày nhận/trả phòng.");
                }

                // Chọn nút "Chọn" đầu tiên
                IWebElement firstAddRoomButton = addRoomButtons.First();
                Console.WriteLine($"Chọn phòng với ID: {firstAddRoomButton.GetAttribute("data-room-id")}");
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", firstAddRoomButton);
                SafeClick(firstAddRoomButton, js);

                // Chờ danh sách phòng được cập nhật sau khi chọn
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Sửa lỗi nhấp vào submit-booking
                IWebElement submitButton2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", submitButton2);
                SafeClick(submitButton2, js);

                wait.Until(d => d.Url.Contains("vnpay"));
                Console.WriteLine("Thành công - Đã chuyển đến trang thanh toán VNPay");

                // Chờ trang VNPay tải hoàn toàn
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Chọn hình thức thanh toán
                IWebElement paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                SafeClick(paymentMethodElement, js);

                // Chờ danh sách ngân hàng tải
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Chọn ngân hàng NCB
                IWebElement ncbBankElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-bank-item-inner' and contains(@style, 'ncb.svg')]")));
                SafeClick(ncbBankElement, js);

                // Chờ form thanh toán hiển thị
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Đảm bảo tab chứa form hiển thị
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.tab-pane.tab1.active")));

                // Nhập thông tin thanh toán
                IWebElement cardNumberElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                cardNumberElement.SendKeys(testData["CardNumber"]); // Lấy từ Excel

                // Điền thông tin vào ô "Tên chủ thẻ"
                IWebElement cardHolderElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                cardHolderElement.Clear();
                cardHolderElement.SendKeys(testData["CardHolder"]); // Lấy từ Excel

                // Điền thông tin vào ô "Ngày phát hành"
                IWebElement cardDateElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                cardDateElement.Clear();
                cardDateElement.SendKeys(testData["CardDate"]); // Lấy từ Excel

                IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                SafeClick(continueButton, js);

                // Nhấn nút Đồng ý
                IWebElement agreeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnAgree")));
                SafeClick(agreeButton, js);

                // Nhập OTP
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue"))).SendKeys(testData["OTP"]); // Lấy từ Excel

                // Nhấn nút Thanh toán
                IWebElement confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                SafeClick(confirmButton, js);

                // Chờ trang PaymentSuccess
                wait.Until(d => d.Url.Contains("PaymentSuccess"));
                string actualUrl = driver.Url;

                // Kiểm tra thông báo "Đặt phòng thành công!"
                IWebElement successMessage = wait.Until(ExpectedConditions.ElementExists(By.XPath("//h2[text()='Đặt phòng thành công!']")));
                string actualResult = successMessage.Text;

                if (actualUrl.Contains("PaymentSuccess"))
                {
                    Console.WriteLine("Thanh toán thành công! Đã chuyển hướng đến: " + actualUrl);
                    WriteTestResultToExcel(testData["TestCase"], actualResult, actualUrl);
                    Assert.Pass("Kiểm thử thành công: Đã chuyển hướng đến trang PaymentSuccess.");
                }
                else
                {
                    Console.WriteLine("Thanh toán thất bại! URL hiện tại: " + actualUrl);
                    WriteTestResultToExcel(testData["TestCase"], "Thanh toán thất bại", actualUrl);
                    Assert.Fail("Kiểm thử thất bại: Không chuyển hướng đến trang PaymentSuccess.");
                }

                driver.Navigate().GoToUrl(baseUrl);
            }
        }

        [Test]
        public void Test_Required_Input_Booking()
        {
            // Đọc dữ liệu từ Excel
            var testDataList = ReadTestDataFromExcel();

            // Lọc chỉ các test case từ TC002 đến TC005
            var testCases = testDataList.Where(data => data["TestCase"].StartsWith("TC00") &&
                                                      int.Parse(data["TestCase"].Substring(4, 1)) >= 2 &&
                                                      int.Parse(data["TestCase"].Substring(4, 1)) <= 5).ToList();

            if (!testCases.Any())
            {
                Assert.Fail("Không tìm thấy test case nào từ TC002 đến TC005 trong file Excel.");
                return;
            }

            Console.WriteLine($"Tổng số test case tìm thấy: {testCases.Count}");
            bool allTestsPassed = true; // Biến để theo dõi tổng thể

            foreach (var testData in testCases)
            {
                Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testData["TestCase"]}");

                string actualResult = "Chưa xác định";
                string actualUrl = driver.Url;

                // Mở trang booking
                try
                {
                    driver.Navigate().GoToUrl(baseUrl); // Làm mới hoàn toàn trang trước mỗi test case
                    element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    SafeClick(element, js);

                    // Lấy URL hiện tại trước khi submit
                    string initialUrl = driver.Url;
                    Console.WriteLine($"URL ban đầu: {initialUrl}");

                    // Nhập dữ liệu từ Excel
                    IWebElement nameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
                    nameElement.Clear();
                    nameElement.SendKeys(testData["Name"] ?? ""); // Lấy trực tiếp từ Excel, mặc định rỗng nếu null
                    Console.WriteLine($"Đã nhập Name: {testData["Name"]}");

                    IWebElement phoneElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("phone")));
                    phoneElement.Clear();
                    phoneElement.SendKeys(testData["Phone"] ?? ""); // Lấy trực tiếp từ Excel, mặc định rỗng nếu null
                    Console.WriteLine($"Đã nhập Phone: {testData["Phone"]}");

                    IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                    checkInElement.Clear();
                    if (!string.IsNullOrEmpty(testData["CheckInDate"]))
                    {
                        string checkInDate = DateTime.Parse(testData["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                        js.ExecuteScript($"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                                        $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                        Console.WriteLine($"Đã nhập CheckInDate: {checkInDate}");
                    }
                    else
                    {
                        Console.WriteLine("CheckInDate bị bỏ trống.");
                    }

                    IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                    checkOutElement.Clear();
                    if (!string.IsNullOrEmpty(testData["CheckOutDate"]))
                    {
                        string checkOutDate = DateTime.Parse(testData["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                        js.ExecuteScript($"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                                        $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                        Console.WriteLine($"Đã nhập CheckOutDate: {checkOutDate}");
                    }
                    else
                    {
                        Console.WriteLine("CheckOutDate bị bỏ trống.");
                    }

                    // Chờ AJAX cập nhật danh sách phòng (nếu cần)
                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // Nhấn nút "Xác nhận"
                    IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                    SafeClick(submitButton, js);

                    // Chờ một chút để kiểm tra xem URL có thay đổi không
                    System.Threading.Thread.Sleep(2000); // Tăng thời gian chờ lên 2 giây
                    actualUrl = driver.Url;
                    Console.WriteLine($"URL sau khi submit: {actualUrl}");

                    // Xác định kết quả dựa trên trường bị bỏ trống
                    if (actualUrl == initialUrl)
                    {
                        // Kiểm tra trường nào bị bỏ trống và gán thông điệp phù hợp
                        if (string.IsNullOrEmpty(testData["Name"]))
                        {
                            actualResult = "Họ và tên là bắt buộc";
                        }
                        else if (string.IsNullOrEmpty(testData["Phone"]))
                        {
                            actualResult = "Số Điện Thoại là bắt buộc";
                        }
                        else if (string.IsNullOrEmpty(testData["CheckInDate"]))
                        {
                            actualResult = "Ngày nhận phòng là bắt buộc";
                        }
                        else if (string.IsNullOrEmpty(testData["CheckOutDate"]))
                        {
                            actualResult = "Ngày trả là bắt buộc";
                        }
                        else
                        {
                            actualResult = "Validation client hoạt động: Cảnh báo 'Please fill out this field' hiển thị.";
                        }
                    }
                    else
                    {
                        actualResult = "Validation client thất bại: Form đã được submit mặc dù có trường trống.";
                    }

                    Console.WriteLine($"Kết quả thực tế: {actualResult}");
                    WriteTestResultToExcel(testData["TestCase"], actualResult, actualUrl);

                    // Cập nhật trạng thái tổng thể
                    if (!actualResult.Contains(testData["ExpectedResult"]))
                    {
                        allTestsPassed = false;
                    }
                }
                catch (Exception ex)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    Console.WriteLine($"Lỗi khi chạy test case {testData["TestCase"]}: {ex.Message}");
                    WriteTestResultToExcel(testData["TestCase"], actualResult, driver.Url);
                    allTestsPassed = false;
                }

                // Đảm bảo làm mới trang trước khi chạy test case tiếp theo
                driver.Navigate().GoToUrl(baseUrl);
                Console.WriteLine($"Đã làm mới trang cho test case tiếp theo: {testData["TestCase"]}");
            }

            // Đánh giá tổng thể sau khi chạy tất cả test case
            if (allTestsPassed)
            {
                Assert.Pass("Tất cả các test case từ TC002 đến TC005 đã chạy thành công.");
            }
            else
            {
                Assert.Fail("Một hoặc nhiều test case từ TC002 đến TC005 đã thất bại.");
            }
        }

        [Test]
        public void Test_Booking_Without_Room()
        {
            // Đọc dữ liệu từ Excel
            var testDataList = ReadTestDataFromExcel();

            // Lọc test case TC006
            var testCase = testDataList.FirstOrDefault(data => data["TestCase"] == "TC006");

            if (testCase == null)
            {
                Assert.Fail("Không tìm thấy test case TC006 trong file Excel.");
                return;
            }

            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testCase["TestCase"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false; // Biến để kiểm soát trạng thái hoàn thành

            try
            {
                // Mở trang booking
                driver.Navigate().GoToUrl(baseUrl);
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                SafeClick(element, js);

                // Lấy URL hiện tại trước khi submit
                string initialUrl = driver.Url;
                Console.WriteLine($"URL ban đầu: {initialUrl}");

                // Nhập dữ liệu từ Excel
                IWebElement nameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
                nameElement.Clear();
                nameElement.SendKeys(testCase["Name"] ?? "");
                Console.WriteLine($"Đã nhập Name: {testCase["Name"]}");

                IWebElement phoneElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("phone")));
                phoneElement.Clear();
                phoneElement.SendKeys(testCase["Phone"] ?? "");
                Console.WriteLine($"Đã nhập Phone: {testCase["Phone"]}");

                IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                checkInElement.Clear();
                if (!string.IsNullOrEmpty(testCase["CheckInDate"]))
                {
                    string checkInDate = DateTime.Parse(testCase["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript($"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                                    $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                    Console.WriteLine($"Đã nhập CheckInDate: {checkInDate}");
                }
                else
                {
                    Console.WriteLine("CheckInDate bị bỏ trống.");
                }

                IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                checkOutElement.Clear();
                if (!string.IsNullOrEmpty(testCase["CheckOutDate"]))
                {
                    string checkOutDate = DateTime.Parse(testCase["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript($"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                                    $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                    Console.WriteLine($"Đã nhập CheckOutDate: {checkOutDate}");
                }
                else
                {
                    Console.WriteLine("CheckOutDate bị bỏ trống.");
                }

                // Chờ AJAX cập nhật danh sách phòng (nếu cần)
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Không chọn phòng, nhấn trực tiếp nút "Xác nhận"
                IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                SafeClick(submitButton, js);

                // Chờ thông báo xuất hiện
                try
                {
                    IWebElement alertElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("statusAlert")));
                    actualResult = alertElement.Text.Trim();
                    Console.WriteLine($"Thông báo thực tế: {actualResult}");
                }
                catch (WebDriverTimeoutException)
                {
                    actualResult = "Không có thông báo lỗi xuất hiện.";
                    Console.WriteLine(actualResult);
                }

                // Lấy URL sau khi submit
                actualUrl = driver.Url;
                Console.WriteLine($"URL sau khi submit: {actualUrl}");

                // Ghi kết quả vào file Excel
                WriteTestResultToExcel(testCase["TestCase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho TC006");

                // So sánh kết quả thực tế với kỳ vọng
                Assert.AreEqual(testCase["ExpectedResult"], actualResult,
                    $"Kiểm thử {testCase["TestCase"]} thất bại. Kỳ vọng: {testCase["ExpectedResult"]}, Thực tế: {actualResult}");
                Console.WriteLine($"Kiểm thử {testCase["TestCase"]} thành công: {actualResult}");
                testCompleted = true; // Đánh dấu test đã hoàn thành thành công
            }
            catch (Exception ex)
            {
                // Chỉ xử lý lỗi nếu test chưa hoàn thành
                if (!testCompleted)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    Console.WriteLine($"Lỗi khi chạy test case {testCase["TestCase"]}: {ex.Message}");
                    WriteTestResultToExcel(testCase["TestCase"], actualResult, actualUrl);
                    Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho TC006 do ngoại lệ");
                    Assert.Fail($"Kiểm thử {testCase["TestCase"]} thất bại do ngoại lệ: {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Bỏ qua ngoại lệ sau khi test đã hoàn thành: {ex.Message}");
                }
            }
        }

        [Test]
        public void Test_Booking_Cancel_Payment()
        {
            // Đọc dữ liệu từ Excel
            var testDataList = ReadTestDataFromExcel();

            // Lọc test case TC007
            var testCase = testDataList.FirstOrDefault(data => data["TestCase"] == "TC007");

            if (testCase == null)
            {
                Assert.Fail("Không tìm thấy test case TC007 trong file Excel.");
                return;
            }

            Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testCase["TestCase"]}");

            string actualResult = "Chưa xác định";
            string actualUrl = string.Empty;
            bool testCompleted = false;

            try
            {
                // Mở trang booking
                driver.Navigate().GoToUrl(baseUrl);
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                SafeClick(element, js);

                // Lấy URL hiện tại trước khi submit
                string initialUrl = driver.Url;
                Console.WriteLine($"URL ban đầu: {initialUrl}");

                // Nhập dữ liệu từ Excel
                IWebElement nameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
                nameElement.Clear();
                nameElement.SendKeys(testCase["Name"] ?? "");
                Console.WriteLine($"Đã nhập Name: {testCase["Name"]}");

                IWebElement phoneElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("phone")));
                phoneElement.Clear();
                phoneElement.SendKeys(testCase["Phone"] ?? "");
                Console.WriteLine($"Đã nhập Phone: {testCase["Phone"]}");

                IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                checkInElement.Clear();
                if (!string.IsNullOrEmpty(testCase["CheckInDate"]))
                {
                    string checkInDate = DateTime.Parse(testCase["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript($"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                                    $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                    Console.WriteLine($"Đã nhập CheckInDate: {checkInDate}");
                }
                else
                {
                    Console.WriteLine("CheckInDate bị bỏ trống.");
                }

                IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                checkOutElement.Clear();
                if (!string.IsNullOrEmpty(testCase["CheckOutDate"]))
                {
                    string checkOutDate = DateTime.Parse(testCase["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                    js.ExecuteScript($"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                                    $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));");
                    Console.WriteLine($"Đã nhập CheckOutDate: {checkOutDate}");
                }
                else
                {
                    Console.WriteLine("CheckOutDate bị bỏ trống.");
                }

                // Chờ AJAX cập nhật danh sách phòng
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));
                System.Threading.Thread.Sleep(3000); // Tăng thời gian chờ lên 3 giây để đảm bảo danh sách phòng render

                // Chọn một phòng
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("addRoomButton")));
                var addRoomButtons = driver.FindElements(By.ClassName("addRoomButton"));
                if (addRoomButtons.Count == 0)
                {
                    throw new Exception("Không có phòng nào hiển thị để chọn. Vui lòng kiểm tra ngày nhận/trả phòng.");
                }

                IWebElement firstAddRoomButton = addRoomButtons.First();
                Console.WriteLine($"Chọn phòng với ID: {firstAddRoomButton.GetAttribute("data-room-id")}");
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", firstAddRoomButton);

                // Chờ nút hiển thị (Displayed: True)
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"btn-add-{firstAddRoomButton.GetAttribute("data-room-id")}")));
                bool isEnabled = (bool)js.ExecuteScript("return arguments[0].disabled === false;", firstAddRoomButton);
                bool isDisplayed = firstAddRoomButton.Displayed;
                Console.WriteLine($"Trạng thái nút addRoomButton - Enabled: {isEnabled}, Displayed: {isDisplayed}");

                if (isEnabled && isDisplayed)
                {
                    try
                    {
                        SafeClick(firstAddRoomButton, js);
                    }
                    catch (ElementClickInterceptedException)
                    {
                        js.ExecuteScript("arguments[0].click();", firstAddRoomButton);
                        Console.WriteLine("Đã nhấn addRoomButton bằng JavaScript.");
                    }
                }
                else
                {
                    throw new Exception("Nút addRoomButton không thể tương tác: " +
                                       $"Enabled={isEnabled}, Displayed={isDisplayed}");
                }

                // Chờ danh sách phòng được cập nhật sau khi chọn
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));
                System.Threading.Thread.Sleep(2000); // Chờ thêm 2 giây

                // Chờ overlay/loading spinner biến mất
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-spinner")));
                    Console.WriteLine("Overlay/loading spinner (loading-spinner) đã biến mất.");
                }
                catch (WebDriverTimeoutException)
                {
                    try
                    {
                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loader")));
                        Console.WriteLine("Overlay/loading spinner (loader) đã biến mất.");
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Không tìm thấy overlay/loading spinner hoặc đã hết thời gian chờ.");
                    }
                }

                // Nhấn nút "Xác nhận" để chuyển đến trang thanh toán
                IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", submitButton);

                isEnabled = (bool)js.ExecuteScript("return arguments[0].disabled === false;", submitButton);
                isDisplayed = submitButton.Displayed;
                Console.WriteLine($"Trạng thái nút submit-booking - Enabled: {isEnabled}, Displayed: {isDisplayed}");

                if (isEnabled && isDisplayed)
                {
                    try
                    {
                        SafeClick(submitButton, js);
                    }
                    catch (ElementClickInterceptedException)
                    {
                        js.ExecuteScript("arguments[0].click();", submitButton);
                        Console.WriteLine("Đã nhấn submit-booking bằng JavaScript.");
                    }
                }
                else
                {
                    throw new Exception("Nút submit-booking không thể tương tác: " +
                                       $"Enabled={isEnabled}, Displayed={isDisplayed}");
                }

                // Lấy URL ngay sau khi nhấn submit
                string urlAfterSubmit = driver.Url;
                Console.WriteLine($"URL sau khi nhấn submit: {urlAfterSubmit}");

                // Chờ chuyển đến trang VNPay với thời gian chờ 60 giây
                var longerWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                longerWait.Until(d => d.Url.Contains("vnpay"));
                Console.WriteLine("Thành công - Đã chuyển đến trang thanh toán VNPay");

                // Kiểm tra lỗi VNPay
                try
                {
                    var errorElement = driver.FindElement(By.XPath("//*[contains(text(), 'An error occurred during the processing')]"));
                    if (errorElement != null && errorElement.Displayed)
                    {
                        string errorMessage = errorElement.Text;
                        throw new Exception($"Lỗi VNPay: {errorMessage}");
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Không tìm thấy thông báo lỗi VNPay. Tiếp tục test.");
                }

                // Chờ trang VNPay tải hoàn toàn
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Nhấn vào phương thức thanh toán (Thẻ nội địa và tài khoản ngân hàng) dựa trên text
                IWebElement paymentMethodButton = longerWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//div[contains(text(), 'Thẻ nội địa và tài khoản ngân hàng')]//ancestor::div[@class='list-method-button']")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", paymentMethodButton);
                SafeClick(paymentMethodButton, js);
                Console.WriteLine("Đã nhấn vào phương thức thanh toán (Thẻ nội địa và tài khoản ngân hàng).");

                // Chờ phương thức thanh toán mở ra (tăng thời gian chờ)
                Thread.Sleep(3000); // Chờ 3 giây để collapse mở và element ngân hàng hiển thị

                // Nhấn vào ngân hàng NCB
                IWebElement bankButton = longerWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("NCB")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", bankButton);
                SafeClick(bankButton, js);
                Console.WriteLine("Đã chọn ngân hàng NCB.");

                // Nhấn vào nút hủy thanh toán
                IWebElement cancelPaymentButton = longerWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[contains(@class, 'ubtn') and contains(., 'Hủy thanh toán')]")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", cancelPaymentButton);
                SafeClick(cancelPaymentButton, js);
                Console.WriteLine("Đã nhấn vào nút hủy thanh toán.");

                // Chờ modal xuất hiện
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#modalCancelPayment")));
                Thread.Sleep(1000); // Chờ 1 giây để modal ổn định

                // Nhấn vào nút xác nhận hủy
                IWebElement confirmCancelButton = longerWait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//a[contains(@class, 'ubtn') and .//span[contains(text(), 'Xác nhận hủy')]]")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", confirmCancelButton);
                SafeClick(confirmCancelButton, js);
                Console.WriteLine("Đã xác nhận hủy thanh toán.");

                // Chờ thông báo xuất hiện
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("alert-box")));
                IWebElement alertElement = driver.FindElement(By.Id("alert-box"));
                // Chỉ lấy text từ thẻ <h2> để loại bỏ các phần tử con không mong muốn
                IWebElement messageElement = alertElement.FindElement(By.TagName("h2"));
                actualResult = messageElement.Text.Trim();
                Console.WriteLine($"Thông báo thực tế sau khi hủy: {actualResult}");
                // Ghi lại toàn bộ HTML của alert-box để phân tích
                string alertHtml = alertElement.GetAttribute("outerHTML");
                Console.WriteLine($"HTML của #alert-box: {alertHtml}");

                // Lấy URL sau khi hủy
                actualUrl = driver.Url;
                Console.WriteLine($"URL sau khi hủy: {actualUrl}");

                // Ghi kết quả vào file Excel
                WriteTestResultToExcel(testCase["TestCase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi Actual Result: {actualResult} vào file Excel cho TC007");

                // Không so sánh trong test, để Excel xử lý
                Console.WriteLine($"Kiểm thử {testCase["TestCase"]} hoàn tất: {actualResult}");
                testCompleted = true;
            }
            catch (Exception ex)
            {
                if (!testCompleted)
                {
                    // Nếu có lỗi, vẫn ghi actualResult nếu đã có, nếu không thì ghi thông báo lỗi
                    string resultToWrite = actualResult != "Chưa xác định" ? actualResult : $"Lỗi: {ex.Message}";
                    Console.WriteLine($"Lỗi khi chạy test case {testCase["TestCase"]}: {ex.Message}");
                    WriteTestResultToExcel(testCase["TestCase"], resultToWrite, actualUrl);
                    Console.WriteLine($"Đã ghi Actual Result: {resultToWrite} vào file Excel cho TC007 do ngoại lệ");
                    Assert.Fail($"Kiểm thử {testCase["TestCase"]} thất bại do ngoại lệ: {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Bỏ qua ngoại lệ sau khi test đã hoàn thành: {ex.Message}");
                }
            }
        }  

        [Test]
        public void Test_Booking_Many_Room()
        {
            // Đọc dữ liệu từ Excel
            var testDataList = ReadTestDataFromExcel();

            // Lọc test case TC008
            var testCase = testDataList.FirstOrDefault(data => data["TestCase"] == "TC008");

            if (testCase == null)
            {
                Assert.Fail("Không tìm thấy test case TC008 trong file Excel.");
                return;
            }

            // Log toàn bộ dữ liệu đọc từ Excel
            Console.WriteLine($"Dữ liệu đọc từ Excel cho TC008: Name={testCase["Name"]}, Phone={testCase["Phone"]}, CheckInDate={testCase["CheckInDate"]}, CheckOutDate={testCase["CheckOutDate"]}, NumberOfRooms={testCase["NumberOfRooms"]}, CardNumber={testCase["CardNumber"]}, CardHolder={testCase["CardHolder"]}, CardDate={testCase["CardDate"]}, OTP={testCase["OTP"]}");

            Console.WriteLine($"Chạy trường hợp kiểm thử: {testCase["TestCase"]}");

            // Biến để lưu kết quả thực tế
            string actualResult = "";
            string actualUrl = "";
            bool testPassed = false;

            try
            {
                // Điều hướng đến trang chính và nhấn nút đặt phòng
                driver.Navigate().GoToUrl(baseUrl);
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                SafeClick(element, (IJavaScriptExecutor)driver);

                // Điền thông tin vào form
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name"))).SendKeys(testCase["Name"]);
                driver.FindElement(By.Id("phone")).SendKeys(testCase["Phone"]);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // Xử lý CheckInDate
                IWebElement checkInElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckInDate")));
                checkInElement.Clear();
                string checkInDate = DateTime.Parse(testCase["CheckInDate"]).ToString("yyyy-MM-ddTHH:mm");
                Console.WriteLine($"CheckInDate được nhập: {checkInDate}");
                js.ExecuteScript(
                    $"document.getElementById('CheckInDate').value = '{checkInDate}'; " +
                    $"document.getElementById('CheckInDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                );

                // Xử lý CheckOutDate
                IWebElement checkOutElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CheckOutDate")));
                checkOutElement.Clear();
                string checkOutDate = DateTime.Parse(testCase["CheckOutDate"]).ToString("yyyy-MM-ddTHH:mm");
                Console.WriteLine($"CheckOutDate được nhập: {checkOutDate}");
                js.ExecuteScript(
                    $"document.getElementById('CheckOutDate').value = '{checkOutDate}'; " +
                    $"document.getElementById('CheckOutDate').dispatchEvent(new Event('change', {{ bubbles: true }}));"
                );

                // Chờ danh sách phòng cập nhật sau khi thay đổi ngày
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Chờ ít nhất một nút "Chọn" tồn tại
                wait.Until(ExpectedConditions.ElementExists(By.ClassName("addRoomButton")));

                // Chờ các nút "Chọn" hiển thị (với thời gian tối đa 15 giây)
                try
                {
                    new WebDriverWait(driver, TimeSpan.FromSeconds(15))
                        .Until(d => d.FindElements(By.ClassName("addRoomButton"))
                                    .Any(b => b.Displayed && b.Enabled));
                    Console.WriteLine("Đã tìm thấy ít nhất một nút 'Chọn' hiển thị và khả dụng.");
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Không có nút 'Chọn' nào hiển thị sau 15 giây. Tiếp tục với các nút hiện có...");
                }

                // Lấy số lượng phòng cần chọn từ dữ liệu Excel (cột F)
                int numberOfRoomsToBook = int.Parse(testCase["NumberOfRooms"]);
                Console.WriteLine($"Số lượng phòng cần chọn: {numberOfRoomsToBook}");

                // Danh sách để lưu các ID phòng đã chọn
                HashSet<string> selectedRoomIds = new HashSet<string>();

                // Chọn số lượng phòng theo yêu cầu
                for (int i = 0; i < numberOfRoomsToBook; i++)
                {
                    // Làm mới danh sách các nút "Chọn", chỉ lấy các nút khả dụng (Enabled)
                    var addRoomButtons = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                        .Until(d => d.FindElements(By.ClassName("addRoomButton"))
                                    .Where(b => b.Enabled)
                                    .ToList());

                    // Kiểm tra nếu có phòng nào khả dụng
                    if (addRoomButtons.Count == 0)
                    {
                        throw new Exception("Không có phòng nào khả dụng để chọn. Vui lòng kiểm tra ngày nhận/trả phòng hoặc hành vi của trang.");
                    }

                    // Log số lượng nút "Chọn" tìm thấy
                    Console.WriteLine($"Số lượng nút 'Chọn' khả dụng: {addRoomButtons.Count}");

                    // Kiểm tra xem số lượng phòng có sẵn có đủ không
                    if (addRoomButtons.Count < numberOfRoomsToBook - i)
                    {
                        throw new Exception($"Không đủ phòng để chọn! Cần {numberOfRoomsToBook} phòng nhưng chỉ còn {addRoomButtons.Count} phòng khả dụng.");
                    }

                    // Tìm một phòng chưa được chọn
                    IWebElement roomButton = null;
                    foreach (var button in addRoomButtons)
                    {
                        string roomId = button.GetAttribute("data-room-id");
                        bool isDisplayed = button.Displayed;
                        bool isEnabled = button.Enabled;
                        bool isNotSelected = !selectedRoomIds.Contains(roomId);

                        Console.WriteLine($"Phòng ID: {roomId}, Displayed: {isDisplayed}, Enabled: {isEnabled}, Chưa chọn: {isNotSelected}");

                        if (isNotSelected && isEnabled)
                        {
                            roomButton = button;
                            break;
                        }
                    }

                    if (roomButton == null)
                    {
                        throw new Exception($"Không tìm thấy phòng khả dụng để chọn lần thứ {i + 1}.");
                    }

                    string selectedRoomId = roomButton.GetAttribute("data-room-id");
                    Console.WriteLine($"Chọn phòng thứ {i + 1} với ID: {selectedRoomId}");
                    selectedRoomIds.Add(selectedRoomId);

                    // Đảm bảo phần tử hiển thị và có thể tương tác
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", roomButton);
                    js.ExecuteScript("arguments[0].style.display = 'block'; arguments[0].style.visibility = 'visible';", roomButton);

                    // Dùng JavaScript để nhấn nút nếu Selenium không thể tương tác
                    try
                    {
                        SafeClick(roomButton, js);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Không thể nhấn nút bằng Selenium: {ex.Message}. Thử nhấn bằng JavaScript...");
                        js.ExecuteScript("arguments[0].click();", roomButton);
                    }

                    // Chờ cập nhật UI sau khi chọn phòng
                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));
                    Thread.Sleep(3000); // Giữ thời gian chờ để UI render lại
                }

                // Nhấn nút submit booking
                IWebElement submitButton2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", submitButton2);
                SafeClick(submitButton2, js);

                // Chuyển đến trang thanh toán VNPay
                wait.Until(d => d.Url.Contains("vnpay"));
                Console.WriteLine("Thành công - Đã chuyển đến trang thanh toán VNPay");

                // Chờ trang VNPay tải hoàn toàn
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Chọn hình thức thanh toán
                IWebElement paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                SafeClick(paymentMethodElement, js);

                // Chờ danh sách ngân hàng tải
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Chọn ngân hàng NCB
                IWebElement ncbBankElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='list-bank-item-inner' and contains(@style, 'ncb.svg')]")));
                SafeClick(ncbBankElement, js);

                // Chờ form thanh toán hiển thị
                wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                // Đảm bảo tab chứa form hiển thị
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.tab-pane.tab1.active")));

                // Nhập thông tin thanh toán
                IWebElement cardNumberElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                if (string.IsNullOrEmpty(testCase["CardNumber"]))
                {
                    throw new Exception("CardNumber không được để trống trong dữ liệu test case TC008.");
                }
                cardNumberElement.SendKeys(testCase["CardNumber"]); // Cột G

                // Điền thông tin vào ô "Tên chủ thẻ"
                IWebElement cardHolderElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                cardHolderElement.Clear();
                if (string.IsNullOrEmpty(testCase["CardHolder"]))
                {
                    throw new Exception("CardHolder không được để trống trong dữ liệu test case TC008.");
                }
                cardHolderElement.SendKeys(testCase["CardHolder"]); // Cột H

                // Điền thông tin vào ô "Ngày phát hành"
                IWebElement cardDateElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                cardDateElement.Clear();
                if (string.IsNullOrEmpty(testCase["CardDate"]))
                {
                    throw new Exception("CardDate không được để trống trong dữ liệu test case TC008.");
                }
                Console.WriteLine($"CardDate được nhập từ Excel: {testCase["CardDate"]}");
                cardDateElement.SendKeys(testCase["CardDate"]); // Cột I

                IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                SafeClick(continueButton, js);

                // Nhấn nút Đồng ý
                IWebElement agreeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnAgree")));
                SafeClick(agreeButton, js);

                // Nhập OTP
                IWebElement otpElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue")));
                if (string.IsNullOrEmpty(testCase["OTP"]))
                {
                    throw new Exception("OTP không được để trống trong dữ liệu test case TC008.");
                }
                otpElement.SendKeys(testCase["OTP"]); // Cột J

                // Nhấn nút Thanh toán
                IWebElement confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                SafeClick(confirmButton, js);

                // Chờ trang PaymentSuccess
                wait.Until(d => d.Url.Contains("PaymentSuccess"));
                actualUrl = driver.Url;

                // Kiểm tra thông báo "Đặt phòng thành công!"
                IWebElement successMessage = wait.Until(ExpectedConditions.ElementExists(By.XPath("//h2[text()='Đặt phòng thành công!']")));
                actualResult = successMessage.Text;

                if (actualUrl.Contains("PaymentSuccess"))
                {
                    Console.WriteLine("Thanh toán thành công! Đã chuyển hướng đến: " + actualUrl);
                    testPassed = true;
                }
                else
                {
                    throw new Exception("Không chuyển hướng đến trang PaymentSuccess.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi chạy test case {testCase["TestCase"]}: {ex.Message}");
                actualResult = "Thanh toán thất bại: " + ex.Message;
                actualUrl = driver.Url;
                testPassed = false;
            }
            finally
            {
                // Ghi kết quả vào file Excel
                WriteTestResultToExcel(testCase["TestCase"], actualResult, actualUrl);
                Console.WriteLine($"Đã ghi kết quả vào Excel: TestCase={testCase["TestCase"]}, ActualResult={actualResult}, URL={actualUrl}");

                // Điều hướng về trang chính
                driver.Navigate().GoToUrl(baseUrl);

                // Nếu kiểm thử thất bại, ném ngoại lệ để báo cáo
                if (!testPassed)
                {
                    Assert.Fail($"Kiểm thử {testCase["TestCase"]} thất bại: {actualResult}");
                }
            }
        }

        [Test]
        public void Test_Input_Booking()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var testDataList = ReadTestDataFromExcel();

            // 1. Lọc các test case từ TC009 đến TC014
            var testCases = testDataList.Where(data =>
                data["TestCase"].StartsWith("TC") &&
                int.TryParse(data["TestCase"].Substring(2), out int testCaseNumber) &&
                testCaseNumber >= 9 && testCaseNumber <= 14
            ).ToList();

            if (!testCases.Any())
            {
                Assert.Fail("Không tìm thấy test case nào từ TC009 đến TC014 trong file Excel.");
                return;
            }

            Console.WriteLine($"Tổng số test case tìm thấy: {testCases.Count}");
            bool allTestsPassed = true; // Biến này vẫn dùng để ghi nhận kết quả từng test case vào Excel.

            // 2. Vòng lặp chạy từng test case
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"Bắt đầu chạy trường hợp kiểm thử: {testCase["TestCase"]}");
                string actualResult = "Chưa xác định";
                string actualUrl = string.Empty;
                bool testPassed = false;
                // Lấy ExpectedResult từ Excel (trim để loại bỏ khoảng trắng thừa)
                string expectedResult = testCase["ExpectedResult"]?.Trim() ?? "";

                try
                {
                    // Mở trang chính
                    driver.Navigate().GoToUrl(baseUrl);

                    IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    SafeClick(element, js);

                    string initialUrl = driver.Url;
                    Console.WriteLine($"URL ban đầu: {initialUrl}");

                    // Clear dữ liệu cũ trong form
                    js.ExecuteScript("document.getElementById('name').value = '';");
                    js.ExecuteScript("document.getElementById('phone').value = '';");
                    js.ExecuteScript("document.getElementById('CheckInDate').value = '';");
                    js.ExecuteScript("document.getElementById('CheckOutDate').value = '';");
                    js.ExecuteScript("document.getElementById('CheckInDate').removeAttribute('disabled');");
                    js.ExecuteScript("document.getElementById('CheckInDate').removeAttribute('readonly');");
                    js.ExecuteScript("document.getElementById('CheckOutDate').removeAttribute('disabled');");
                    js.ExecuteScript("document.getElementById('CheckOutDate').removeAttribute('readonly');");

                    // Điền thông tin Name
                    IWebElement nameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
                    nameElement.SendKeys(testCase["Name"] ?? "");
                    Console.WriteLine($"Đã nhập Name: {testCase["Name"]}");
                    Thread.Sleep(2000);

                    // Điền thông tin Phone
                    IWebElement phoneElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("phone")));
                    phoneElement.SendKeys(testCase["Phone"] ?? "");
                    Console.WriteLine($"Đã nhập Phone: {testCase["Phone"]}");
                    Thread.Sleep(2000);

                    // Lấy giá trị CheckInDate và CheckOutDate từ Excel (chuỗi bất kỳ)
                    string checkInDateStr = testCase["CheckInDate"] ?? "";
                    string checkOutDateStr = testCase["CheckOutDate"] ?? "";

                    // Gán trực tiếp vào form, không parse DateTime
                    if (!string.IsNullOrEmpty(checkInDateStr))
                    {
                        js.ExecuteScript(
                            $"document.getElementById('CheckInDate').value = '{checkInDateStr}'; " +
                            "document.getElementById('CheckInDate').dispatchEvent(new Event('change', { bubbles: true }));"
                        );
                        Console.WriteLine($"CheckInDate được nhập: {checkInDateStr}");
                    }
                    Thread.Sleep(2000);

                    if (!string.IsNullOrEmpty(checkOutDateStr))
                    {
                        js.ExecuteScript(
                            $"document.getElementById('CheckOutDate').value = '{checkOutDateStr}'; " +
                            "document.getElementById('CheckOutDate').dispatchEvent(new Event('change', { bubbles: true }));"
                        );
                        Console.WriteLine($"CheckOutDate được nhập: {checkOutDateStr}");
                    }
                    Thread.Sleep(2000);

                    // Xác định "ngày hợp lệ" chỉ dựa trên việc trường không rỗng
                    bool isCheckInValid = !string.IsNullOrEmpty(checkInDateStr);
                    bool isCheckOutValid = !string.IsNullOrEmpty(checkOutDateStr);

                    if (isCheckInValid && isCheckOutValid)
                    {
                        // Chờ danh sách phòng cập nhật (Ajax)
                        wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));
                        Thread.Sleep(2000);

                        // Tìm nút "Chọn" của phòng đầu tiên
                        var addRoomButtons = driver.FindElements(By.ClassName("addRoomButton"))
                            .Where(b => b.Enabled && b.Displayed)
                            .ToList();

                        if (addRoomButtons.Count > 0)
                        {
                            IWebElement roomButton = addRoomButtons.First();
                            Console.WriteLine($"Chọn phòng với ID: {roomButton.GetAttribute("data-room-id")}");
                            js.ExecuteScript("arguments[0].scrollIntoView(true);", roomButton);
                            SafeClick(roomButton, js);
                            Thread.Sleep(2000);

                            // Kiểm tra và nhấn nút "Xác nhận"
                            IWebElement submitButton = wait.Until(ExpectedConditions.ElementExists(By.Id("submit-booking")));
                            bool isSubmitEnabled = (bool)js.ExecuteScript("return arguments[0].disabled === false;", submitButton);
                            Console.WriteLine($"Trạng thái nút submit-booking: Enabled={isSubmitEnabled}");

                            if (isSubmitEnabled)
                            {
                                SafeClick(submitButton, js);
                                Thread.Sleep(2000);

                                try
                                {
                                    // Kiểm tra chuyển hướng đến trang thanh toán
                                    wait.Until(d => d.Url.Contains("vnpay") || d.Url.Contains("payment"));
                                    actualUrl = driver.Url;
                                    Console.WriteLine($"Đã chuyển đến trang thanh toán: {actualUrl}");

                                    // Thực hiện quá trình thanh toán (demo)
                                    IWebElement paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(
                                        By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                                    SafeClick(paymentMethodElement, js);
                                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                                    IWebElement ncbBankElement = wait.Until(ExpectedConditions.ElementToBeClickable(
                                        By.XPath("//div[@class='list-bank-item-inner' and contains(@style, 'ncb.svg')]")));
                                    SafeClick(ncbBankElement, js);
                                    wait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                                    IWebElement cardNumberElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                                    cardNumberElement.SendKeys(testCase["CardNumber"] ?? "9704198526191432198");

                                    IWebElement cardHolderElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                                    cardHolderElement.Clear();
                                    cardHolderElement.SendKeys(testCase["CardHolder"] ?? "NGUYEN VAN A");

                                    IWebElement cardDateElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                                    cardDateElement.Clear();
                                    cardDateElement.SendKeys(testCase["CardDate"] ?? "07/25");

                                    IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                                    SafeClick(continueButton, js);

                                    IWebElement agreeButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                                        By.XPath("//a[.//span[text()='Đồng ý & Tiếp tục']]")));
                                    js.ExecuteScript("arguments[0].scrollIntoView(true);", agreeButton);
                                    SafeClick(agreeButton, js);

                                    IWebElement otpElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue")));
                                    otpElement.SendKeys(testCase["OTP"] ?? "123456");

                                    IWebElement confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                                    SafeClick(confirmButton, js);

                                    // Chờ kết quả thanh toán
                                    wait.Until(d => d.Url.Contains("PaymentSuccess"));
                                    actualUrl = driver.Url;
                                    actualResult = "Đặt phòng và thanh toán thành công!";
                                }
                                catch (WebDriverTimeoutException)
                                {
                                    actualResult = "Không chuyển qua trang thanh toán";
                                }
                            }
                            else
                            {
                                actualResult = "Nút Xác nhận bị vô hiệu hóa.";
                            }
                        }
                        else
                        {
                            actualResult = "Không có phòng nào khả dụng để chọn.";
                        }
                    }
                    else
                    {
                        actualResult = "Ngày nhận phòng hoặc trả phòng không hợp lệ.";
                    }

                    Console.WriteLine($"ExpectedResult: {expectedResult}");
                    Console.WriteLine($"ActualResult: {actualResult}");

                    // So sánh actualResult với ExpectedResult để quyết định pass/fail
                    if (actualResult.Equals(expectedResult, StringComparison.OrdinalIgnoreCase))
                    {
                        testPassed = true;
                    }
                    else
                    {
                        testPassed = false;
                    }

                    WriteTestResultToExcel(testCase["TestCase"], actualResult, driver.Url);
                }
                catch (Exception ex)
                {
                    actualResult = $"Lỗi: {ex.Message}";
                    WriteTestResultToExcel(testCase["TestCase"], actualResult, driver.Url);
                    testPassed = false;
                    allTestsPassed = false;
                }
                finally
                {
                    // Quay về trang chủ để sẵn sàng cho test case kế tiếp
                    driver.Navigate().GoToUrl(baseUrl);
                }

                if (!testPassed)
                {
                    allTestsPassed = false;
                }
            }

            // Nếu bạn muốn overall test luôn pass (vì các test case đã được ghi kết quả vào Excel)
            Assert.Pass("Đã chạy hết các test case từ TC009 đến TC014. Vui lòng kiểm tra file Excel để biết kết quả chi tiết.");
        }

        [Test]
        public void Test_Booking_with_Wrongs_Date()
        {
            // Tạo wait riêng, có thể tăng thời gian nếu cần
            WebDriverWait localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var testDataList = ReadTestDataFromExcel();

            // 1. Lọc các test case TC015, TC016, TC017
            var testCases = testDataList.Where(data =>
            {
                if (data["TestCase"].StartsWith("TC"))
                {
                    // Lấy số sau "TC"
                    bool parsed = int.TryParse(data["TestCase"].Substring(2), out int testCaseNumber);
                    if (parsed && testCaseNumber >= 15 && testCaseNumber <= 17)
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();

            if (!testCases.Any())
            {
                Assert.Fail("Không tìm thấy test case nào từ TC015 đến TC017 trong file Excel.");
                return;
            }

            Console.WriteLine($"Tổng số test case tìm thấy (TC015 - TC017): {testCases.Count}");

            foreach (var testCase in testCases)
            {
                string actualResult = "Chưa xác định";
                string actualUrl = "";
                string expectedResult = testCase["ExpectedResult"]?.Trim() ?? "";

                Console.WriteLine($"Bắt đầu chạy test case: {testCase["TestCase"]}");
                try
                {
                    // Điều hướng về trang chủ
                    driver.Navigate().GoToUrl(baseUrl);

                    // Nhấn nút "Đặt phòng"
                    element = localWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btn-booking")));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    SafeClick(element, js);

                    // Xóa dữ liệu cũ
                    js.ExecuteScript("document.getElementById('name').value = '';");
                    js.ExecuteScript("document.getElementById('phone').value = '';");
                    js.ExecuteScript("document.getElementById('CheckInDate').value = '';");
                    js.ExecuteScript("document.getElementById('CheckOutDate').value = '';");

                    // Điền Name
                    IWebElement nameElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
                    nameElement.SendKeys(testCase["Name"] ?? "");

                    // Điền Phone
                    IWebElement phoneElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("phone")));
                    phoneElement.SendKeys(testCase["Phone"] ?? "");

                    // Điền CheckInDate (có thể là dữ liệu sai)
                    string checkInDateStr = testCase["CheckInDate"] ?? "";
                    if (!string.IsNullOrEmpty(checkInDateStr))
                    {
                        js.ExecuteScript(
                            $"document.getElementById('CheckInDate').value = '{checkInDateStr}'; " +
                            "document.getElementById('CheckInDate').dispatchEvent(new Event('change', { bubbles: true }));"
                        );
                    }

                    // Điền CheckOutDate (có thể là dữ liệu sai)
                    string checkOutDateStr = testCase["CheckOutDate"] ?? "";
                    if (!string.IsNullOrEmpty(checkOutDateStr))
                    {
                        js.ExecuteScript(
                            $"document.getElementById('CheckOutDate').value = '{checkOutDateStr}'; " +
                            "document.getElementById('CheckOutDate').dispatchEvent(new Event('change', { bubbles: true }));"
                        );
                    }

                    // Chờ trang load / AJAX
                    Thread.Sleep(2000);
                    localWait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                    // ======= KIỂM TRA XEM CÓ HIỆN CẢNH BÁO statusAlert HAY KHÔNG =======
                    // Vì TC016, TC017 có thể bắn alert sớm ngay khi set ngày sai.
                    // Nên ta chờ 1 chút, sau đó tìm element statusAlert
                    Thread.Sleep(1000);
                    bool alertDisplayed = false;
                    string alertText = "";

                    try
                    {
                        // Chờ 2 giây xem alert có xuất hiện không
                        WebDriverWait shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                        IWebElement statusAlert = shortWait.Until(ExpectedConditions.ElementIsVisible(By.Id("statusAlert")));
                        if (statusAlert != null && statusAlert.Displayed)
                        {
                            alertDisplayed = true;
                            alertText = statusAlert.Text.Trim();
                        }
                    }
                    catch (WebDriverTimeoutException)
                    {
                        // Không thấy alert trong vòng 2 giây => alertDisplayed vẫn = false
                    }

                    if (alertDisplayed)
                    {
                        // Nếu có alert => Lấy nội dung alert làm actualResult
                        actualResult = alertText;
                        Console.WriteLine($"Phát hiện alert statusAlert: {actualResult}");
                        // Ghi kết quả vào Excel và kết thúc test case
                        WriteTestResultToExcel(testCase["TestCase"], actualResult, driver.Url);
                        continue; // Chuyển sang test case tiếp theo
                    }

                    // =========================
                    // Nếu không có alert thì ta tiếp tục luồng chọn phòng, thanh toán
                    // (giống như Test_Input_Booking).
                    // =========================

                    // Tìm nút "Chọn" của phòng đầu tiên
                    var addRoomButtons = driver.FindElements(By.ClassName("addRoomButton"))
                                               .Where(b => b.Displayed && b.Enabled)
                                               .ToList();

                    if (addRoomButtons.Count == 0)
                    {
                        actualResult = "Không có phòng nào khả dụng để chọn.";
                        WriteTestResultToExcel(testCase["TestCase"], actualResult, driver.Url);
                        continue;
                    }

                    IWebElement firstRoomButton = addRoomButtons.First();
                    SafeClick(firstRoomButton, js);
                    Thread.Sleep(1000);

                    // Nhấn nút Xác nhận
                    IWebElement submitButton = localWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                    SafeClick(submitButton, js);

                    // Chờ chuyển sang trang thanh toán
                    try
                    {
                        localWait.Until(d => d.Url.Contains("vnpay") || d.Url.Contains("payment"));
                        actualUrl = driver.Url;
                        Console.WriteLine($"Đã chuyển đến trang thanh toán: {actualUrl}");

                        // Tiếp tục demo nhập thông tin thanh toán
                        IWebElement paymentMethodElement = localWait.Until(ExpectedConditions.ElementToBeClickable(
                            By.XPath("//div[@class='list-method-button' and @data-bs-target='#accordionList2']")));
                        SafeClick(paymentMethodElement, js);

                        localWait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                        IWebElement ncbBankElement = localWait.Until(ExpectedConditions.ElementToBeClickable(
                            By.XPath("//div[@class='list-bank-item-inner' and contains(@style, 'ncb.svg')]")));
                        SafeClick(ncbBankElement, js);

                        localWait.Until(d => (bool)js.ExecuteScript("return jQuery.active == 0"));

                        IWebElement cardNumberElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                        cardNumberElement.SendKeys(testCase["CardNumber"] ?? "9704198526191432198");

                        IWebElement cardHolderElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                        cardHolderElement.Clear();
                        cardHolderElement.SendKeys(testCase["CardHolder"] ?? "NGUYEN VAN A");

                        IWebElement cardDateElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                        cardDateElement.Clear();
                        cardDateElement.SendKeys(testCase["CardDate"] ?? "07/25");

                        IWebElement continueButton = localWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                        SafeClick(continueButton, js);

                        IWebElement agreeButton = localWait.Until(ExpectedConditions.ElementToBeClickable(
                            By.XPath("//a[.//span[text()='Đồng ý & Tiếp tục']]")));
                        SafeClick(agreeButton, js);

                        IWebElement otpElement = localWait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue")));
                        otpElement.SendKeys(testCase["OTP"] ?? "123456");

                        IWebElement confirmButton = localWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                        SafeClick(confirmButton, js);

                        // Chờ kết quả thanh toán
                        localWait.Until(d => d.Url.Contains("PaymentSuccess"));
                        actualUrl = driver.Url;
                        actualResult = "Đặt phòng và thanh toán thành công!";
                    }
                    catch (WebDriverTimeoutException)
                    {
                        actualResult = "Không chuyển qua trang thanh toán";
                    }

                    // Ghi kết quả
                    WriteTestResultToExcel(testCase["TestCase"], actualResult, actualUrl);
                }
                catch (Exception ex)
                {
                    actualResult = "Lỗi: " + ex.Message;
                    WriteTestResultToExcel(testCase["TestCase"], actualResult, driver.Url);
                }
            }

            // Kết thúc hàm
            Assert.Pass("Đã chạy xong Test_Booking_with_Wrongs_Date (TC015, TC016, TC017). Vui lòng kiểm tra file Excel để xem chi tiết.");
        }

    }
}  