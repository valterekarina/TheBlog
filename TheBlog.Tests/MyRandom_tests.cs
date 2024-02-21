using TheBlogg.Services;

namespace TheBlog.Tests
{
    [TestClass]
    public class MyRandom_tests
    {
        [TestMethod]
        public void GivenBoundries_WhenGetRadom_ThenIntInBoundries()
        {
            //Arrange
            IMyRandom random = new MyRandom();

            //Act
            var randomInt = random.GetRandom(10);

            //Assert
            Assert.IsTrue(randomInt >= 0 && randomInt < 10);
        }
    }
}
