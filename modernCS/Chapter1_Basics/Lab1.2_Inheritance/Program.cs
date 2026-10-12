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
