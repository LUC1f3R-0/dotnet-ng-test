using API.Register.Models;

namespace Application.Authentication.Register;

public interface IRegisterService
{
    Task<RegisterResult> RegisterAsync(RegisterInput input, CancellationToken ct = default);    
}