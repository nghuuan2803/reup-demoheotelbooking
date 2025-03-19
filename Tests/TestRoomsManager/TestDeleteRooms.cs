using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static OpenQA.Selenium.BiDi.Modules.Script.EvaluateResult;

namespace SeleniumTest;


[TestFixture]
public class TestDeleteRooms
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
        worksheet = workbook.Worksheet("DeleteRooms");

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
    public void DeleteRooms()
    {
        Login("admin", "admin");
        driver.Navigate().GoToUrl($"http://localhost:5145/Admin");

        for (int i = 3; i <= 3; i++)
        {
            var id = worksheet.Cell(i, 2).GetString();
            var expected = worksheet.Cell(i, 3).GetString();

            // Tạo WebDriverWait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            try
            {
                // Tạo WebDriverWait

                // Tạo ID của nút Xóa
                string deleteid = $"del-{id}";
                Console.WriteLine($"Đang tìm phần tử với ID: {deleteid}");

                // Kiểm tra xem phần tử có thực sự tồn tại
                try
                {
                    IWebElement deleteButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(deleteid)));
                    Console.WriteLine($"Tìm thấy phần tử: {deleteButton.Displayed}");

                    // Kiểm tra vị trí của phần tử
                    Console.WriteLine($"Vị trí nút Xóa: X = {deleteButton.Location.X}, Y = {deleteButton.Location.Y}");

                    // Cuộn trang đến vị trí của nút Xóa
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", deleteButton);
                    Thread.Sleep(500); // Chờ để đảm bảo nút hiển thị

                    // Click bằng JavaScript để đảm bảo click thành công
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", deleteButton);
                    Console.WriteLine("Đã click vào nút Xóa.");

                    // Xử lý hộp thoại xác nhận nếu có
                    try
                    {
                        IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                        Console.WriteLine("Có hộp thoại xác nhận. Đang bấm OK...");
                        alert.Accept(); // Bấm OK
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Không có hộp thoại xác nhận.");
                    }

                    // Chờ kiểm tra xem phòng đã bị xóa chưa
                    Thread.Sleep(2000); // Chờ thêm để đảm bảo trang cập nhật





                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"Không tìm thấy phần tử có ID: {deleteid}");
                    worksheet.Cell(i, 4).Value = "Xóa phòng thành công";
                    worksheet.Cell(i, 5).Value = "Fail";
                }

            }
            catch
            {
                bool isSuccess = wait.Until(d => !d.PageSource.Contains(id));
                worksheet.Cell(i, 4).Value = $"Xóa phòng thành công";
                worksheet.Cell(i, 5).Value = isSuccess ? "Pass" : "Fail";
            }

            // Quay lại trang Admin để tiếp tục case tiếp theo
            driver.Navigate().GoToUrl($"{baseUrl}/Admin");
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