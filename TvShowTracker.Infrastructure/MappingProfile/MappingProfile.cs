using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Infrastructure.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<ActorDto, Actor>();
            CreateMap<Actor, ActorDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
            CreateMap<Episode,EpisodeDto>();
            CreateMap<EpisodeDto, Episode>();
            CreateMap<TvShow, TvShowDto>();
            CreateMap<TvShowDto, TvShow>();
        }
    }
}
