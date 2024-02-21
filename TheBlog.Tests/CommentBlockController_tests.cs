using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlogg.Controllers;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class CommentBlockController_tests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly CommentBlockController _commentBlockController;

        public CommentBlockController_tests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentBlockController = new CommentBlockController(_commentRepositoryMock.Object);
        }
        [TestMethod]
        public void GivenCommentToBlockInfo_WhenBlockComment_thenCommentBlocked()
        {
            // Arrange
            _commentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Comment { Id = 1, Text = "Test Comment", IsBlocked = false, IsReported = false });

            var validModel = new Comment
            {
                Id = 1,
                IsBlocked = true,
                IsReported = true
            };

            // Act
            var result = _commentBlockController.BlockComment(validModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Comment));
            _commentRepositoryMock.Verify(x => x.Update(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public void GivenCommentToBlockInfoWithNoComments_WhenBlockComment_thenCommentNotBlocked()
        {
            // Arrange
            _commentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Comment)null);

            var validModel = new Comment
            {
                Id = 1,
                IsBlocked = true,
                IsReported = true
            };

            // Act
            var result = _commentBlockController.BlockComment(validModel) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("comment not found", result.Value);
            _commentRepositoryMock.Verify(x => x.Update(It.IsAny<Comment>()), Times.Never);
        }

        [TestMethod]
        public void GivenInvalidModel_WhenBlockComment_ThenCommentNotBlocked()
        {
            // Arrange
            var invalidModel = new Comment();

            _commentBlockController.ModelState.AddModelError("Id", "Id is required.");

            // Act
            var result = _commentBlockController.BlockComment(invalidModel) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("invalid modelstate", result.Value);
            _commentRepositoryMock.Verify(x => x.Update(It.IsAny<Comment>()), Times.Never);
        }
    }
}
