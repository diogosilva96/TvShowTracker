using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TvShowTracker.Domain.Models;
using TvShowTracker.Infrastructure.Validators;

namespace TvShowTracker.Infrastructure.Tests.Validators
{
    [TestFixture]
    public class UserModelValidatorTests
    {
        private UserModelValidator _underTest;

        [SetUp]
        public void SetUp()
        {
            _underTest = CreateUserModelValidator();
        }

        private UserModelValidator CreateUserModelValidator()
        {
            return new UserModelValidator();
        }

        [TestCase("email","name","lastName","password", nameof(UserModel.Email),Description = "When Email is invalid")]
        [TestCase("email@test.com", "name", "lastName", "weakPw", nameof(UserModel.Password),Description = "When Password is invalid")]
        [TestCase("email", "name", "l", "password", nameof(UserModel.LastName), Description = "When LastName is invalid")]
        [TestCase("email", "f", "lastName", "password", nameof(UserModel.FirstName), Description = "When FirstName is invalid")]
        public async Task Validation_Fails_When_At_Least_One_Property_Is_Invalid(string email, string firstName, string lastName, string password, string failingField)
        {
            //TODO: ideally each field should be tested separately, with each different failing case (if more than one exists)
            var user = new UserModel()
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            var validationResult = await _underTest.ValidateAsync(user);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.ToList().Any(e => e.ErrorMessage.Contains(failingField));
        }

        
       [Test]
        public async Task? Validation_Passes_When_Properties_Are_Valid()
        {
            var user = new UserModel()
            {
                Email = "youcantseeme@hotmail.com",
                Password = "ucantseeme",
                FirstName = "john",
                LastName = "cena"
            };

            var validationResult = await _underTest.ValidateAsync(user);
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }
    }
}
