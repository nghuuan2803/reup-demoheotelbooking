using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ClosedXML.Excel;
using System;
using SeleniumExtras.WaitHelpers;

namespace DemoHotelBooking.Tests
{
    [TestFixture] 
    public class InvoiceTests
    {
        private IWebDriver driver;
        private string baseUrl = "http://localhost:5145";
        private XLWorkbook workbook;
        private IXLWorksheet bookingWorksheet;      // Sheet BookingList
        private IXLWorksheet checkinWorksheet;      // Sheet Checkin
        private IXLWorksheet checkoutWorksheet;     // Sheet Checkout
        private IXLWorksheet paymentWorksheet;      // Sheet Payment
        private IXLWorksheet fullInvoiceWorksheet;
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize(); 
            workbook = new XLWorkbook("../../../Data/TestData_Invoice.xlsx");
            bookingWorksheet = workbook.Worksheet("BookingList");
            checkinWorksheet = workbook.Worksheet("Checkin");
            checkoutWorksheet = workbook.Worksheet("Checkout");
            paymentWorksheet = workbook.Worksheet("Payment");
            fullInvoiceWorksheet = workbook.Worksheet("FullInvoice");
        }

        private void LoginAndNavigateToBookingList()
        {
            var username = "admin";
            var password = "admin";

            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");
            var usernameField = driver.FindElement(By.Id("username"));  // Xác nhận ID thực tế
            var passwordField = driver.FindElement(By.Id("password"));
            var loginButton = driver.FindElement(By.Id("submit-login"));

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.Url.Contains("/Room"));

            var manageButton = driver.FindElement(By.CssSelector("a.btn.btn-primary[href='/admin/Roommanager/roomstatus']"));
            manageButton.Click();
        }

        [Test]
        public void Test_BookingList()
        {
            LoginAndNavigateToBookingList();

            driver.Navigate().GoToUrl($"{baseUrl}/invoice/bookinglist");
            for (int i = 2; i <= bookingWorksheet.LastRowUsed().RowNumber(); i++)
            {
                var bookingId = bookingWorksheet.Cell(i, 1).GetString();
                var expectedUrl = bookingWorksheet.Cell(i, 2).GetString();
                var expectedResult = bookingWorksheet.Cell(i, 3).GetString();

                var detailLink = driver.FindElement(By.XPath($"//a[@href='/Invoice/BookingDetails/{bookingId}']"));
                detailLink.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                bool isRedirected = wait.Until(d => d.Url.Contains(expectedUrl));
                string actualResult = isRedirected ? "Success" : "Failed to redirect";

                bookingWorksheet.Cell(i, 4).Value = actualResult; // Ghi ActualResult

                // Nếu ExpectedResult rỗng, đánh dấu test là Invalid
                if (string.IsNullOrWhiteSpace(expectedResult))
                {
                    bookingWorksheet.Cell(i, 5).Value = "Invalid Test Case: Missing ExpectedResult";
                }
                else
                {
                    bookingWorksheet.Cell(i, 5).Value = (actualResult == expectedResult) ? "Pass" : "Fail";
                }
                driver.Navigate().GoToUrl($"{baseUrl}/invoice/bookinglist");
            }
            workbook.Save();
        }

        [Test]
        public void Test_Checkin()
        {
            LoginAndNavigateToBookingList();

            for (int i = 2; i <= checkinWorksheet.LastRowUsed().RowNumber(); i++)
            {
                var bookingId = checkinWorksheet.Cell(i, 1).GetString();
                var expectedMessage = checkinWorksheet.Cell(i, 2).GetString();

                driver.Navigate().GoToUrl($"{baseUrl}/Invoice/BookingDetails/{bookingId}");
                var checkinButton = driver.FindElement(By.CssSelector("a.btn.btn-primary"));
                checkinButton.Click();

                driver.SwitchTo().Alert().Accept(); // Xác nhận dialog

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(d => d.Url.Contains("/Invoice/InvoiceDetail/"));
                string actualMessage = GetAlertMessage();

                checkinWorksheet.Cell(i, 3).Value = actualMessage; // Ghi ActualMessage

                // Kiểm tra nếu ExpectedMessage trống
                if (string.IsNullOrWhiteSpace(expectedMessage))
                {
                    checkinWorksheet.Cell(i, 4).Value = "Invalid Test Case: Missing ExpectedMessage";
                }
                else
                {
                    checkinWorksheet.Cell(i, 4).Value = (actualMessage == expectedMessage) ? "Pass" : "Fail";
                }
                // Nếu bạn có logic kiểm tra trạng thái sau checkin thì bổ sung thêm cột và so sánh
            }
            workbook.Save();
        }

        [Test]
        public void Test_Checkout()
        {
            LoginAndNavigateToBookingList();

            for (int i = 2; i <= checkoutWorksheet.LastRowUsed().RowNumber(); i++)
            {
                var invoiceId = checkoutWorksheet.Cell(i, 1).GetString();
                var expectedMessage = checkoutWorksheet.Cell(i, 2).GetString();

                driver.Navigate().GoToUrl($"{baseUrl}/Invoice/InvoiceDetail/{invoiceId}");
                var checkoutButton = driver.FindElement(By.CssSelector("a.btn.btn-danger"));
                checkoutButton.Click();

                driver.SwitchTo().Alert().Accept(); // Xác nhận dialog

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                string actualResult;
                string actualMessage;

                try
                {
                    wait.Until(d => d.Url.Contains($"/Invoice/invoicedetail/{invoiceId}"));
                    actualMessage = GetAlertMessage();
                    actualResult = $"Success - Message: {actualMessage}";
                }
                catch (WebDriverTimeoutException ex)
                {
                    actualResult = $"Failed: Timeout - {ex.Message}";
                    actualMessage = "No message found due to timeout";
                }

                checkoutWorksheet.Cell(i, 3).Value = actualResult;
                // Kiểm tra nếu ExpectedMessage trống
                if (string.IsNullOrWhiteSpace(expectedMessage))
                {
                    checkoutWorksheet.Cell(i, 4).Value = "Invalid Test Case: Missing ExpectedMessage";
                }
                else
                {
                    checkoutWorksheet.Cell(i, 4).Value = actualResult.StartsWith("Success") ? "Pass" : "Fail";
                }

                if (actualMessage != expectedMessage && actualResult.StartsWith("Success"))
                {
                    checkoutWorksheet.Cell(i, 4).Value += $" (Note: Expected '{expectedMessage}', got '{actualMessage}')";
                }
            }
            workbook.Save();
        }

        [Test]
        public void Test_Payment()
        {
            LoginAndNavigateToBookingList();

            for (int i = 2; i <= paymentWorksheet.LastRowUsed().RowNumber(); i++)
            {
                var invoiceId = paymentWorksheet.Cell(i, 1).GetString();
                var paymentMethod = paymentWorksheet.Cell(i, 2).GetString();
                var expectedUrl = paymentWorksheet.Cell(i, 3).GetString(); 

                driver.Navigate().GoToUrl($"{baseUrl}/Invoice/invoicedetail/{invoiceId}");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                string actualResult;
                string actualUrl = "";
                try
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Name("paymethod")));
                    var paymentSelect = new SelectElement(driver.FindElement(By.Name("paymethod")));
                    paymentSelect.SelectByValue(paymentMethod);

                    IWebElement confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
                    confirmButton.Click();

                    if (paymentMethod == "0")  // Tiền mặt
                    {
                        wait.Until(d => d.Url.Contains($"/Invoice/InvoiceDetail/{invoiceId}"));
                        actualResult = "Success - Thanh toán tiền mặt thành công";
                        actualUrl = driver.Url;
                    }
                    else if (paymentMethod == "1")  // MoMo
                    {
                        wait.Until(d => d.Url.Contains("test-payment.momo.vn"));
                        actualResult = "Success - Chuyển hướng đến MoMo thành công";
                        actualUrl = driver.Url;
                    }
                    else
                    {
                        actualResult = "Unknown Payment Method";
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    actualResult = "Failed: Timeout - Page did not load correctly";
                }
                catch (Exception ex)
                {
                    actualResult = $"Failed: {ex.Message}";
                }

                // Ghi kết quả vào file Excel
                paymentWorksheet.Cell(i, 5).Value = actualUrl; // Ghi lại actual URL
                paymentWorksheet.Cell(i, 6).Value = actualResult;
                paymentWorksheet.Cell(i, 7).Value = actualUrl.Contains(expectedUrl) ? "Pass" : "Fail"; // So sánh với ExpectedURL
            }
            workbook.Save();
        }
        [Test]
        public void Test_FullBookingProcess()
        {
            // Lặp qua các dòng trong file Excel
            for (int i = 2; i <= 2; i++)
            {
                // Lấy dữ liệu từ Excel
                var name = fullInvoiceWorksheet.Cell(i, 1).GetString();
                var phone = fullInvoiceWorksheet.Cell(i, 2).GetString();
                var checkinDate = fullInvoiceWorksheet.Cell(i, 3).GetString();
                var checkoutDate = fullInvoiceWorksheet.Cell(i, 4).GetString();
                var roomId = fullInvoiceWorksheet.Cell(i, 5).GetString(); // Ví dụ: "8" cho STD108
                var expectedResult = fullInvoiceWorksheet.Cell(i, 6).GetString();

                // Điều hướng đến trang đặt phòng
                driver.Navigate().GoToUrl($"{baseUrl}/booking/Booking");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));

                // Điền thông tin khách hàng
                var nameField = driver.FindElement(By.Id("name"));
                nameField.Clear();
                nameField.SendKeys(name);

                var phoneField = driver.FindElement(By.Id("phone"));
                phoneField.Clear();
                phoneField.SendKeys(phone);

                // Điền ngày nhận phòng
                var checkinField = driver.FindElement(By.Id("CheckInDate"));
                checkinField.Clear();
                var checkinDateFormatted = DateTime.Parse(checkinDate).ToString("yyyy-MM-ddTHH:mm");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.getElementById('CheckInDate').value = '{checkinDateFormatted}';");
                js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", checkinField);

                // Điền ngày trả phòng
                var checkoutField = driver.FindElement(By.Id("CheckOutDate"));
                checkoutField.Clear();
                var checkoutDateFormatted = DateTime.Parse(checkoutDate).ToString("yyyy-MM-ddTHH:mm");
                js.ExecuteScript($"document.getElementById('CheckOutDate').value = '{checkoutDateFormatted}';");
                js.ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }));", checkoutField);

                // Chọn phòng
                var addRoomButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector($"#availbleRoomList .addRoomButton[data-room-id='{roomId}']")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", addRoomButton);
                try
                {
                    addRoomButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", addRoomButton);
                }

                // Chờ phòng được thêm
                wait.Until(d => d.FindElement(By.CssSelector($"#roomListContainer .removeRoomButton[data-room-id='{roomId}']")).Displayed);

                // Gửi form đặt phòng
                var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("submit-booking")));
                js.ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                try
                {
                    submitButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", submitButton);
                }

                // Chờ chuyển hướng đến trang VNPAY
                wait.Until(d => d.Url.Contains("https://sandbox.vnpayment.vn/paymentv2/Transaction/PaymentMethod.html"));
                Console.WriteLine($"Row {i}: Đã chuyển đến trang thanh toán VNPay");

                // Chọn "Thẻ nội địa và tài khoản ngân hàng"
                var paymentMethodElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.list-method-button[data-bs-target='#accordionList2']")));
                try
                {
                    paymentMethodElement.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", paymentMethodElement);
                }

                // Chọn ngân hàng NCB
                var ncbBankElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("NCB")));
                try
                {
                    ncbBankElement.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", ncbBankElement);
                }

                // Chờ trang nhập thông tin thẻ
                wait.Until(d => d.Url.Contains("https://sandbox.vnpayment.vn/paymentv2/Ncb/Transaction/Index.html"));

                // Điền thông tin thẻ
                var cardNumberElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("card_number_mask")));
                cardNumberElement.SendKeys("9704198526191432198");

                var cardHolderElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardHolder")));
                cardHolderElement.SendKeys("NGUYEN VAN A");

                var cardDateElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cardDate")));
                cardDateElement.SendKeys("07/15");

                // Nhấn "Tiếp tục"
                var continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnContinue")));
                try
                {
                    continueButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", continueButton);
                }

                // Nhấn "Đồng ý & Tiếp tục"
                var agreeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnAgree")));
                try
                {
                    agreeButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", agreeButton);
                }

                // Nhập OTP
                var otpElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("otpvalue")));
                otpElement.SendKeys("123456");

                // Nhấn "Thanh toán"
                var confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnConfirm")));
                try
                {
                    confirmButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    js.ExecuteScript("arguments[0].click();", confirmButton);
                }

                // Chờ chuyển hướng đến trang thành công
                string actualResult;
                try
                {
                    wait.Until(d => d.Url.Contains("/Booking/PaymentSuccess"));
                    actualResult = "Success";
                    Console.WriteLine($"Row {i}: Thanh toán thành công! Đã chuyển hướng đến: {driver.Url}");
                }
                catch (WebDriverTimeoutException)
                {
                    actualResult = $"Failed: Stopped at {driver.Url}";
                    Console.WriteLine($"Row {i}: Thanh toán thất bại! URL hiện tại: {driver.Url}");
                }

                // Ghi kết quả vào Excel
                fullInvoiceWorksheet.Cell(i, 7).Value = actualResult;
                fullInvoiceWorksheet.Cell(i, 8).Value = (actualResult == expectedResult) ? "Pass" : "Fail";

                LoginAndNavigateToBookingList();

                driver.Navigate().GoToUrl($"{baseUrl}/invoice/bookinglist");

                // 3. Thực hiện Check-in
                // Lấy dữ liệu từ dòng 4 trong sheet FullInvoice
                var bookingIdCheckin = fullInvoiceWorksheet.Cell(5, 1).GetString();
                var expectedUrlCheckin = fullInvoiceWorksheet.Cell(5, 2).GetString();
                var expectedMessageCheckin = fullInvoiceWorksheet.Cell(8, 2).GetString(); // Lấy từ Excel, ví dụ: "Lập phiếu thành công"


                var detailButton = driver.FindElement(By.CssSelector($"a[href='/Invoice/BookingDetails/{bookingIdCheckin}']"));
                detailButton.Click();

                wait.Until(d => d.Url.Contains($"/Invoice/BookingDetails/{bookingIdCheckin}"));

                var checkinButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn.btn-primary")));
                checkinButton.Click();

                driver.SwitchTo().Alert().Accept();

                wait.Until(d => d.Url.Contains("/Invoice/InvoiceDetail/"));
                var actualUrlCheckin = driver.Url;
                var alertMessageCheckin = GetAlertMessage();

                // Ghi kết quả Check-in vào dòng 8
                fullInvoiceWorksheet.Cell(8, 1).Value = bookingIdCheckin;
                fullInvoiceWorksheet.Cell(8, 2).Value = expectedMessageCheckin; // "Lập phiếu thành công" từ Excel
                fullInvoiceWorksheet.Cell(8, 3).Value = alertMessageCheckin; // Từ GetAlertMessage()
                fullInvoiceWorksheet.Cell(8, 4).Value = (alertMessageCheckin == expectedMessageCheckin) ? "Pass" : "Fail";

                // 4. Thực hiện Check-out
                // Lấy InvoiceId từ URL sau khi Check-in thành công
                var invoiceId = driver.Url.Split('/').Last().Split('?').First();
                var expectedMessageCheckout = fullInvoiceWorksheet.Cell(12, 2).GetString();

                // Nhấn nút "Trả phòng" (giả sử nút có ID "btn-checkout")
                var checkoutButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn.btn-danger")));
                checkoutButton.Click();

                // Xác nhận từ dialog hệ thống
                driver.SwitchTo().Alert().Accept();

                // Chờ thông báo "Trả phòng thành công" và URL chuyển hướng
                wait.Until(d => d.Url.Contains("/Invoice/invoicedetail/"));
                var alertMessageCheckout = GetAlertMessage(); // Phương thức bạn đã có sẵn

                // Ghi kết quả Check-out vào dòng 11 trong sheet FullInvoice
                fullInvoiceWorksheet.Cell(12, 1).Value = invoiceId;
                fullInvoiceWorksheet.Cell(12, 2).Value = expectedMessageCheckout;
                fullInvoiceWorksheet.Cell(12, 3).Value = alertMessageCheckout;
                fullInvoiceWorksheet.Cell(12, 4).Value = (alertMessageCheckout == expectedMessageCheckout) ? "Pass" : "Fail";
                // 5. Thanh toán sau Check-out
                var paymentMethodDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("paymethod")));
                var selectElement = new SelectElement(paymentMethodDropdown);
                selectElement.SelectByValue("0"); // Chọn "Tiền mặt"

                var confirmPaymentButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input.btn.btn-primary")));
                confirmPaymentButton.Click();

                // Chờ chuyển hướng sau khi thanh toán
                string actualPaymentResult;
                string actualUrlPayment;
                try
                {
                    wait.Until(d => d.Url.Contains("/Invoice/InvoiceDetail/"));
                    actualUrlPayment = driver.Url;
                    var paymentMessage = GetAlertMessage();
                    // Sửa logic kiểm tra để khớp với thông báo thực tế
                    actualPaymentResult = paymentMessage.Contains("Thanh toán thành công") ? "Success - Thanh toán tiền mặt thành công" : $"Failed: {paymentMessage}";
                    Console.WriteLine($"Row {i}: Thanh toán sau Check-out: {actualPaymentResult}");
                }
                catch (WebDriverTimeoutException ex)
                {
                    actualUrlPayment = driver.Url;
                    actualPaymentResult = $"Failed: {ex.Message}";
                    Console.WriteLine($"Row {i}: Thanh toán sau Check-out thất bại! {ex.Message}");
                }

                // Ghi kết quả thanh toán vào dòng 16 trong sheet FullInvoice
                fullInvoiceWorksheet.Cell(16, 1).Value = invoiceId; // InvoiceId
                fullInvoiceWorksheet.Cell(16, 2).Value = "0"; // PaymentMethod
                fullInvoiceWorksheet.Cell(16, 3).Value = $"/Invoice/InvoiceDetail/{invoiceId}"; // ExpectedURL
                fullInvoiceWorksheet.Cell(16, 4).Value = "Success - Thanh toán tiền mặt thành công"; // ExpectedResult
                fullInvoiceWorksheet.Cell(16, 5).Value = actualUrlPayment; // ActualURL
                fullInvoiceWorksheet.Cell(16, 6).Value = actualPaymentResult; // ActualResult
                fullInvoiceWorksheet.Cell(16, 7).Value = (actualPaymentResult == "Success - Thanh toán tiền mặt thành công") ? "Pass" : "Fail"; // Pass/Fail
            }
            // Lưu file Excel
            workbook.Save();
        }
        private string GetAlertMessage()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                var alert = wait.Until(d => d.FindElement(By.Id("statusAlert")));
                return alert.Text.Trim().Replace("\n×", "");
            }
            catch
            {
                return "No message found";
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
