using System;

namespace CommandExamples
{
    class Example2Command : ICommand
    {
        public string Description => "Example 2";

        public void Execute() => Console.WriteLine($"Executing {nameof(Example2Command)}");
    }
}