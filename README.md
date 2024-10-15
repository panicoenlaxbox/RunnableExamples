# Introduction

Runnable examples for demo purposes

Ideas taken from https://github.com/CarlosLanderas/CSharp-6-and-7-features (project RunnableExamples) and https://codereview.stackexchange.com/questions/142627/optimizing-simple-menu-in-console-app (project MenuExamples)

## A better solution

```csharp
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

var services = new ServiceCollection();
services
    .AddTransient<IConfiguration>(_ => configuration)
    .AddLogging(configure => configure.AddConsole());

RegisterExamples();
await RunExamplesAsync();

return;

async Task RunExamplesAsync()
{
    var examples = GetExamples();
    
    var i = 1;
    foreach (var example in examples)
    {
        Console.WriteLine($"{i++}. {example}");
    }
    Console.WriteLine($"{i}. Exit");
    Console.Write("Enter the number of the example to run: ");

    while (true)
    {
        var input = Console.ReadLine();
        if (int.TryParse(input, out var exampleIndex) && exampleIndex > 0 && exampleIndex <= examples.Length)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            await scope.ServiceProvider.GetRequiredKeyedService<IRunnable>(examples.ElementAt(exampleIndex - 1))
                .RunAsync();
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

void RegisterExamples() =>
    Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(IRunnable).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
        .Select(t => t)
        .ToList()
        .ForEach(t => { services.AddKeyedTransient(typeof(IRunnable), t.Name, t); });


string[] GetExamples()
{
    return services.Where(sd => sd.ServiceType == typeof(IRunnable))
        .Select(sd => (string)sd.ServiceKey!)
        .ToArray();
}

internal interface IRunnable
{
    Task RunAsync();
}

internal class Example1(ILogger<Example1> logger) : IRunnable
{
    public Task RunAsync()
    {
        logger.LogInformation("Example1");
        return Task.CompletedTask;
    }
}

internal class Example2 : IRunnable
{
    public Task RunAsync()
    {
        Console.WriteLine("Example2");
        return Task.CompletedTask;
    }
}
```
