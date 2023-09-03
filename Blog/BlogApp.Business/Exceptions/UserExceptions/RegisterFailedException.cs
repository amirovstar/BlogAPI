namespace BlogApp.Business.Exceptions.UserExceptions;

public class RegisterFailedException : Exception
{
    public RegisterFailedException() : base("Register failed for some reasons")
    {
        
    }
    public RegisterFailedException(string? message) : base(message)
    {
    }
}
