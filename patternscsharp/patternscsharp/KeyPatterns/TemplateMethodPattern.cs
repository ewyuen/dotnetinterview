namespace Patterns;

public abstract class AbstractClass
{
    public void TemplateMethod()
    {
        Step1();
        Step2();
        Hook();
    }
    protected abstract void Step1();
    protected abstract void Step2();
    protected virtual void Hook() { }
}

public class ConcreteClass : AbstractClass
{
    protected override void Step1() => Console.WriteLine("Step1");
    protected override void Step2() => Console.WriteLine("Step2");

    public static void Run()
    {
        var c = new ConcreteClass();
        c.TemplateMethod();
    }
}
