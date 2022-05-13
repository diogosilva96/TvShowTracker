using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker.Models;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;

namespace TvShowTracker.Background.Worker.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly IEpisodeDateApiService _episodeDateApiService;
        private readonly IMapper _mapper;
        private readonly ILogger<SynchronizationService> _logger;

        public SynchronizationService(TvShowTrackerDbContext context, IEpisodeDateApiService episodeDateApiService, IMapper mapper, ILogger<SynchronizationService> logger)
        {
            _context = context;
            _episodeDateApiService = episodeDateApiService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {

            var currentPage = 1;

            try
            {
                while (true)
                {
                    _logger.LogInformation("Starting synchronization for page {page}...",currentPage);
                    // ideally some retry policies should exist here for the http requests - for e.g, Polly nuget
                    var tvShowResult = await _episodeDateApiService.GetMostPopularShowsAsync(currentPage);
                    if (tvShowResult is null)
                    {
                        _logger.LogWarning("TvShow result is null for page {page}",currentPage);
                        currentPage++;
                        continue;
                    }

                    if (tvShowResult.Page >= tvShowResult.Pages)
                    {
                        _logger.LogInformation("All the shows have been synchronized.");
                        break;
                    }

                    foreach (var show in tvShowResult.Shows)
                    {
                        await SynchronizeShowInformation(show);
                    }
                    _logger.LogInformation("Page {page} synchronization complete.", currentPage);
                    currentPage++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while synchronizing data for page {page}. Error Details: {details}",currentPage, ex.ToString());
            }
        }

        private async Task SynchronizeShowInformation(Show show)
        {
            await TryRunAsync(async () => {
                if (await _context.Shows.AnyAsync(s => s.Name.ToLower() == show.Name.ToLower()))
                {
                    //if show exists don't bother syncing
                    return;
                }

                var showInfo = await _episodeDateApiService.GetTvShowDetails(show.Name.ToLower());
                if (showInfo is null)
                {
                    _logger.LogWarning("Could not get details of show {name}.", show.Name);
                }

                var showData = showInfo?.Show ?? show;
                var dbShow = _mapper.Map<TvShow>(showData);
                _context.Shows.Add(dbShow);
                await _context.SaveChangesAsync();

                await TryRunAsync(async () => await SynchronizeShowEpisodes(showData, dbShow));
                await TryRunAsync(async () => await SynchronizeShowGenres(showData, dbShow));
                await _context.SaveChangesAsync();
            });
        }

        private async Task TryRunAsync(Func<Task> executeAction)
        {
            try
            {
                await executeAction();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while running {action}. Error details: {details}",executeAction.Method.Name, ex.ToString());
            }
        }

        private async Task SynchronizeShowEpisodes(Show show, TvShow dbShow)
        {
            if (show?.Episodes is null || !show.Episodes.Any())
            {
                return;
            }
            foreach (var episode in show.Episodes)
            {
                var episodeExists = await _context.Episodes.AnyAsync(e => e.Show.Name.ToLower() == show.Name &&
                                                                                      e.Number == episode.Episode &&
                                                                                      e.Season == episode.Season);
                if (episodeExists)
                {
                    continue;
                }
                var dbEpisode = _mapper.Map<Episode>(episode);
                dbEpisode.Show = dbShow;
                _context.Episodes.Add(dbEpisode);
            }

            if (!_context.ChangeTracker.HasChanges())
            {
                return;
            }

            await _context.SaveChangesAsync();
        }
        private async Task SynchronizeShowGenres(Show show, TvShow dbShow)
        {
            if (show?.Genres is null || !show.Genres.Any())
            {
                return;
            }
            foreach (var genre in show.Genres)
            {
                Genre dbGenre;
                var genreExists = await _context.Genres.AnyAsync(g => g.Name.ToLower() == genre.ToLower());
                if (!genreExists)
                {
                    dbGenre = new() { Name = genre, Description = genre };
                    _context.Genres.Add(dbGenre);
                    dbShow.Genres.Add(dbGenre);
                    continue;
                }

                dbGenre = await _context.Genres.FirstAsync(g => g.Name == genre);
                dbShow.Genres.Add(dbGenre);
            }
            if (!_context.ChangeTracker.HasChanges())
            {
                return;
            }

            await _context.SaveChangesAsync();
        }
    }
}
