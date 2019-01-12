using System;

namespace CommandExamples
{
    class Example1Command : ICommand
    {
        public string Description => "Example 1";

        public void Execute() => Console.WriteLine($"Executing {nameof(Example1Command)}");
    }
}