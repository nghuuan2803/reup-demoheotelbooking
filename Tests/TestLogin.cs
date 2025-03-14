﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using ClosedXML.Excel;


namespace Tests
{
    [TestFixture]
    public class TestLogin
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5145";
        IWebElement element;
        XLWorkbook workbook;
        IXLWorksheet worksheet;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl($"{baseUrl}/account/login");
            Thread.Sleep(2000);

            workbook = new XLWorkbook("../../../Data/Login_data.xlsx");
            worksheet = workbook.Worksheet(1);

        }
        [TearDown]
        public void TearDown()
        {
            workbook.Save();
            workbook.Dispose();
            driver.Quit();
            driver.Dispose();
        }

        private void Login(string username, string password, bool rememberMe = false)
        {
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            // Nhập username và password
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Nếu chọn "Remember Me"
            if (rememberMe)
            {
                driver.FindElement(By.Name("RememberMe")).Click();
            }

            // Nhấn nút "Login"
            driver.FindElement(By.Id("submit-login")).Click();
        }

        [Test]
        public void Test_Login_Success()
        {
            for (int i = 2; i <= 5; i++)
            {
                var username = worksheet.Cell(i, 1).GetString();
                var password = worksheet.Cell(i, 2).GetString();
                var expected = worksheet.Cell(i, 3).GetString();
                Login(username, password);
                if (driver.Url.Contains("/Room"))
                {
                    worksheet.Cell(i, 5).Value = "pass";
                    driver.Navigate().GoToUrl($"{baseUrl}/account/logout");
                }
                else
                {
                    var error = driver.FindElement(By.Id("message")).Text;
                    worksheet.Cell(i, 4).Value = error;

                    if (expected == error)
                        worksheet.Cell(i, 5).Value = "pass";
                    else
                        worksheet.Cell(i, 5).Value = "fail";
                }
            }
            //lấy dữ liệu excel

            // Kiểm tra xem đã chuyển hướng đến trang chính chưa
            //Assert.IsTrue(driver.Url.Contains("/Room"), "Không chuyển hướng đến trang chính sau khi đăng nhập thành công.");
        }

        [Test]
        public void Test_Login_Failure_WrongPassword()
        {
            Login("admin", "wrongPassword");

            // Kiểm tra thông báo lỗi xuất hiện
            var errorMessage = driver.FindElement(By.ClassName("text-danger")).Text;
            Assert.IsTrue(errorMessage.Contains("Invalid login attempt."), "Thông báo lỗi không hiển thị khi nhập sai mật khẩu.");
        }

        [Test]
        public void Test_Login_Failure_NonExistentUser()
        {
            Login("nonexistentUser", "anyPassword");

            var errorMessage = driver.FindElement(By.ClassName("text-danger")).Text;
            Assert.IsTrue(errorMessage.Contains("Invalid login attempt."), "Thông báo lỗi không hiển thị khi đăng nhập với tài khoản không tồn tại.");
        }

        [Test]
        public void Test_Login_EmptyFields()
        {
            Login("", "");

            var errorMessages = driver.FindElements(By.ClassName("text-danger"));
            Assert.IsTrue(errorMessages.Count > 0, "Không có thông báo lỗi khi để trống tên đăng nhập và mật khẩu.");
        }

        [Test]
        public void Test_Login_RememberMe()
        {
            Login("validUser", "validPassword", rememberMe: true);

            // Kiểm tra xem đã đăng nhập thành công chưa
            Assert.IsTrue(driver.Url.Contains("/Home/Index"), "Không chuyển hướng đến trang chính sau khi đăng nhập thành công.");

            // Kiểm tra cookie "Remember Me"
            var rememberMeCookie = driver.Manage().Cookies.GetCookieNamed(".AspNetCore.Identity.Application");
            Assert.IsNotNull(rememberMeCookie, "Cookie Remember Me không được lưu.");
        }

        [Test]
        public void Test_Login_Redirect_ReturnUrl()
        {
            string returnUrl = "/room";
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login?returnUrl={returnUrl}");

            Login("validUser", "validPassword");

            Assert.IsTrue(driver.Url.Contains(returnUrl), "Không chuyển hướng đúng đến trang được bảo vệ sau khi đăng nhập.");
        }
    }
}
