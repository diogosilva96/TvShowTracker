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

        [TestCase(null,20, TestName = "When page is null and pageSize has value")]
        [TestCase(20,null, TestName = "When pageSize is null and page has value")]
        public async Task Validation_Fails_When_Passing_Invalid_Page_Configuration(int? page,int? pageSize)
        {
            var filter = new PagedFilter()
            {
                Page = page,
                PageSize = pageSize
            };

           var result = await _underTest.ValidateAsync(filter);
           result.IsValid.Should().BeFalse();
           result.Errors.Should().NotBeNullOrEmpty();
           result.Errors.ToList()
                 .Any(e => e.ErrorMessage.Contains("Page size and page must be either both null or not null."));
        }

        [TestCase(null, null, TestName = "When page is null and pageSize is null")]
        [TestCase(20, 100, TestName = "When pageSize has value and page has value")]
        public async Task Validation_Fails_When_Passing_Valid_Page_Configuration(int? page, int? pageSize)
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
