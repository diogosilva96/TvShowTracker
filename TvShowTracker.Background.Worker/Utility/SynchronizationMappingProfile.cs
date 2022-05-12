using AutoMapper;
using TvShowTracker.Background.Worker.Models;
using TvShowTracker.DataAccessLayer.Models;
using TvShow = TvShowTracker.DataAccessLayer.Models.TvShow;

namespace TvShowTracker.Background.Worker.Utility
{
    public class SynchronizationMappingProfile : Profile
    {
        public SynchronizationMappingProfile()
        {
            CreateMap<Show, TvShow>().ForMember(destination => destination.Id, opt => opt.Ignore())
                                     .ForMember(destination => destination.Genres, opt => opt.Ignore());
            CreateMap<ShowEpisode, Episode>().ForMember(destination => destination.Id, opt => opt.Ignore())
                                             .ForMember(destination => destination.Number, opt => opt.MapFrom(source => source.Episode));
        }
    }
}
