namespace ConsoleDictionary
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var app = new ConsoleApp(@"words.json");
            app.Run();
        }
    }
}