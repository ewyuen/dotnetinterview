using GraphQLServer.Data;
using GraphQLServer.Models;

namespace GraphQLServer.GraphQL;

public class Query
{
    /// <summary>
    /// Gets all books in the library.
    /// </summary>
    public List<Book> GetBooks([Service] IBookRepository repository)
    {
        return repository.GetAllBooks();
    }

    /// <summary>
    /// Gets a specific book by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the book</param>
    public Book? GetBook(string id, [Service] IBookRepository repository)
    {
        return repository.GetBookById(id);
    }

    /// <summary>
    /// Gets all authors.
    /// </summary>
    public List<Author> GetAuthors([Service] IBookRepository repository)
    {
        return repository.GetAllAuthors();
    }

    /// <summary>
    /// Gets a specific author by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the author</param>
    public Author? GetAuthor(string id, [Service] IBookRepository repository)
    {
        return repository.GetAuthorById(id);
    }

    /// <summary>
    /// Gets the total number of books in the library.
    /// </summary>
    public int GetBookCount([Service] IBookRepository repository)
    {
        return repository.GetAllBooks().Count;
    }

    /// <summary>
    /// Searches for books by title (case-insensitive partial match).
    /// </summary>
    /// <param name="title">The text to search for in book titles</param>
    public List<Book> SearchBooks(string title, [Service] IBookRepository repository)
    {
        return repository.GetAllBooks()
            .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
