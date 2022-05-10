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
        CreateMap<UserDto, User>().ForMember(destination => destination.Id,
                                             opt => opt.MapFrom(source => source.Id == null
                                                                    ? null
                                                                    : hashingService.Decode(source.Id)));
        CreateMap<User, UserDto>().ForMember(destination => destination.Id,
                                             opt => opt.MapFrom(source => hashingService.Encode(source.Id)))
                                  .ForMember( destination => destination.Password, opt => opt.Ignore());

        CreateMap<ActorDto, Actor>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => source.Id == null
                                                                      ? null
                                                                      : hashingService.Decode(source.Id)));
        CreateMap<Actor, ActorDto>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<Genre, GenreDto>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<GenreDto, Genre>().ForMember(destination => destination.Id,
                                               opt => opt.MapFrom(source => source.Id == null
                                                                      ? null
                                                                      : hashingService.Decode(source.Id)));
        CreateMap<Episode, EpisodeDto>().ForMember(destination => destination.Id,
                                                   opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<EpisodeDto, Episode>().ForMember(destination => destination.Id,
                                                   opt => opt.MapFrom(source => source.Id == null
                                                                          ? null
                                                                          : hashingService.Decode(source.Id)));
        CreateMap<TvShow, TvShowDto>().ForMember(destination => destination.Id,
                                                 opt => opt.MapFrom(source => hashingService.Encode(source.Id)));
        CreateMap<TvShowDto, TvShow>().ForMember(destination => destination.Id,
                                                 opt => opt.MapFrom(source => source.Id == null
                                                                        ? null
                                                                        : hashingService.Decode(source.Id)));

        CreateMap<RegisterUserDto,User>().ForMember(destination => destination.Password, opt => opt.MapFrom(source => BCrypt.Net.BCrypt.HashPassword(source.Password)));
    }
}