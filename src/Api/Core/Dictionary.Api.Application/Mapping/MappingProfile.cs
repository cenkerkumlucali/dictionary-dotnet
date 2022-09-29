using AutoMapper;
using Common.Models.Queries;
using Common.Models.Queries.Entry;
using Common.Models.RequestModels;
using Common.Models.RequestModels.Entry;
using Common.Models.RequestModels.EntryComment;
using Common.Models.RequestModels.User;
using Dictionary.Api.Domain.Models;

namespace Dictionary.Api.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginUserViewModel>().ReverseMap();

        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, UpdateUserCommand>().ReverseMap();

        CreateMap<Entry, CreateEntryCommand>().ReverseMap();
        CreateMap<EntryComment, CreateEntryCommentCommand>().ReverseMap();

        CreateMap<Entry, GetEntriesViewModel>()
            .ForMember(c => c.CommentCount, opt 
                => opt.MapFrom(c => c.EntryComments.Count)).ReverseMap();
    }
}