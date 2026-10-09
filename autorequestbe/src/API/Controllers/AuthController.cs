using System.Security.Claims;
using API.Authentication;
using API.Models.Requests;
using API.Models.Responses;
using API.Register.Models;
using Application.Authentication.Login;
using Application.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IRegisterService _registerService;
    private readonly ILoginService _loginService;
    private readonly AuthCookieService _authCookieService;

    public AuthController(IRegisterService registerService, ILoginService loginService, AuthCookieService authCookieService)
    {
        _registerService = registerService;
        _loginService = loginService;
        _authCookieService = authCookieService;
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserUuid = User.FindFirstValue("sub"),
            Email = User.FindFirstValue("email"),
            Name = User.FindFirstValue(ClaimTypes.Name),
            Role = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponses<object>>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var input = new LoginInput(request.Email, request.Password);

        // Application layer validates credentials,
        // generates tokens and persists refresh session.
        var result = await _loginService.LoginAsync(input, ct);

        // API layer stores tokens as HttpOnly cookies.
        _authCookieService.SetCookies(
            Response,
            result.AccessToken,
            result.RefreshToken,
            result.AccessTokenExpiresAtUtc,
            result.RefreshTokenExpiresAtUtc
        );

        // Do not return access or refresh tokens in JSON.
        return Ok(new ApiResponses<object>
        {
            Success = true,
            Message = "Login Success",
            Data = new
            {
                result.UserUuid,
                result.Name,
                result.Email,
                result.Status,
                result.IsVerified,
                result.Role
            }
        });
    }
}