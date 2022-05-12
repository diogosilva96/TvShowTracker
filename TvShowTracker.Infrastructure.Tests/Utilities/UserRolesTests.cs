using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TvShowTracker.Infrastructure.Utilities;

namespace TvShowTracker.Infrastructure.Tests.Utilities
{
    [TestFixture]
    public class UserRolesTests
    {

        [Test]
        public void Role_User_String_Should_Be_User()
        {
            UserRoles.User.Should().Be("User");
        }

        [Test]
        public void Role_Administrator_String_Should_Be_Administrator()
        {
            UserRoles.Administrator.Should().Be("Administrator");
        }
    }
}
