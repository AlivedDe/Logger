using System;

namespace Logger.Console
{
    public class ConsoleAdapter : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            get
            {
                return System.Console.ForegroundColor;
            }
            set
            {
                System.Console.ForegroundColor = value;
            }
        }

        public void ResetColor()
        {
            System.Console.ResetColor();
        }

        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}
