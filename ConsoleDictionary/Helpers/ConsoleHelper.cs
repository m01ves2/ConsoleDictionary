namespace ConsoleDictionary.Helpers
{
    public static class ConsoleHelper
    {
        /// <summary>
        /// Prints a message in a specified color, then restores the original color.
        /// </summary>
        public static void Print(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Prints a message in a specified color without a line break.
        /// </summary>
        public static void PrintInline(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Prints a success message in green.
        /// </summary>
        public static void PrintSuccess(string message)
        {
            Print(message, ConsoleColor.Green);
        }

        /// <summary>
        /// Prints an error message in red.
        /// </summary>
        public static void PrintError(string message)
        {
            Print(message, ConsoleColor.Red);
        }

        /// <summary>
        /// Prints a warning message in yellow.
        /// </summary>
        public static void PrintWarning(string message)
        {
            Print(message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Prints usual message in default color.
        /// </summary>
        /// <param name="message"></param>
        public static void PrintNormal(string message)
        {
            Print(message, Console.ForegroundColor);
        }
    }
}
