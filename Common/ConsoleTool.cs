using System;

namespace JSDelivrCLI.Common
{
    public static class ConsoleTool
    {
        public static void WriteColorful(string value, ConsoleColor color)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = currentColor;
        }
    }
}