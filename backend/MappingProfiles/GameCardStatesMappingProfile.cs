using AutoMapper;
using backend.Dtos.GameCardState;
using backend.Entities;

namespace backend.MappingProfiles;

public class GameCardStatesMappingProfile : Profile
{
    public GameCardStatesMappingProfile()
    {
        CreateMap<GetGameCardStateDto, GameCardState>().ReverseMap();
    }
}