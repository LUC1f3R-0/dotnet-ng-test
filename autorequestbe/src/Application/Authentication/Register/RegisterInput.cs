namespace Application.Authentication.Register;

public sealed record RegisterInput(string? Name, string Email, string Pass, string ConfirmPass);