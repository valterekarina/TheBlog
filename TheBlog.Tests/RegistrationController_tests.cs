using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlogg.Controllers;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class RegistrationController_tests
    {
        [TestMethod]
        public void GivenValidModel_WhenRegister_ThenRegistered()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var registrationController = new RegistrationController(userRepositoryMock.Object);

            var validModel = new RegistrationModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                PasswordHash = "password123"
            };

            // Act
            var result = registrationController.Register(validModel) as CreatedResult;

            // Assert
            Assert.IsNotNull(result);
            userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void GivenInvalidModel_WhenRegister_ThenNotRegistered()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var registrationController = new RegistrationController(userRepositoryMock.Object);

            var invalidModel = new RegistrationModel
            {
                Name = "John Doe",
                Email = null,
                PasswordHash = "password123"
            };

            registrationController.ModelState.AddModelError("Email", "Email is required.");

            // Act
            var result = registrationController.Register(invalidModel) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("fail", result.Value);
            userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
        }
    }
}
