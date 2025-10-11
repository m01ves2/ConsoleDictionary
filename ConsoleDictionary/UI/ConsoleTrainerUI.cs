using ConsoleDictionary.Entities;
using ConsoleDictionary.Interfaces;
using ConsoleDictionary.Managers;

namespace ConsoleDictionary.UI
{
    public class ConsoleTrainerUI
    {
        private readonly Trainer _trainer;
        private readonly IConsole _console;

        public ConsoleTrainerUI(Trainer trainer, IConsole console)
        {
            _trainer = trainer;
            _console = console;
        }

        public void Start()
        {
            while (true) {
                var word = _trainer.GetRandomWord();
                if (word == null) {
                    _console.PrintWarning("No words to train.");
                    break;
                }

                string answer = AskTranslation(word);
                if (answer == "q") break;

                CheckAndPrintResult(word, answer);
            }
        }

        private string AskTranslation(Word word)
        {
            _console.PrintNormal($"Translate word (q - to quit): {word.Text}");
            return _console.ReadLine().Trim().ToLower();
        }

        private void CheckAndPrintResult(Word word, string answer)
        {
            if (_trainer.CheckAnswer(word, answer))
                _console.PrintSuccess("✅ Right!");
            else
                _console.PrintError($"❌ Wrong. Correct answer(s): {string.Join(", ", word.Translations)}");
        }

        public void PrintStatistics()
        {
            IEnumerable<(string Text, int Correct, int Wrong)> stat = _trainer.GetStatistics();

            _console.PrintSuccess("\nStatistics: [word] — correct | wrong");
            foreach (var (text, correct, wrong) in _trainer.GetStatistics())
                _console.PrintNormal($"{text,-15} {correct,3} | {wrong,3}");
        }
    }
}
