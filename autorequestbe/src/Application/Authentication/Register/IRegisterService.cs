namespace Application.Authentication.Register;

public interface IRegisterService
{
    Task RegisterAsync(RegisterInput input, CancellationToken ct = default);
}