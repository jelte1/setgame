using AutoMapper;
using backend.DTOs.Card;
using backend.Entities;

namespace backend.MappingProfiles;

public class CardsMappingProfile : Profile
{
    public CardsMappingProfile()
    {
        CreateMap<CreateCardDto, Card>().ReverseMap();
        CreateMap<GetCardDto, Card>().ReverseMap();
    }
}