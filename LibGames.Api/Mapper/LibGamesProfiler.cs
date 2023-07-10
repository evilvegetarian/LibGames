using AutoMapper;
using LibGames.Api.DAL.Entities;
using LibGames.Api.Models;

namespace LibGames.Api.Mapper;

public class LibGamesProfiler : Profile
{
    public LibGamesProfiler()
    {
        CreateMap<GameCreateDto, Game>()
            .ForMember(game => game.YearOfRelease,
                    opt => opt.MapFrom(src => new DateTime(src.YearOfRelease.Year, src.YearOfRelease.Month, src.YearOfRelease.Day)))
            .ReverseMap();
        CreateMap<Game, GameDto>()
            .ForMember(game => game.YearOfRelease,
                       opt => opt.MapFrom(src => new DateOnly(src.YearOfRelease.Year, src.YearOfRelease.Month, src.YearOfRelease.Day)))
            .ReverseMap();
        CreateMap<Game, GameForGenrStudDto>()
            .ForMember(game => game.YearOfRelease,
                       opt => opt.MapFrom(src => new DateOnly(src.YearOfRelease.Year, src.YearOfRelease.Month, src.YearOfRelease.Day)))
            .ReverseMap();


        CreateMap<GenreCreatDto, Genre>().ReverseMap();
        CreateMap<Genre, GenreDto>().ReverseMap();
        CreateMap<Genre, GenreGameDto>().ReverseMap();


        CreateMap<StudioCreatDto, Studio>().ReverseMap();
        CreateMap<Studio, StudioDto>().ReverseMap();
        CreateMap<Studio, StudioGameDto>().ReverseMap();


    }
}
