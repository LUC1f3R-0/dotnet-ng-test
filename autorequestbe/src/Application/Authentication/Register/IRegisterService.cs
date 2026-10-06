using Application.Authentication.Register.Result;

namespace Application.Authentication.Register;

public interface IRegisterService
{
    Task<RegisterResult> RegisterAsync(RegisterInput input, CancellationToken ct = default);    
}