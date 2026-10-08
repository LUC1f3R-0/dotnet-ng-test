using System.Net;

namespace Application.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message):base(message, HttpStatusCode.Unauthorized)
    { }
}