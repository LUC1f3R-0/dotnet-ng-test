using System.Security.Claims;
using API.Authentication;
using API.Models.Requests;
using API.Models.Responses;
using API.Register.Models;
using Application.Authentication.Login;
using Application.Authentication.Refresh;
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
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(IRegisterService registerService, ILoginService loginService, AuthCookieService authCookieService, IRefreshTokenService refreshTokenService)
    {
        _registerService = registerService;
        _loginService = loginService;
        _authCookieService = authCookieService;
        _refreshTokenService = refreshTokenService;
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        Console.WriteLine("me controller running");
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

        return StatusCode(StatusCodes.Status200OK, new ApiResponses<object>
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

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        Console.WriteLine("refresh token controller running");
            // Retrieve refresh token from the browser cookie.
            var refreshToken = Request.Cookies["refreshToken"];
    
            // Validate token, retrieve session, rotate refresh token,
            // and generate a new access token.
            var result = await _refreshTokenService.RefreshAsync(refreshToken, ct);
    
            // Missing, expired, revoked, or invalid refresh session.
            if (result is null)
            {
                return Unauthorized(new ApiResponses<object>
                {
                    Success = false,
                    Message = "Refresh token is invalid or expired.",
                    Data = null
                });
            }
    
            // Replace both HttpOnly cookies.
            _authCookieService.SetCookies(
                Response,
                result.AccessToken,
                result.RefreshToken,
                result.AccessTokenExpiresAtUtc,
                result.RefreshTokenExpiresAtUtc
            );
    
            return Ok(new ApiResponses<object>
            {
                Success = true,
                Message = "Session refreshed successfully.",
                Data = null
            });
        }
}