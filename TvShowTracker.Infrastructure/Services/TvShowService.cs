using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;
using TvShowTracker.Infrastructure.Validators;

namespace TvShowTracker.Infrastructure.Services
{
    public class TvShowService : ITvShowService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<PagedFilter> _pagedFilterValidator;
        private readonly ILogger<TvShowTrackerDbContext> _logger;

        public TvShowService(TvShowTrackerDbContext context, IMapper mapper, IValidator<PagedFilter> pagedFilterValidator,
                             ILogger<TvShowTrackerDbContext> logger)
        {
            _context = context;
            _mapper = mapper;
            _pagedFilterValidator = pagedFilterValidator;
            _logger = logger;
        }
        public async Task<Result<IEnumerable<TvShowModel>>> GetAllAsync(GetTvShowsFilter filter)
        {
            try
            {
                var validationResult = await _pagedFilterValidator.ValidateAsync(filter);
                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<IEnumerable<TvShowModel>>(validationResult);
                }

                Expression<Func<TvShow, bool>> filterExpression = show =>
                    (string.IsNullOrEmpty(filter.Name) || show.Name.ToLower() == filter.Name.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Country) || show.Country.ToLower() == filter.Country.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Network) || show.Network.ToLower() == filter.Network.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Status) || show.Status.ToLower() == filter.Status.ToLower()) &&
                    (!filter.EndDate.HasValue || show.EndDate == filter.EndDate) &&
                    (!filter.StartDate.HasValue || show.StartDate == filter.StartDate);

                List<TvShow>? shows = null;
                if (filter.Page is not null && filter.PageSize is not null)
                {
                    shows = await _context.Shows.Where(filterExpression)
                                                                .Skip(filter.Page.Value * filter.PageSize.Value)
                                                                .Take(filter.PageSize.Value)
                                                                .ToListAsync();
                }
                if (filter.Page is null && filter.PageSize is null)
                {
                    shows = await _context.Shows.Where(filterExpression).ToListAsync();
                }
                var showModels = shows is not null && shows.Any() ? 
                    shows.Select(s => _mapper.Map<TvShowModel>(s)) : 
                    Enumerable.Empty<TvShowModel>();

                return ResultHelper.ToSuccessResult(showModels);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}",nameof(GetAllAsync), ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<TvShowModel>>(new List<string>() { ex.ToString() });
            }
        }
    }
}
