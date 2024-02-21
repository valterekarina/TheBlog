using Moq;
using TheBlogg.Services;

namespace TheBlog.Tests
{
    [TestClass]
    public class GenerateRandomPassword_tests
    {
        private readonly Mock<IMyRandom> _randomMock;

        public GenerateRandomPassword_tests()
        {
            _randomMock = new Mock<IMyRandom>();
            _randomMock.Setup(r => r.GetRandom(66)).Returns(1);
        }

        [TestMethod]
        public void WhenGetRandomPassword_ThenGetPasswordLength12()
        {
            //Arrange
            IGenerateRandomPassword generateRandomPassword = new GenerateRandomPassword(_randomMock.Object);

            //Act
            var randomPassword = generateRandomPassword.RandomPasswordGenerator();

            //Assert
            Assert.AreEqual(12, randomPassword.Length);
            Assert.IsFalse(randomPassword.Contains("b"));
            Assert.AreEqual("BBBBBBBBBBBB", randomPassword);
        }
    }
}
