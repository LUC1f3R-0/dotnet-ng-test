using API.Models.Requests;
using API.Models.Responses;
using API.Register.Models;
using Application.Authentication.Login;
using Application.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IRegisterService _registerService;
    private readonly ILoginService _loginService;

    public AuthController(IRegisterService registerService, ILoginService loginService)
    {
        _registerService = registerService;
        _loginService = loginService;
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

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponses<object>>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var input = new LoginInput(request.Email, request.Password);
        
        var user = await _loginService.LoginAsync(input, ct);
        
        return StatusCode(StatusCodes.Status200OK, new ApiResponses<LoginResult>
        {
            Success = true,
            Message = "Login Success",
            Data = user
        });
    }
}