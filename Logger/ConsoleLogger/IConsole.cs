using System;

namespace Logger
{
    public interface IConsole
    {
        ConsoleColor ForegroundColor { get; set; }

        void WriteLine(string text);

        void ResetColor();
    }
}
