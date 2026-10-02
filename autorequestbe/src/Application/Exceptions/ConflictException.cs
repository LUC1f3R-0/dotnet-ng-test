using System.Net;
namespace Application.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string messege) : base(messege, HttpStatusCode.Conflict)
    { }
}