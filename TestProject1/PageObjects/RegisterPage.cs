using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestProject1.PageObjects;

public class RegisterPage
{
	private readonly IWebDriver driver;
	private readonly WebDriverWait wait;

	// Khởi tạo với driver và thiết lập thời gian đợi tối đa là 10 giây
	public RegisterPage(IWebDriver driver)
	{
		this.driver = driver;
		wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
	}

	// Định nghĩa các phần tử trên trang đăng ký
	public IWebElement PhoneNumberInput => wait.Until(d => d.FindElement(By.Id("phoneNum")));
	public IWebElement FullNameInput => wait.Until(d => d.FindElement(By.Id("fullName")));
	public IWebElement EmailInput => wait.Until(d => d.FindElement(By.Id("email")));
	public IWebElement PasswordInput => wait.Until(d => d.FindElement(By.Id("password")));
	public IWebElement ConfirmPasswordInput => wait.Until(d => d.FindElement(By.Id("confirmPass")));
	public IWebElement SubmitButton => wait.Until(d => d.FindElement(By.Id("submit-Register")));

	// Các phương thức nhập dữ liệu vào form
	public void EnterPhoneNumber(string phone)
	{
		PhoneNumberInput.Clear();
		PhoneNumberInput.SendKeys(phone);
	}

	public void EnterFullName(string fullName)
	{
		FullNameInput.Clear();
		FullNameInput.SendKeys(fullName);
	}

	public void EnterEmail(string email)
	{
		EmailInput.Clear();
		EmailInput.SendKeys(email);
	}

	public void EnterPassword(string password)
	{
		PasswordInput.Clear();
		PasswordInput.SendKeys(password);
	}

	public void EnterConfirmPassword(string confirmPassword)
	{
		ConfirmPasswordInput.Clear();
		ConfirmPasswordInput.SendKeys(confirmPassword);
	}

	// Phương thức thực hiện đăng ký hoàn chỉnh
	public void Register(string phone, string fullName, string email, string password)
	{
		EnterPhoneNumber(phone);
		EnterFullName(fullName);
		EnterEmail(email);
		EnterPassword(password);
		EnterConfirmPassword(password);
		SubmitButton.Click();
	}
	public string GetErrorMessagePhoneNumber()
	{
		try
		{
			var errorElement = wait.Until(d => d.FindElement(By.CssSelector("span.text-danger[data-valmsg-for='PhoneNumber']")));
			return errorElement.Text;
		}
		catch (NoSuchElementException)
		{
			return string.Empty;
		}
	}
	public string GetErrorMessageFullname()
	{
		try
		{
			var errorElement = wait.Until(d => d.FindElement(By.CssSelector("span.text-danger[data-valmsg-for='FullName']")));
			return errorElement.Text;
		}
		catch (NoSuchElementException)
		{
			return string.Empty;
		}
	}
	public string GetErrorMessageEmail()
	{
		try
		{
			var errorElement = wait.Until(d => d.FindElement(By.CssSelector("span.text-danger[data-valmsg-for='Email']")));
			return errorElement.Text;
		}
		catch (NoSuchElementException)
		{
			return string.Empty;
		}
	}
	public string GetErrorMessagePassword()
	{
		try
		{
			var errorElement = wait.Until(d => d.FindElement(By.CssSelector("span.text-danger[data-valmsg-for='Password']")));
			return errorElement.Text;
		}
		catch (NoSuchElementException)
		{
			return string.Empty;
		}
	}
	public string GetErrorMessageConfirmPassword()
	{
		try
		{
			var errorElement = wait.Until(d => d.FindElement(By.CssSelector("span.text-danger[data-valmsg-for='ConfirmPassword']")));
			return errorElement.Text;
		}
		catch (NoSuchElementException)
		{
			return string.Empty;
		}
	}
}
