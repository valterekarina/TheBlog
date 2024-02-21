using Microsoft.EntityFrameworkCore;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlog.Tests
{
    [TestClass]
    public class ArticleRepository_tests
    {
        [TestMethod]
        public void GivenArticle_WhenCreate_ThenArticleCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var articleRepository = new ArticleRepository(dbContext);
                var userRepository = new UserRepository(dbContext);

                var articleToAdd = new Article
                {

                    Title = "Test Article",
                    Description = "Test Description",

                    ImageUrl = "image.png",
                    Rating = 0,
                    UserId = 1,
                    User = userRepository.GetById(1),
                };

                // Act
                var createdArticle = articleRepository.Create(articleToAdd);

                // Assert
                Assert.IsNotNull(createdArticle);
                Assert.AreNotEqual(0, createdArticle.Id);
                Assert.AreEqual(articleToAdd.Title, createdArticle.Title);
            }
        }

        [TestMethod]
        public void GivenArticles_WhenUpdate_ThenArticleUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var existingTitle = "Existing Article";
            var existingDescription = "Existing Description";

            using (var dbContext = new TheBlogDbContext(options))
            {
                var articleRepository = new ArticleRepository(dbContext);
                var userRepository = new UserRepository(dbContext);
                var existingArticle = new Article
                {
                    Title = existingTitle,
                    Description = existingDescription,

                    ImageUrl = "image.png",
                    Rating = 0,
                    UserId = 1,
                    User = userRepository.GetById(1),
                };

                // Act
                dbContext.Articles.Add(existingArticle);
                dbContext.SaveChanges();
                var count = dbContext.Articles.Count();

                //Assert
                Assert.IsNotNull(count);

                //Act

                var articleToUpdate = dbContext.Articles.FirstOrDefault(a => a.Title == existingTitle);
                articleToUpdate.Title = "updated Article";

                articleRepository.Update(articleToUpdate);
                var updatedArticle = dbContext.Articles.FirstOrDefault(a => a.Description == existingDescription);
                var newCount = dbContext.Articles.Count();

                //Assert

                Assert.IsNotNull(articleToUpdate);
                Assert.AreEqual(count, newCount);
                Assert.AreEqual("updated Article", updatedArticle.Title);
            }
        }

        [TestMethod]
        public void GivenArticles_WhenDelete_ThenArticleDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var articleRepository = new ArticleRepository(dbContext);
                var userRepository = new UserRepository(dbContext);

                var articleToAdd = new Article
                {

                    Title = "Test Article",
                    Description = "Test Description",

                    ImageUrl = "image.png",
                    Rating = 0,
                    UserId = 1,
                    User = userRepository.GetById(1),
                };

                // Act
                dbContext.Articles.Add(articleToAdd);
                dbContext.SaveChanges();
                var count = dbContext.Articles.Count();

                // Assert
                Assert.IsNotNull(count);

                //Act
                var deletedArticle = articleRepository.Delete(dbContext.Articles.FirstOrDefault(a => a.Title == articleToAdd.Title));
                var newCount = dbContext.Articles.Count();

                //Assert

                Assert.IsNotNull(deletedArticle);
                Assert.AreNotEqual(count, newCount);
                Assert.AreEqual(articleToAdd.Title, deletedArticle.Title);
            }
        }

        [TestMethod]
        public void GivenArticle_WhenGetById_ThenGetArticle()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var articleRepository = new ArticleRepository(dbContext);
                var userRepository = new UserRepository(dbContext);

                var articleToAdd = new Article
                {
                    Id = 10,
                    Title = "Test Article2",
                    Description = "Test Description",
                    ImageUrl = "image.png",
                    Rating = 0,
                    UserId = 1,
                    User = userRepository.GetById(1),
                };

                // Act
                dbContext.Articles.Add(articleToAdd);
                dbContext.SaveChanges();

                var testArticle = articleRepository.GetById(10);

                //Assert
                Assert.IsNotNull(testArticle);
                Assert.AreEqual("Test Article2", testArticle.Title);
            }
        }
    }
}
