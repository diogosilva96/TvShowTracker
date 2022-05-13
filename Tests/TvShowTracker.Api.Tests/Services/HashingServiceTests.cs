using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Services;

namespace TvShowTracker.Api.Tests.Services
{
    [TestFixture]
    public class HashingServiceTests
    {

        public HashingService CreateUnderTest(string saltKey)
        {
            var logger = Substitute.For<ILogger<HashingService>>();
            return new HashingService(saltKey, logger);
        }

        [TestCase("a")]
        [TestCase("asdasbdsabdsadasdas")]
        public void Decode_Returns_Null_For_Invalid_Decode_Strings(string invalidString)
        {
            var underTest = CreateUnderTest("SaltKey");
            var result = underTest.Decode(invalidString);

            result.Should().BeNull();

        }

        [TestCase("PkX",420)]
        [TestCase("PW",20)]
        public void Decode_Returns_Number_For_Valid_Decode_Strings(string decodeString, int expectedNumber)
        {
            var underTest = CreateUnderTest("SaltKey");
            var result = underTest.Decode(decodeString);

            result.Should().NotBeNull();
            result.Should().Be(expectedNumber);

        }

        [TestCase(1,"3L")]
        [TestCase(20,"PW")]
        [TestCase(420,"PkX")]
        public void Encode_Returns_String_For_Number(int number,string expectedResult)
        {
            var underTest = CreateUnderTest("SaltKey");

            var result = underTest.Encode(number);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(expectedResult);

        }

        [TestCase(1)]
        [TestCase(20)]
        [TestCase(420)]
        public void Ensure_Decode_Returns_Same_Number_For_Different_SaltKeys(int number)
        {
            var underTest1 = CreateUnderTest("SaltKey");

            var underTest2 = CreateUnderTest("SaltKeySaltKey");

            var result1 = underTest1.Encode(number);
            var result2 = underTest2.Encode(number);

            result1.Should().NotBeNullOrEmpty();
            result2.Should().NotBeNullOrEmpty();

            result1.Should().NotBeEquivalentTo(result2);

            var originalNumber1 = underTest1.Decode(result1);
            var originalNumber2 = underTest2.Decode(result2);

            originalNumber1.Should().Be(number);
            originalNumber2.Should().Be(number);
            originalNumber1.Should().Be(originalNumber2);

        }
    }
}
