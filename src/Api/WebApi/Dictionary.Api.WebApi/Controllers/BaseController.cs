using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.Api.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    public Guid? UserId => Guid.NewGuid(); //new(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
}