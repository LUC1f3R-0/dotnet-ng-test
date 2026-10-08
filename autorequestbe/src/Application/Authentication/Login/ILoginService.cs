using API.Register.Models;

namespace Application.Authentication.Login;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(LoginInput input, CancellationToken ct = default);
}
