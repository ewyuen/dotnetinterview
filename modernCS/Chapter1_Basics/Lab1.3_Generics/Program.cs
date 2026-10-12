// Lab 1.1: Book Class
Console.WriteLine("=== Lab 1.1: Book Class ===");
Book book1 = new Book("1984", "George Orwell", "123456");
book1.DisplayInfo();
book1.CheckOut();
book1.DisplayInfo();
book1.Return();
book1.DisplayInfo();

Console.WriteLine();

// Lab 1.2: Inheritance with Vehicles
Console.WriteLine("=== Lab 1.2: Inheritance with Vehicles ===");
Vehicle car = new Car("Toyota", "Camry", 2020, 4);
Vehicle bike = new Bike("Harley", "Sportster", 2019, true);

car.DisplayInfo();
Console.WriteLine();
bike.DisplayInfo();

Console.WriteLine();

// Lab 1.3: Generics and Interfaces
Console.WriteLine("=== Lab 1.3: Generics and Interfaces ===");
Library<Book> bookLibrary = new Library<Book>();
bookLibrary.Add(new Book("1984", "George Orwell", "123456"));
bookLibrary.Add(new Book("To Kill a Mockingbird", "Harper Lee", "789012"));
bookLibrary.Add(new Book("Pride and Prejudice", "Jane Austen", "345678"));

Console.WriteLine("All books:");
bookLibrary.GetAll();

Console.WriteLine("\nSearch for 'George':");
var searchResults = bookLibrary.Search("George");
foreach (var book in searchResults)
{
    book.DisplayInfo();
}
