using System;

namespace CommandExamples
{
    interface ICommand
    {
        string Description { get; }
        void Execute();
    }
}