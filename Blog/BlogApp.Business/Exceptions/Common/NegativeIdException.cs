using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Common;

public class NegativeIdException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }
    public NegativeIdException()
    {
        ErrorMessage = "id 0-dan kicik ve ya beraber ola bilmez";
    }
    public NegativeIdException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}
