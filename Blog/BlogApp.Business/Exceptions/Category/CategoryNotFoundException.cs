namespace BlogApp.Business.Exceptions.Category;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException() : base("Kateqoriya tapilmadi!") { }
    public CategoryNotFoundException(string? message) : base(message)
    {
    }
}
