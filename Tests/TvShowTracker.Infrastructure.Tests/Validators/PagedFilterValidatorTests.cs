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
    public class PagedFilterValidatorTests
    {
        private PagedFilterValidator _underTest;
        [SetUp]
        public void SetUp()
        {
            _underTest = CreatePagedFilterValidator();
        }

        private PagedFilterValidator CreatePagedFilterValidator()
        {
            return new PagedFilterValidator();
        }

        [TestCase(-1,20, TestName = "When page is negative")]
        [TestCase(20,501, TestName = "When pageSize is greater than 500")]
        public async Task Validation_Fails_When_Passing_Invalid_Page_Configuration(int page,int pageSize)
        {
            var filter = new PagedFilter()
            {
                Page = page,
                PageSize = pageSize
            };

           var result = await _underTest.ValidateAsync(filter);
           result.IsValid.Should().BeFalse();
           result.Errors.Should().NotBeNullOrEmpty();
        }

        [TestCase(1, 499)]
        [TestCase(20, 100)]
        public async Task Validation_Fails_When_Passing_Valid_Page_Configuration(int page, int pageSize)
        {
            var filter = new PagedFilter()
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _underTest.ValidateAsync(filter);
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeNullOrEmpty();
        }
    }
}
