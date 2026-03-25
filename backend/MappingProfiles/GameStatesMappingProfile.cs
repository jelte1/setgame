using AutoMapper;
using backend.Dtos.GameState;
using backend.Entities;

namespace backend.MappingProfiles;

public class GameStatesMappingProfile : Profile
{
    public GameStatesMappingProfile()
    {
        CreateMap<GetGameStateDto, GameState>().ReverseMap();
    }
}