public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = true;
    }

    public void CheckOut()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"{Title} has been checked out.");
        }
        else
        {
            Console.WriteLine($"{Title} is not available.");
        }
    }

    public void Return()
    {
        IsAvailable = true;
        Console.WriteLine($"{Title} has been returned.");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Available: {IsAvailable}");
    }
}