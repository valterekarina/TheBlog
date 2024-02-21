using Microsoft.EntityFrameworkCore;
using TheBlogg.Data;
using TheBlogg.Models;


namespace TheBlog.Tests
{
    [TestClass]
    public class UserRepository_tests
    {
        [TestMethod]
        public void GivenUser_WhenCreate_ThenUserCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userToAdd = new User
                {
                    Name = "test name",
                    Email = "test email",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = false,
                    CanRank = false
                };

                // Act
                var createdUser = userRepository.Create(userToAdd);

                // Assert
                Assert.IsNotNull(createdUser);
                Assert.AreNotEqual(0, createdUser.Id);
                Assert.AreEqual(userToAdd.Name, createdUser.Name);
                Assert.IsTrue(BCrypt.Net.BCrypt.Verify("password", createdUser.PasswordHash));
            }
        }

        [TestMethod]
        public void GivenUser_WhenDelete_ThenUserDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userToAdd = new User
                {
                    Id = 2,
                    Name = "test name",
                    Email = "test email",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = false,
                    CanRank = false
                };

                // Act
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();

                var count = dbContext.Users.Count();
                var userToDelete = dbContext.Users.FirstOrDefault(u => u.Name == "test name");

                //Assert
                Assert.IsNotNull(userToDelete);

                //Act
                var deletedUser = userRepository.Delete(userToDelete);
                var newCount = dbContext.Users.Count();

                // Assert
                Assert.IsNotNull(deletedUser);
                Assert.AreNotEqual(count, newCount);
            }
        }

        [TestMethod]
        public void GivenUser_WhenGetByEmail_ThenUserFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userToAdd = new User
                {
                    Id = 3,
                    Name = "test name",
                    Email = "email@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = true,
                    CanComment = false,
                    CanRank = false
                };

                // Act
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();

                var foundUser = userRepository.GetByEmail("email@email.com");

                // Assert
                Assert.IsNotNull(foundUser);
                Assert.IsTrue(foundUser.CanCreateArticle);
            }
        }

        [TestMethod]
        public void GivenUser_WhenGetById_ThenUserFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userToAdd = new User
                {
                    Id = 4,
                    Name = "test name",
                    Email = "email@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = true,
                    CanRank = false
                };

                // Act
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();

                var foundUser4 = userRepository.GetById(4);
                var foundUser3 = userRepository.GetById(3);
                var foundUser5 = userRepository.GetById(5);

                // Assert
                Assert.IsNotNull(foundUser4);
                Assert.IsNotNull(foundUser3);
                Assert.IsNull(foundUser5);
                Assert.IsTrue(foundUser4.CanComment);
                Assert.IsFalse(foundUser3.CanComment);
            }
        }

        [TestMethod]
        public void GivenUser_WhenUpdate_ThenUserUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TheBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var dbContext = new TheBlogDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userToAdd = new User
                {
                    Id = 5,
                    Name = "test name",
                    Email = "email@email.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = true,
                    CanRank = false
                };

                // Act
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();

                var userToUpdate = dbContext.Users.FirstOrDefault(u => u.Id == 5);

                //Assert
                Assert.IsNotNull(userToUpdate);

                //Act
                userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword("newPassword");
                userToUpdate.Role = "admin";

                var updatedUser = userRepository.Update(userToUpdate);

                // Assert
                Assert.IsFalse(BCrypt.Net.BCrypt.Verify("password", updatedUser.PasswordHash));
                Assert.IsTrue(BCrypt.Net.BCrypt.Verify("newPassword", updatedUser.PasswordHash));
                Assert.AreEqual(updatedUser.Role, "admin");
            }
        }
    }
}
