/*using DemoHotelBooking.Controllers;
using DemoHotelBooking.Models;
using DemoHotelBooking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium.BiDi.Modules.Session;
using System.Threading.Tasks;

namespace DemoHotelBooking.Tests
{
	[TestFixture]
	public class RegisterUnitTest
	{
		private Mock<UserManager<AppUser>> _userManagerMock;
		private Mock<SignInManager<AppUser>> _signInManagerMock;
		private Mock<RoleManager<IdentityRole>> _roleManagerMock;
		private Mock<AppDbContext> _contextMock;
		private AccountController _controller;

		[SetUp]
		public void Setup()
		{
			var userStoreMock = new Mock<IUserStore<AppUser>>();
			_userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
			_signInManagerMock = new Mock<SignInManager<AppUser>>(
				_userManagerMock.Object,
				Mock.Of<IHttpContextAccessor>(),
				Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
				null, null, null, null);
			_roleManagerMock = new Mock<RoleManager<IdentityRole>>(
				Mock.Of<IRoleStore<IdentityRole>>(),
				null, null, null, null);
			_contextMock = new Mock<AppDbContext>();

			_controller = new AccountController(_contextMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _roleManagerMock.Object);
		}

		[Test]
		public async Task Register_ValidModel_ReturnsRedirectToHome()
		{
			// Arrange
			var model = new RegisterViewModel
			{
				Email = "test@example.com",
				FullName = "Test User",
				Password = "P@ssword123",
				PhoneNumber = "123456789"
			};

			var user = new AppUser
			{
				UserName = model.Email,
				Email = model.Email,
				FullName = model.FullName,
				PhoneNumber = model.PhoneNumber
			};

			_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), model.Password))
				.ReturnsAsync(IdentityResult.Success);
			_roleManagerMock.Setup(x => x.RoleExistsAsync("Customer")).ReturnsAsync(true);
			_userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Customer")).ReturnsAsync(IdentityResult.Success);
			_signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<AppUser>(), false, null)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.Register(model) as RedirectToActionResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.AreEqual("Home", result.ControllerName);
		}

		[Test]
		public async Task Register_InvalidModel_ReturnsViewWithErrors()
		{
			// Arrange
			var model = new RegisterViewModel
			{
				Email = "invalid-email",
				FullName = "Test User",
				Password = "short",
				PhoneNumber = "123456789"
			};

			_controller.ModelState.AddModelError("Email", "Invalid email format");

			// Act
			var result = await _controller.Register(model) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(model, result.Model);
		}

		[Test]
		public async Task Register_FailedUserCreation_ReturnsViewWithErrors()
		{
			// Arrange
			var model = new RegisterViewModel
			{
				Email = "test@example.com",
				FullName = "Test User",
				Password = "P@ssword123",
				PhoneNumber = "123456789"
			};

			_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), model.Password))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

			// Act
			var result = await _controller.Register(model) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(model, result.Model);
			Assert.IsTrue(_controller.ModelState.ContainsKey(string.Empty));
		}
	}
}
*/