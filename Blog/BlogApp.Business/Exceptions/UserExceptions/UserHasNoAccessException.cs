namespace BlogApp.Business.Exceptions.UserExceptions;

public class UserHasNoAccessException : Exception
{
    public UserHasNoAccessException() : base("User has no acces this command")
    {
    }

    public UserHasNoAccessException(string? message) : base(message)
    {
    }
}
