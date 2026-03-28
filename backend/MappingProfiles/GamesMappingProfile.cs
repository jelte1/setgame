using AutoMapper;
using backend.DTOs.Game;
using backend.Entities;

namespace backend.MappingProfiles;

public class GamesMappingProfile : Profile
{
    public GamesMappingProfile()
    {
        CreateMap<CreateGameDto, Game>().ReverseMap();
        CreateMap<GetGameDto, Game>().ReverseMap();
        CreateMap<GetBaseGameDto, Game>().ReverseMap();
    }
}