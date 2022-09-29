using AutoMapper;
using Common.Models.Queries.User;
using Dictionary.Api.Application.Interfaces.Repositories;
using Dictionary.Api.Domain.Models;
using MediatR;

namespace Dictionary.Api.Application.Features.Queries.GetUserDetail;

public class GetUserDetailQueryHandler:IRequestHandler<GetUserDetailQuery,UserDetailViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserDetailQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDetailViewModel> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
    {
        User user = null;
        if (request.UserId == Guid.Empty)
            user = await _userRepository.GetByIdAsync(request.UserId);
        else if (!string.IsNullOrEmpty(request.UserName))
            user = await _userRepository.GetSingleAsync(c => c.UserName == request.UserName);
        return _mapper.Map<UserDetailViewModel>(user);
    }
}