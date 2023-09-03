namespace BlogApp.Business.Exceptions.UserExceptions;

public class UserExistException : Exception
{
    public UserExistException() : base("Username or email already exist")
    {
        
    }
    public UserExistException(string? message) : base(message)
    {
    }
}
