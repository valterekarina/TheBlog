using Microsoft.EntityFrameworkCore;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class CommentRepository_tests
    {
        [TestMethod]
        public void addUserAndArticle()
        {
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);
                var userToAdd = new User
                {
                    Id = 99,
                    Name = "test name",
                    Email = "test email",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = false,
                    CanRank = false
                };



                var articleToAdd = new Article
                {
                    Id = 99,
                    Title = "Test Article2",
                    Description = "Test Description",
                    ImageUrl = "image.png",
                    Rating = 0,
                    UserId = 1,
                    User = userRepository.GetById(1),
                };

                //Act
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();

                dbContext.Articles.Add(articleToAdd);
                dbContext.SaveChanges();

                //Assert
                Assert.AreNotEqual(0, dbContext.Users.Count());
                Assert.AreNotEqual(0, dbContext.Articles.Count());
            }
        }

        [TestMethod]
        public void GivenComment_WhenCreate_ThenUserCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var commentRepository = new CommentRepository(dbContext);
                var userRepository = new UserRepository(dbContext);
                var articleRepository = new ArticleRepository(dbContext);
                var dateToAdd = DateTime.Now;

                var commentToAdd = new Comment
                {
                    Text = "New Comment",
                    CreatedAt = dateToAdd,
                    IsReported = false,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = 99,
                    User = userRepository.GetById(99),
                    ArticleId = 99,
                    Article = articleRepository.GetById(99)
                };

                // Act
                var createdComment = commentRepository.Create(commentToAdd);

                // Assert
                Assert.IsNotNull(createdComment);
                Assert.AreNotEqual(0, createdComment.Id);
                Assert.AreNotEqual(dateToAdd, DateTime.Now);
            }
        }


        [TestMethod]
        public void GivenComment_WhenDelete_ThenCommentDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var commentRepository = new CommentRepository(dbContext);
                var dateToAdd = DateTime.Now;

                var commentToAdd = new Comment
                {
                    Id = 2,
                    Text = "New Comment",
                    CreatedAt = dateToAdd,
                    IsReported = false,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = 1,
                    ArticleId = 1
                };

                // Act
                dbContext.Comments.Add(commentToAdd);
                dbContext.SaveChanges();

                var count = dbContext.Comments.Count();

                var commentToDelete = dbContext.Comments.FirstOrDefault(c => c.Id == 2);

                //Assert
                Assert.IsNotNull(commentToDelete);

                //Act

                var deletedComment = commentRepository.Delete(commentToDelete);
                var newCount = dbContext.Comments.Count();

                // Assert
                Assert.IsNotNull(deletedComment);
                Assert.AreEqual(2, deletedComment.Id);
                Assert.AreNotEqual(count, newCount);
            }
        }

        [TestMethod]
        public void GivenComment_WhenGetById_ThenCommentFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var commentRepository = new CommentRepository(dbContext);
                var dateToAdd = DateTime.Now;

                var commentToAdd2 = new Comment
                {
                    Id = 2,
                    Text = "New Comment",
                    CreatedAt = dateToAdd,
                    IsReported = false,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = 1,
                    ArticleId = 1
                };
                var commentToAdd3 = new Comment
                {
                    Id = 3,
                    Text = "New Comment",
                    CreatedAt = dateToAdd,
                    IsReported = true,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = 1,
                    ArticleId = 1
                };

                // Act
                dbContext.Comments.Add(commentToAdd2);
                dbContext.SaveChanges();

                dbContext.Comments.Add(commentToAdd3);
                dbContext.SaveChanges();

                var foundComment2 = commentRepository.GetById(2);
                var foundComment3 = commentRepository.GetById(3);
                var foundComment4 = commentRepository.GetById(4);

                //Assert
                Assert.IsNotNull(foundComment2);
                Assert.IsNotNull(foundComment3);
                Assert.IsNull(foundComment4);
                Assert.IsFalse(foundComment2.IsReported);
                Assert.IsTrue(foundComment3.IsReported);
            }
        }

        [TestMethod]
        public void GivenComment_WhenUpdate_ThenCommentUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var commentRepository = new CommentRepository(dbContext);
                var dateToAdd = DateTime.Now;

                var commentToAdd = new Comment
                {
                    Id = 5,
                    Text = "New Comment",
                    CreatedAt = dateToAdd,
                    IsReported = false,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = 1,
                    ArticleId = 1
                };

                // Act
                dbContext.Comments.Add(commentToAdd);
                dbContext.SaveChanges();

                var count = dbContext.Comments.Count();

                var commentToUpdate = dbContext.Comments.FirstOrDefault(c => c.Id == 5);

                //Assert
                Assert.IsNotNull(commentToUpdate);

                //Act
                commentToUpdate.CreatedAt = DateTime.Now;
                commentToAdd.Text = "updated text";
                var updatedComment = commentRepository.Update(commentToUpdate);
                var newCount = dbContext.Comments.Count();

                //Assert
                Assert.IsNotNull(updatedComment);
                Assert.AreNotEqual(dateToAdd, updatedComment.CreatedAt);
                Assert.AreEqual(count, newCount);
                Assert.AreNotEqual("New Comment", updatedComment.Text);
                Assert.AreEqual("updated text", updatedComment.Text);
            }
        }
    }
}
