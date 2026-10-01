using System.Net;

namespace Application.Exceptions;

public abstract class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }
    
    public AppException(string messege, HttpStatusCode statusCode):base(messege)
    {
        StatusCode = statusCode;
    }
}
