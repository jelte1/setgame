using AutoMapper;
using backend.Dtos.FoundSet;
using backend.Entities;

namespace backend.MappingProfiles;

public class FoundSetsMappingProfile : Profile
{
    public FoundSetsMappingProfile()
    {
        CreateMap<GetFoundSetDto, FoundSet>().ReverseMap();
    }
}