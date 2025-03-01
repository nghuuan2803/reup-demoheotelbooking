using NUnit.Framework;
using OpenQA.Selenium;
using TestProject1.PageObjects;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DemoHotelBooking.Tests
{
	[TestFixture]
	public class RegisterTests
	{
		private IWebDriver driver;
		private string baseUrl = "http://localhost:5145/Account/Register"; // Cập nhật URL theo cấu hình của bạn

		[SetUp]
		public void SetUp()
		{
			// Khởi tạo ChromeDriver (đảm bảo ChromeDriver đã được cài đặt)
			driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
			driver.Navigate().GoToUrl(baseUrl);
		}

		[Test] // Dữ liệu hợp lệ
		public void Register_With_ValidData_Should_Success() 
		{
			// Sử dụng Page Object để thực hiện đăng ký
			var registerPage = new RegisterPage(driver);
			registerPage.Register("0123456789", "Trung Tester", "testuser@example.com", "StrongPass123!");

			// Dùng WebDriverWait đợi chuyển hướng URL chứa "/Room"
			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			wait.Until(d => d.Url.Contains("/Room"));

			// Kiểm tra URL sau khi chuyển hướng
			Assert.AreEqual("http://localhost:5145/Room", driver.Url);
		}

/*		[Test] // Tấn công brute force
		public void Register_With_BruteForce_Should_Be_Limited()
		{
			var registerPage = new RegisterPage(driver);

			for (int i = 0; i < 5; i++) // Giả sử hệ thống giới hạn sau 5 lần thất bại
			{
				registerPage.Register("0123456789", "Trung Tester", "testuser@example.com", "WrongPass123!");
			}

			string emailError = registerPage.SubmitButton.GetAttribute("validationMessage");

			// In ra console để kiểm tra thực tế
			Console.WriteLine("Brute force validation message: " + emailError);
			Assert.IsTrue(emailError.Contains("Too many attempts. Please try again later.") ||
						  emailError.Contains("Bạn đã thử quá nhiều lần, vui lòng thử lại sau."));
		}*/

		[Test] // Email có nhiều dấu chấm
		public void Register_With_InvalidEmail_MultipleDots_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("test..user@example.com"); 
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.EmailInput.GetAttribute("data-val-email");
			Assert.IsTrue(errorMessage.Contains("The Email field is not a valid e-mail address."));
		}


		[Test] // Email có khoảng trắng đầu/cuối
		public void Register_With_EmailContainingSpaces_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail(" testuser@example.com "); // Email có khoảng trắng
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.EmailInput.GetAttribute("data-val-email");
			Assert.IsTrue(errorMessage.Contains("The Email field is not a valid e-mail address."));
		}

		[Test] // Email không hợp lệ thiếu '@'
		public void Register_With_InvalidEmail_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("blabla"); // Thiếu '@'
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");

			// Ấn submit để trigger HTML5 validation
			registerPage.SubmitButton.Click();

			// Lấy thông báo lỗi của trình duyệt
			string emailError = registerPage.EmailInput.GetAttribute("validationMessage");

			// In ra console để kiểm tra thực tế
			Console.WriteLine("Email validation message: " + emailError);

			// Kiểm tra thông báo lỗi có chứa tiếng Anh hoặc tiếng Việt
			Assert.IsTrue(emailError.Contains("Please include an '@'") ||
						  emailError.Contains("Vui lòng bao gồm '@'"));
		}

		[Test]
		public void Register_With_SQLInjection_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("test' OR '1'='1' -- @example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string emailError = registerPage.EmailInput.GetAttribute("validationMessage");
			Console.WriteLine("Email validation message: " + emailError);

			Assert.IsTrue(emailError.Contains("A part followed by '@'"));  
		}

		[Test] // Email có tên miền lạ
		public void Register_With_ValidButRareDomain_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.Register("0123456789", "Trung Tester", "testuser@customdomain.hahaabcd", "StrongPass123!");

			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			wait.Until(d => d.Url.Contains("/Room"));

			Assert.AreEqual("http://localhost:5145/Room", driver.Url);
		}

		[Test] // Email đã tồn tại
		public void Register_With_Existing_Email_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.Register("0123456789", "Trung Tester", "admin@example.com", "StrongPass123!");

			string errorMessage = registerPage.GetErrorMessageEmail();
			Assert.IsTrue(errorMessage.Contains("Email đã tồn tại")); 
		}

		[Test] // Không nhập Email
		public void Register_WithoutEmail_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageEmail();
			Assert.IsTrue(errorMessage.Contains("The Email field is required."));
		}


		[Test] // Số điện thoại có ký tự đặc biệt
		public void Register_With_InvalidPhoneNumber_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123-456-789"); // Giả sử chỉ chấp nhận số
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePhoneNumber();
			Assert.IsTrue(errorMessage.Contains("The Số điện thoại field is not a valid phone number."));
		}

		[Test] // Số điện thoại quá dài
		public void Register_With_TooLongPhoneNumber_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("01234567890123456789"); // Giả sử số điện thoại chỉ cho phép tối đa 10-15 ký tự
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePhoneNumber();
			Assert.IsTrue(errorMessage.Contains("The Số điện thoại field is not a valid phone number."));
		}

		[Test] // Không nhập số điện thoại
		public void Register_WithoutPhoneNumber_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePhoneNumber();
			Assert.IsTrue(errorMessage.Contains("The Số điện thoại field is required."));
		}

		[Test] // Số điện thoại đã tồn tại
		public void Register_With_Existing_PhoneNumber_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePhoneNumber();
			Assert.IsTrue(errorMessage.Contains("The Số điện thoại field is required."));
		}

		[Test]
public void Register_With_XSS_Injection_Should_Fail()
{
    var registerPage = new RegisterPage(driver);
    registerPage.EnterPhoneNumber("0123456789");
    registerPage.EnterFullName("<script>alert('XSS')</script>");
    registerPage.EnterEmail("testuser@example.com");
    registerPage.EnterPassword("StrongPass123!");
    registerPage.EnterConfirmPassword("StrongPass123!");

    // Đợi nút Submit sẵn sàng
    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(registerPage.SubmitButton));

    // Nhấp bằng JavaScript nếu cần
    try 
    {
        registerPage.SubmitButton.Click();
    }
    catch (ElementClickInterceptedException)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", registerPage.SubmitButton);
    }

    string fullnameError = registerPage.FullNameInput.GetAttribute("validationMessage");
    Console.WriteLine("Fullname validation message: " + fullnameError);
    Assert.IsTrue(fullnameError.Contains("The Họ và tên field is required.") || fullnameError.Contains("Invalid input"));
}

		[Test] // Họ và tên quá dài
		public void Register_With_TooLongFullName_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			string longName = new string('A', 300); 
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName(longName);
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageFullname();
			Assert.IsTrue(errorMessage.Contains("The Họ và tên field must be at most 255 characters."));
		}

		[Test] // Nhập tên chỉ có khoảng trắng
		public void Register_With_EmptyFullName_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("    "); // Chỉ nhập khoảng trắng
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageFullname();
			Assert.IsTrue(errorMessage.Contains("The Họ và tên field is required."));
		}

		[Test] // Không nhập họ và tên 
		public void Register_WithoutFullName_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageFullname();
			Console.WriteLine("Validation message: " + errorMessage);
			Assert.IsTrue(!string.IsNullOrEmpty(errorMessage), "Không tìm thấy thông báo lỗi!");		}

		[Test] // Không nhập Password
		public void Register_WithoutPassword_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterConfirmPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePassword();
			Assert.IsTrue(errorMessage.Contains("The Password field is required."));
		}

		[Test] // Mật khẩu không khớp
		public void Register_With_Mismatched_Passwords_Should_Fail() 
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.EnterConfirmPassword("WrongPass123!"); 
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageConfirmPassword();
			Assert.IsTrue(errorMessage.Contains("The password and confirmation password do not match."));
		}

		[Test] // Mật khẩu quá ngắn
		public void Register_With_TooShortPassword_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("lol@example.com");
			registerPage.EnterPassword("1"); 
			registerPage.EnterConfirmPassword("1");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessagePassword();
			Assert.IsTrue(errorMessage.Contains("The Password must be at least 8 characters long."));
		}

		[Test] // Không nhập Confirm Password
		public void Register_WithoutConfirmPassword_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.EnterPhoneNumber("0123456789");
			registerPage.EnterFullName("Trung Tester");
			registerPage.EnterEmail("testuser@example.com");
			registerPage.EnterPassword("StrongPass123!");
			registerPage.SubmitButton.Click();

			string errorMessage = registerPage.GetErrorMessageConfirmPassword();
			Assert.IsTrue(errorMessage.Contains("The Confirm password field is required."));
		}

		[Test] // Không nhập gì cả
		public void Register_WithoutAnyFields_Should_Fail()
		{
			var registerPage = new RegisterPage(driver);
			registerPage.SubmitButton.Click(); 

			string phoneError = registerPage.GetErrorMessagePhoneNumber();
			string fullNameError = registerPage.GetErrorMessageFullname();
			string emailError = registerPage.GetErrorMessageEmail();
			string passwordError = registerPage.GetErrorMessagePassword();
			string confirmPasswordError = registerPage.GetErrorMessageConfirmPassword();

			Assert.IsTrue(phoneError.Contains("The Số điện thoại field is required."));
			Assert.IsTrue(fullNameError.Contains("The Họ và tên field is required."));
			Assert.IsTrue(emailError.Contains("The Email field is required."));
			Assert.IsTrue(passwordError.Contains("The Password field is required."));
			Assert.IsTrue(confirmPasswordError.Contains("The Confirm password field is required."));
		}

		[TearDown]
		public void TearDown()
		{
			driver.Quit();
		}
	}
}
