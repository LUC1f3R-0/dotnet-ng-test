using API.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpGet("me")]
    public ActionResult<object> Me()
    {
        return true;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Login([FromBody] RegisterRequest request)
    {
        Console.WriteLine("running this");
        return Ok();
    }
}