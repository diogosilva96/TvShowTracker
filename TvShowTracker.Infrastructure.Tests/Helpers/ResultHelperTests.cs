using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using TvShowTracker.Infrastructure.Helpers;

namespace TvShowTracker.Infrastructure.Tests.Helpers
{
    [TestFixture]
    public class ResultHelperTests
    {
        [Test]
        public void ToSuccessResult_StateUnderTest_ExpectedBehavior()
        {
            var data = "myStr";
            var result = ResultHelper.ToSuccessResult(data);
            result.Success.Should().BeTrue();
            result.Data.Should().Be("myStr");
            result.Errors.Should().BeNullOrEmpty();
        }

        [Test]
        public void ToErrorResult_Sets_Success_To_False_And_Appends_Errors_For_String_List_Argument()
        {
            
            var result = ResultHelper.ToErrorResult<string>(new List<string>(){"myError1","myError2"});

            result.Data.Should().BeNullOrEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain("myError1");
            result.Errors.Should().Contain("myError2");
        }

        [Test]
        public void ToErrorResult_Sets_Success_To_False_And_Appends_Errors_For_Validation_Result_Argument()
        {

            var validationResult = new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop1", "error1"),
                new ValidationFailure("prop2", "error2"),
                new ValidationFailure("prop3", "error3")
            });

            var result = ResultHelper.ToErrorResult<string>(validationResult);

            result.Data.Should().BeNullOrEmpty();
            result.Success.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().BeEquivalentTo(new List<string>() { "error1", "error2", "error3" });
        }

        [Test]
        public void ToErrorResult_Throws_Exception_When_Errors_Are_Empty()
        {
            var errorAction = () => ResultHelper.ToErrorResult<string>(new List<string>());

            errorAction.Should().ThrowExactly<ArgumentException>()
                       .Where(e => e.Message.Contains("There are no validation errors."));
        }

        [Test]
        public void ToErrorResult_Throws_Exception_When_Errors_Are_Null()
        {
            List<string> errors = null;
            var errorAction = () => ResultHelper.ToErrorResult<string>(errors);

            errorAction.Should().ThrowExactly<ArgumentException>()
                       .Where(e => e.Message.Contains("There are no validation errors."));
        }

        [Test]
        public void ToErrorResult_Throws_Exception_When_ValidationResult_Contains_No_Errors()
        {
            ValidationResult? validationResult = new ValidationResult();
            var errorAction = () => ResultHelper.ToErrorResult<string>(validationResult);

            errorAction.Should().ThrowExactly<ArgumentException>()
                       .Where(e => e.Message.Contains("There are no validation errors."));
        }

        [Test]
        public void ToErrorResult_Throws_Exception_When_ValidationResult_Is_Null()
        {
            ValidationResult? validationResult = null;
            var errorAction = () => ResultHelper.ToErrorResult<string>(validationResult);

            errorAction.Should().ThrowExactly<ArgumentException>()
                       .Where(e => e.Message.Contains("There are no validation errors."));
        }
    }
}
