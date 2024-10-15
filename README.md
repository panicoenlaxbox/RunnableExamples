# Introduction

Runnable examples for demo purposes

Ideas taken from https://github.com/CarlosLanderas/CSharp-6-and-7-features (project RunnableExamples) and https://codereview.stackexchange.com/questions/142627/optimizing-simple-menu-in-console-app (project MenuExamples)

## A better solution

```csharp
using System.Reflection;

RunExamples();

return;

void RunExamples()
{
    var examples = GetExamples().ToList();
    var i = 1;
    foreach (var example in examples)
    {
        Console.WriteLine($"{i++}. {example.GetType().Name}");
    }

    Console.WriteLine($"{i}. Exit");

    Console.Write("Enter the number of the example to run: ");

    while (true)
    {
        var input = Console.ReadLine();
        if (int.TryParse(input, out var runnableIndex) && runnableIndex > 0 && runnableIndex <= examples.Count)
        {
            examples.ElementAt(runnableIndex - 1).Run();
        }
        else if (int.TryParse(input, out var exitIndex) && exitIndex == i)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid input");
        }
    }
}

IEnumerable<IRunnable> GetExamples() =>
    Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(IRunnable).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
        .Select(t => (IRunnable)Activator.CreateInstance(t)!);

internal interface IRunnable
{
    void Run();
}

internal class Example1 : IRunnable
{
    public void Run()
    {
        Console.WriteLine("Example1");
    }
}

internal class Example2 : IRunnable
{
    public void Run()
    {
        Console.WriteLine("Example2");
    }
}
```
