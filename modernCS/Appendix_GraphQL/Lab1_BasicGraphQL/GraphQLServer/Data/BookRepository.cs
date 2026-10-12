using GraphQLServer.Models;

namespace GraphQLServer.Data;

public interface IBookRepository
{
    List<Book> GetAllBooks();
    Book? GetBookById(string id);
    List<Author> GetAllAuthors();
    Author? GetAuthorById(string id);
}

public class BookRepository : IBookRepository
{
    private readonly List<Author> _authors;
    private readonly List<Book> _books;

    public BookRepository()
    {
        // Initialize sample authors
        _authors = new List<Author>
        {
            new Author
            {
                Id = "1",
                Name = "F. Scott Fitzgerald",
                BirthYear = 1896,
                Country = "United States"
            },
            new Author
            {
                Id = "2",
                Name = "Harper Lee",
                BirthYear = 1926,
                Country = "United States"
            },
            new Author
            {
                Id = "3",
                Name = "George Orwell",
                BirthYear = 1903,
                Country = "United Kingdom"
            },
            new Author
            {
                Id = "4",
                Name = "Jane Austen",
                BirthYear = 1775,
                Country = "United Kingdom"
            },
            new Author
            {
                Id = "5",
                Name = "J.K. Rowling",
                BirthYear = 1965,
                Country = "United Kingdom"
            }
        };

        // Initialize sample books
        _books = new List<Book>
        {
            new Book
            {
                Id = "1",
                Title = "The Great Gatsby",
                AuthorId = "1",
                PublishedYear = 1925,
                Genre = "Fiction",
                Pages = 180
            },
            new Book
            {
                Id = "2",
                Title = "To Kill a Mockingbird",
                AuthorId = "2",
                PublishedYear = 1960,
                Genre = "Fiction",
                Pages = 324
            },
            new Book
            {
                Id = "3",
                Title = "1984",
                AuthorId = "3",
                PublishedYear = 1949,
                Genre = "Science Fiction",
                Pages = 328
            },
            new Book
            {
                Id = "4",
                Title = "Animal Farm",
                AuthorId = "3",
                PublishedYear = 1945,
                Genre = "Political Satire",
                Pages = 112
            },
            new Book
            {
                Id = "5",
                Title = "Pride and Prejudice",
                AuthorId = "4",
                PublishedYear = 1813,
                Genre = "Romance",
                Pages = 432
            },
            new Book
            {
                Id = "6",
                Title = "Harry Potter and the Philosopher's Stone",
                AuthorId = "5",
                PublishedYear = 1997,
                Genre = "Fantasy",
                Pages = 223
            }
        };
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }

    public Book? GetBookById(string id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public List<Author> GetAllAuthors()
    {
        return _authors;
    }

    public Author? GetAuthorById(string id)
    {
        return _authors.FirstOrDefault(a => a.Id == id);
    }
}
