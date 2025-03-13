using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000);
        driver.Quit();
    }
}