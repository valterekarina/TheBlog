using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlogg.Controllers;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class CommentDeleteController_tests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly CommentDeleteController _controllerMock;

        public CommentDeleteController_tests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _controllerMock = new CommentDeleteController(_commentRepositoryMock.Object);
        }
        [TestMethod]
        public void GivenComment_WhenDelete_ThenCommentDeleted()
        {
            // Arrange
            _commentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Comment { Id = 1, Text = "Test Comment" });

            var validModel = new Comment
            {
                Id = 1
            };

            // Act
            var result = _controllerMock.Delete(validModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("comment deleted", result.Value);
            _commentRepositoryMock.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public void GivenCommentWithNoExistingComments_WhenDelete_ThenNotDeleted()
        {
            // Arrange

            _commentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Comment)null);

            var validModel = new Comment
            {
                Id = 1
            };

            // Act
            var result = _controllerMock.Delete(validModel) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("comment not found", result.Value);
            _commentRepositoryMock.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
        }

        [TestMethod]
        public void GivenInvalidModel_WhenDelete_ThenNotDeleted()
        {
            // Arrange
            var invalidModel = new Comment();

            _controllerMock.ModelState.AddModelError("Id", "Id is required.");

            // Act
            var result = _controllerMock.Delete(invalidModel) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid modelstate", result.Value);
            _commentRepositoryMock.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Never);
        }
    }
}
