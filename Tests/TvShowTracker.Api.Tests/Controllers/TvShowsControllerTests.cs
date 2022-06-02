using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TvShowTracker.Api.Controllers;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;

namespace TvShowTracker.Api.Tests.Controllers
{
    [TestFixture]
    public class TvShowsControllerTests
    {
        private ITvShowService _showService;
        private IHashingService _hashingService;
        private IHttpContextAccessor _contextAccessor;
        private ILogger<TvShowsController> _logger;
        private IMemoryCache _memoryCache;
        private TvShowsController _underTest;

        [SetUp]
        public void SetUp()
        {
            _showService = Substitute.For<ITvShowService>();
            _hashingService = Substitute.For<IHashingService>();
            _contextAccessor = Substitute.For<IHttpContextAccessor>();
            _logger = Substitute.For<ILogger<TvShowsController>>();
            _memoryCache = Substitute.For<IMemoryCache>();
            _underTest = new TvShowsController(_showService, _hashingService, _contextAccessor, _logger, _memoryCache);
        }


        //[Test]
        //public async Task GetAllAsync_Returns_TvShowModel()
        //{
        //    var id = "XD";
        //    var returnResult = new Result<IEnumerable<TvShowModel>>(new List<TvShowModel>()
        //    {
        //        new TvShowModel()
        //        {
        //            Id = "XD",
        //            Name = "Show"
        //        },
        //        new TvShowModel()
        //        {
        //            Id = "DX",
        //            Name = "MyShow"
        //        }
        //    });
        //    _showService.GetAllAsync(Arg.Any<GetTvShowsFilter>()).Returns(returnResult);

        //    var result = await _underTest.GetAllAsync(new GetTvShowsFilter());

        //    result.Should().BeOfType<OkObjectResult>();
        //    var objectResult = (OkObjectResult)result;
        //    objectResult.Value.Should().BeOfType<Result<IEnumerable<TvShowModel>>>();
        //    var value = (Result<IEnumerable<TvShowModel>>)objectResult.Value;
        //    value.IsSuccess.Should().BeTrue();
        //    value.Should().BeEquivalentTo(returnResult);

        //}
        //[Test]
        //public async Task GetAllAsync_Returns_Problem()
        //{
        //    var id = "XD";
        //    _showService.GetAllAsync(Arg.Any<GetTvShowsFilter>()).Returns(new Result<IEnumerable<TvShowModel>>(new Exception("Error 1")));

        //    var result = await _underTest.GetAllAsync(new GetTvShowsFilter());

        //    result.Should().BeOfType<ObjectResult>();
        //    var objectResult = (ObjectResult)result;
        //    objectResult.StatusCode.Should().Be(500);
        //    objectResult.Value.Should().BeOfType<ProblemDetails>();
        //    ((ProblemDetails)objectResult.Value).Detail.Should().Be("Error 1");
        //}
        //[Test]
        //public async Task GetByIdAsync_Returns_TvShowDetailsModel()
        //{
        //    var id = "XD";
        //    _hashingService.Decode(Arg.Is<string>(id)).Returns(1);
        //    _showService.GetByIdAsync(1).Returns(new Result<TvShowDetailsModel>(new TvShowDetailsModel()
        //                                         {
        //                                             Id = "XD",
        //                                             Name = "Show"
        //                                         }));

        //    var result = await _underTest.GetByIdAsync(id);

        //    result.Should().BeOfType<OkObjectResult>();
        //    var objectResult = (OkObjectResult)result;
        //    objectResult.Value.Should().BeOfType<Result<TvShowDetailsModel>>();
        //    var value = (Result<TvShowDetailsModel>)objectResult.Value;
        //    value.IsSuccess.Should().BeTrue();
        //}

        //[Test]
        //public async Task GetByIdAsync_Returns_Problem_When_TvShowService_Returns_Error_Result()
        //{
        //    var id = "XD";
        //    _hashingService.Decode(Arg.Is<string>(id)).Returns(1);
        //    _showService.GetByIdAsync(1).Returns(new Result<TvShowDetailsModel>(new Exception("Error")));

        //    var result = await _underTest.GetByIdAsync(id);

        //    result.Should().BeOfType<ObjectResult>();
        //    var objectResult = (ObjectResult)result;
        //    objectResult.StatusCode.Should().Be(500);
        //    objectResult.Value.Should().BeOfType<ProblemDetails>();
        //    ((ProblemDetails)objectResult.Value).Detail.Should().Be("Error");
        //}

        [Test]
        public async Task GetByIdAsync_Returns_NotFound_When_Id_Cannot_Be_Decoded()
        {
            var id = "XD";
            _hashingService.Decode(id).Returns((int?)null); // cannot decode id
       

            var result = await _underTest.GetByIdAsync(id);

            result.Should().BeOfType<NotFoundObjectResult>();
            var objectResult = (NotFoundObjectResult)result;
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().Be("XD");
        }



    }
}
