using API.Models.Requests;
using API.Models.Responses;
using Application.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IRegisterService _registerService;
    
    public AuthController(IRegisterService registerService)
    {
        _registerService = registerService;
    }    
    
    [HttpGet("me")]
    public ActionResult<object> Me()
    {
        return false;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponses<object>>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var input = new RegisterInput(
            Name: request.Name,
            Email: request.Email,
            Password: request.Password,
            ConfirmPassword: request.ConfirmPassword);
    
        await _registerService.RegisterAsync(input, ct);
        return StatusCode(StatusCodes.Status201Created, new ApiResponses<object>
        {
            Success = true,
            Message = "Register success"
        });
    }
}