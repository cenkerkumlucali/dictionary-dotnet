using AutoMapper;
using Common.Models.Queries;
using Common.Models.RequestModels;
using Dictionary.Api.Domain.Models;

namespace Dictionary.Api.Application.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginUserViewModel>().ReverseMap();
        CreateMap<User, LoginUserCommand>().ReverseMap();
    }
}