namespace Patterns;

// Simple interpreter that evaluates expressions of digits separated by +
public class Interpreter
{
    public int Interpret(string input)
    {
        var parts = input.Split('+');
        return parts.Select(p => int.Parse(p.Trim())).Sum();
    }

    public static void Run()
    {
        var i = new Interpreter();
        Console.WriteLine(i.Interpret("1+ 2 +3"));
    }
}
