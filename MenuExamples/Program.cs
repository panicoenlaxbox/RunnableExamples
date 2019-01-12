using System;
using System.Linq;
using System.Reflection;

namespace CommandExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = (from type in typeof(Program).GetTypeInfo().Assembly.GetTypes()
                            let typeInfo = type.GetTypeInfo()
                            where typeInfo.IsClass && typeof(ICommand).IsAssignableFrom(type)
                            select type).Select(Activator.CreateInstance).Cast<ICommand>().ToArray();

            while (true)
            {
                for (var i = 0; i < commands.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, commands[i].Description);
                }

                Console.WriteLine("{0}. {1}", commands.Length + 1, "Exit");

                string input;
                int index;
                do
                {
                    input = Console.ReadLine();
                }
                while (!int.TryParse(input, out index) || !IsValidIndex(index, commands));

                var isCommand = index < commands.Length + 1;
                if (isCommand)
                {
                    commands[index - 1].Execute();
                }

                Environment.Exit(0);
            }
        }

        private static bool IsValidIndex(int index, ICommand[] commands)
        {
            return (index >= 1 && index <= commands.Length + 1);
        }
    }
}
