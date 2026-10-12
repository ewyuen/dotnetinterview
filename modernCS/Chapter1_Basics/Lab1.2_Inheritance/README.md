# Lab 1.2: Inheritance with Vehicles

## Goal
This lab demonstrates inheritance, one of the core principles of object-oriented programming. Building on the Book class from Lab 1.1, you'll learn:
- Creating base classes and derived classes
- Method overriding and polymorphism
- The `virtual` and `override` keywords
- Using inheritance to create class hierarchies

## Illustration
We'll create a vehicle hierarchy with a base `Vehicle` class and two derived classes: `Car` and `Bike`. This shows how inheritance allows code reuse and specialization.

### Key Concepts
- **Base Class**: The parent class that contains common properties and methods
- **Derived Class**: Child classes that inherit from the base class
- **Polymorphism**: The ability to treat derived class objects as base class objects
- **Method Overriding**: Providing specific implementations in derived classes

## Implementation
The implementation builds on Lab 1.1 by adding vehicle classes alongside the existing Book class.

### Vehicle.cs (Base Class)
```csharp
public class Vehicle
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public Vehicle(string make, string model, int year)
    {
        Make = make;
        Model = model;
        Year = year;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Make: {Make}, Model: {Model}, Year: {Year}");
    }
}
```

### Car.cs (Derived Class)
```csharp
public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }

    public Car(string make, string model, int year, int doors) : base(make, model, year)
    {
        NumberOfDoors = doors;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo(); // Call base class method
        Console.WriteLine($"Number of Doors: {NumberOfDoors}");
    }
}
```

### Bike.cs (Derived Class)
```csharp
public class Bike : Vehicle
{
    public bool HasSidecar { get; set; }

    public Bike(string make, string model, int year, bool hasSidecar) : base(make, model, year)
    {
        HasSidecar = hasSidecar;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo(); // Call base class method
        Console.WriteLine($"Has Sidecar: {HasSidecar}");
    }
}
```

### Program.cs
```csharp
// Demonstrate inheritance and polymorphism
Vehicle car = new Car("Toyota", "Camry", 2020, 4);
Vehicle bike = new Bike("Harley", "Sportster", 2019, true);

// Both objects are treated as Vehicle but exhibit different behavior
car.DisplayInfo();
Console.WriteLine();
bike.DisplayInfo();
```

## Running the Lab
1. Navigate to this directory
2. Run `dotnet run`
3. Observe how different vehicle types display their information

## Expected Output
```
Make: Toyota, Model: Camry, Year: 2020
Number of Doors: 4

Make: Harley, Model: Sportster, Year: 2019
Has Sidecar: True
```

## Learning Outcomes
After completing this lab, you should understand:
- How inheritance promotes code reuse
- The difference between `virtual` and `override`
- How polymorphism allows flexible object handling
- The importance of calling base class constructors with `: base()`
- How derived classes can extend or modify base class behavior