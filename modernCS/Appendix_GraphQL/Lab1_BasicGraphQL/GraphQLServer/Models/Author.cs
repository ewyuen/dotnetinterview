namespace GraphQLServer.Models;

public class Author
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string Country { get; set; } = string.Empty;
}
