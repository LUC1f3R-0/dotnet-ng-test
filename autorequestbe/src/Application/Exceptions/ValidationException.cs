using System.Net;

namespace Application.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string messege) : base(messege, HttpStatusCode.BadRequest)
    { }
}