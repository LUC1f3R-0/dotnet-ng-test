using API.Models.Requests;
using API.Models.Responses;
using Application.Authentication.Register;
using Application.Authentication.Register.Result;
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
            Name: request.Name?.Trim(),
            Email: request.Email.Trim().ToLowerInvariant(),
            Pass: request.Password,
            ConfirmPass: request.ConfirmPassword
        );

        var user = await _registerService.RegisterAsync(input, ct);
        
        return StatusCode(StatusCodes.Status201Created, new ApiResponses<RegisterResult>
            {
                Success = true,
                Message = "Registered successfully.",
                Data = user
            });
    }
}