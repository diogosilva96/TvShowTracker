using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;


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
                    return new Result<IEnumerable<TvShowModel>>(new ValidationException(validationResult.Errors));
                }

                Expression<Func<TvShow, bool>> filterExpression = show =>
                    (string.IsNullOrEmpty(filter.Name) || show.Name.ToLower() == filter.Name.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Country) || show.Country.ToLower() == filter.Country.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Network) || show.Network.ToLower() == filter.Network.ToLower()) &&
                    (string.IsNullOrEmpty(filter.Status) || show.Status.ToLower() == filter.Status.ToLower()) &&
                    (!filter.EndDate.HasValue || show.EndDate == filter.EndDate) &&
                    (!filter.StartDate.HasValue || show.StartDate == filter.StartDate);

                List<TvShow>? shows = null;
                shows = await _context.Shows.Where(filterExpression)
                                      .Skip(filter.Page.Value * filter.PageSize.Value)
                                      .Take(filter.PageSize.Value)
                                      .ToListAsync();
                

                var showModels = shows is not null && shows.Any() ? 
                    shows.Select(s => _mapper.Map<TvShowModel>(s)) : 
                    Enumerable.Empty<TvShowModel>();

                return new Result<IEnumerable<TvShowModel>>(showModels);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}",nameof(GetAllAsync), ex.ToString());
                return new Result<IEnumerable<TvShowModel>>(ex);
            }
        }

        public async Task<Result<TvShowDetailsModel>> GetByIdAsync(int id)
        {
            try
            {
                var show = await _context.Shows
                                         .Include(s => s.Genres)
                                         .Include(s => s.Cast)
                                         .Include(s => s.Episodes)
                                         .FirstOrDefaultAsync(s => s.Id == id);
                if (show == null)
                {
                    return new Result<TvShowDetailsModel>(new ValidationException("Show not found."));
                }

                var showDetails = _mapper.Map<TvShowDetailsModel>(show);
                return new Result<TvShowDetailsModel>(showDetails);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(GetAllAsync), ex.ToString());
                return new Result<TvShowDetailsModel>(ex);
            }
        }
    }
}
