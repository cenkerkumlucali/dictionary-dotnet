using Common.Models.RequestModels.User;
using Dictionary.Api.Application.Features.Commands.User.ConfirmEmail;
using Dictionary.Api.Application.Features.Queries.GetUserDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.Api.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController:BaseController
{
    private IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await _mediator.Send(new GetUserDetailQuery(id));

        return Ok(user);
    }


    [HttpGet]
    [Route("UserName/{userName}")]
    public async Task<IActionResult> GetByUserName(string userName)
    {
        var user = await _mediator.Send(new GetUserDetailQuery(Guid.Empty,userName));

        return Ok(user);
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
    {
        var result = await _mediator.Send(loginUserCommand);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        return Ok(result);
    }
    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand updateUserCommand)
    {
        var result = await _mediator.Send(updateUserCommand);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("Confirm")]
    public async Task<IActionResult> ConfirmEmail(Guid id)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand{ConfirmationId = id});
        return Ok(result);
    }
    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand changeUserPasswordCommand)
    {
        if (!changeUserPasswordCommand.UserId.HasValue)
            changeUserPasswordCommand.UserId = UserId;
        var result = await _mediator.Send(changeUserPasswordCommand);
        return Ok(result);
    }
}