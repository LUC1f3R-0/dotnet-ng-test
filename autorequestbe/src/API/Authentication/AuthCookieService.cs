namespace API.Authentication;

public sealed class AuthCookieService
{
    public void SetCookies(HttpResponse response, string accessToken, string refreshToken, DateTimeOffset accessExpiresAt, DateTimeOffset refreshExpiresAt)
    {
        response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = accessExpiresAt,
                Path = "/"
            });

        response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpiresAt,
                Path = "/api/auth"
            });
    }

    public void ClearCookies(HttpResponse response)
    {
        response.Cookies.Delete("accessToken", new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });

        response.Cookies.Delete("refreshToken", new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/api/auth"
            });
    }
}