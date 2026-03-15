using AutoMapper;
using backend.Dtos.Card;
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