using AutoMapper;
using backend.Dtos.Card;
using backend.Dtos.User;
using backend.Entities;

namespace backend.MappingProfiles;

public class UsersMappingProfile : Profile
{
    public UsersMappingProfile()
    {
        CreateMap<RegisterLoginUserDto, User>().ReverseMap();
        
    }
}