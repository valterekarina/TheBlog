using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlogg.Controllers;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class CommentCreateController_tests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IArticleRepository> _articleRepositoryMock;
        private readonly CommentCreateController _commentCreateController;

        public CommentCreateController_tests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _articleRepositoryMock = new Mock<IArticleRepository>();

            _commentCreateController = new CommentCreateController(_commentRepositoryMock.Object, _articleRepositoryMock.Object);
        }
        [TestMethod]
        public void GivenCommentExistingArticle_WhenAddComment_ThenCommentAdded()
        {
            _articleRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Article { Id = 1, Title = "Test Article" });
            // Arrange
            var validModel = new Comment
            {
                Text = "This is a test comment",
                UserId = 1,
                ArticleId = 1
            };

            // Act
            var result = _commentCreateController.AddComment(validModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Comment));
            _commentRepositoryMock.Verify(x => x.Create(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public void GivenCommentNotExistingArticle_WhenAddComment_ThenCommentAdded()
        {
            // Arrange
            _articleRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Article)null);
            var validModel = new Comment
            {
                Text = "This is a test comment",
                UserId = 1,
                ArticleId = 1
            };

            // Act
            var result = _commentCreateController.AddComment(validModel) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Article not found", result.Value);
            _commentRepositoryMock.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
        }

        [TestMethod]
        public void GivenInvalidModel_WhenAddComment_ThenCommentAdded()
        {
            // Arrange
            var invalidModel = new Comment();

            _commentCreateController.ModelState.AddModelError("Text", "Text is required.");

            // Act
            var result = _commentCreateController.AddComment(invalidModel) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("invalid modelstate", result.Value);
            _commentRepositoryMock.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
        }
    }
}
