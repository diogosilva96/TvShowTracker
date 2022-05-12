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
    public class RegisterUserModelValidatorTests
    {
        private RegisterUserModelValidator _underTest;

        [SetUp]
        public void SetUp()
        {
            _underTest = CreateRegisterUserModelValidator();
        }

        private RegisterUserModelValidator CreateRegisterUserModelValidator()
        {
            return new RegisterUserModelValidator();
        }


        [TestCase("email", "name", "lastName", "password", true,nameof(UserModel.Email), Description = "When Email is invalid")]
        [TestCase("email@test.com", "name", "lastName", "weakPw", true,nameof(UserModel.Password), Description = "When Password is invalid")]
        [TestCase("email", "name", "l", "password", true, nameof(UserModel.LastName), Description = "When LastName is invalid")]
        [TestCase("email", "f", "lastName", "password", true, nameof(UserModel.FirstName), Description = "When FirstName is invalid")]
        [TestCase("email", "firstName", "lastName", "password", false, "GDPR", Description = "When GDPR consent is not granted")]
        public async Task Validation_Fails_When_At_Least_One_Property_Is_Invalid(string email, string firstName, string lastName, string password, bool gdprConsent, string failingField)
        {
            //TODO: ideally each field should be tested separately, with each different failing case (if more than one exists)
            //very similar to UserModelValidator
            var user = new RegisterUserModel()
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                GrantGdprConsent = gdprConsent
            };

            var validationResult = await _underTest.ValidateAsync(user);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.ToList().Any(e => e.ErrorMessage.Contains(failingField));
        }


        [Test]
        public async Task? Validation_Passes_When_Properties_Are_Valid()
        {
            var user = new RegisterUserModel()
            {
                Email = "elonmusk@hotmail.com",
                Password = "elontesla",
                FirstName = "elon",
                LastName = "musk",
                GrantGdprConsent = true
            };

            var validationResult = await _underTest.ValidateAsync(user);
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }
    }
}
