using System;
using System.Linq;
using System.Reflection;

namespace RunnableExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteRunnables();
        }

        private static void ExecuteRunnables()
        {
            var runnables = (from type in typeof(Program).GetTypeInfo().Assembly.GetTypes()
                             let typeInfo = type.GetTypeInfo()
                             where typeInfo.IsClass && typeof(IRunnable).IsAssignableFrom(type)
                             select type).ToList();

            foreach (var type in runnables)
            {
                var runnable = Activator.CreateInstance(type) as IRunnable;
                Console.WriteLine($"Running example {type.Name}...");
                runnable.Run();
            }
        }
    }
}
