using Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.Api.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController:ControllerBase
{
    private IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
    {
        var result = await _mediator.Send(loginUserCommand);
        return Ok(result);
    }
}