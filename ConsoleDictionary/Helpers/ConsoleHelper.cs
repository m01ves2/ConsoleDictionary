using ConsoleDictionary.Interfaces;

namespace ConsoleDictionary.Helpers
{
    public class ConsoleHelper : IConsole
    {
        /// <summary>
        /// Prints a message in a specified color, then restores the original color.
        /// </summary>
        private void Print(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Prints a message in a specified color without a line break.
        /// </summary>
        private void PrintInline(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Prints a success message in green.
        /// </summary>
        public void PrintSuccess(string message)
        {
            Print(message, ConsoleColor.Green);
        }

        /// <summary>
        /// Prints an error message in red.
        /// </summary>
        public void PrintError(string message)
        {
            Print(message, ConsoleColor.Red);
        }

        /// <summary>
        /// Prints a warning message in yellow.
        /// </summary>
        public void PrintWarning(string message)
        {
            Print(message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Prints usual message in default color.
        /// </summary>
        /// <param name="message"></param>
        public void PrintNormal(string message)
        {
            Print(message, Console.ForegroundColor);
        }

        public string ReadLine()
        {
            return Console.ReadLine() ?? "";
        }
    }
}
