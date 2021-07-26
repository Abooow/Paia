using System;

namespace Paia.CliTest.Services
{
    internal sealed class PrintHello : IPrintHello
    {
        public void Print()
        {
            PrintMessage();
        }

        public void PrintLine()
        {
            PrintMessage('\n');
        }

        private void PrintMessage(char end = '\0')
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Hello" + end);
            Console.ResetColor();
        }
    }
}
