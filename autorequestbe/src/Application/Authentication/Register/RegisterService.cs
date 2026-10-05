namespace Application.Authentication.Register;

public sealed class RegisterService : IRegisterService
{
    // private readonly IRegisterService _registerService;
    
    // public RegisterService(IRegisterService register)
    // {
    //      _registerService = register;
    // }

    public async Task RegisterAsync(RegisterInput input, CancellationToken ct = default)
    {
        string[] inputs = [input.Name!, input.Email, input.Password, input.ConfirmPassword];
        
        foreach(string text in inputs)
        {
            Console.WriteLine(text);
        }
        await Task.CompletedTask;
        return;
    }
}