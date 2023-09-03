namespace BlogApp.Core.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public List<BlogCategory> BlogCategories { get; set; }
}
