using AutoMapper;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;

namespace TvShowTracker.Infrastructure.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile(IHashingService hashingService)
    {
        // TODO: the IHashingService injection should probably be on custom value resolver, and not injected directly on the profile
        CreateMap<UserModel, User>().ForMember(destination => destination.Id,
                                             opt => opt.MapFrom(source => source.Id == null
                                                                    ? null
                                                                    : hashingService.Decode(source.Id)));
        CreateMap<User, UserModel>().ForMember(destination => destination.Id,
                                             opt => opt.MapFrom(source => hashingService.Encode(source.Id)))
                                  .ForMember(destination => destination.Password, opt => opt.Ignore());
                                  

        CreateMap<ActorModel, Actor>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => source.Id == null
                                                                      ? null
                                                                      : hashingService.Decode(source.Id)));
        CreateMap<Actor, ActorModel>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<Genre, GenreModel>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<GenreModel, Genre>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => source.Id == null
                                                                      ? null
                                                                      : hashingService.Decode(source.Id)));
        CreateMap<Episode, EpisodeModel>().ForMember(destination => destination.Id,
                                                   opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<EpisodeModel, Episode>().ForMember(destination => destination.Id,
                                                   opt => opt.MapFrom(source => source.Id == null
                                                                          ? null
                                                                          : hashingService.Decode(source.Id)));
        CreateMap<TvShow, TvShowModel>().ForMember(destination => destination.Id,
                                                 opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<TvShowModel, TvShow>().ForMember(destination => destination.Id,
                                                 opt => opt.MapFrom(source => source.Id == null
                                                                        ? null
                                                                        : hashingService.Decode(source.Id)));

        CreateMap<RegisterUserModel, User>().ForMember(destination => destination.Password, opt => opt.MapFrom(source => BCrypt.Net.BCrypt.HashPassword(source.Password)));
    }
}