using BugTracker.Business.Mapers;
using BugTracker.Data.Models;

namespace BugTracker.Business.Tests.Mappers
{
    [TestClass]
    public class UserMapperTests
    {
        [TestMethod]
        public void ToDto_ShouldMapCorrectly()
        {
            var user = new User
            {
                Id = "id1",
                UserName = "john"
            };

            var dto = UserMapper.ToDto(user);

            Assert.AreEqual("id1", dto.Id);
            Assert.AreEqual("john", dto.UserName);
        }
    }
}
