using AutoMapper;
using backend.DTOs.Card;
using backend.DTOs.User;
using backend.Entities;

namespace backend.MappingProfiles;

public class UsersMappingProfile : Profile
{
    public UsersMappingProfile()
    {
        CreateMap<RegisterLoginUserDto, User>().ReverseMap();
        CreateMap<GetUserDto, User>().ReverseMap();
    }
}