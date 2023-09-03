namespace BlogApp.Business.Exceptions.UserExceptions;

public class LoginFailedException : Exception
{
    public LoginFailedException() : base("Login failed for some reason")
    {
        
    }
    public LoginFailedException(string? message) : base(message)
    {
    }
}
