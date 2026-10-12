namespace Patterns;

public class Prototype : ICloneable
{
    public string Data { get; set; } = "";
    public object Clone() => MemberwiseClone();

    public static void Run()
    {
        var p1 = new Prototype { Data = "Original" };
        var p2 = (Prototype)p1.Clone();
        Console.WriteLine($"Prototype: p1={p1.Data}, p2={p2.Data}, same={ReferenceEquals(p1, p2)}");
    }
}
