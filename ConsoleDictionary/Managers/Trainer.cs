using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;

namespace ConsoleDictionary.Managers
{
    public class Trainer
    {
        private DictionaryManager _dictionary;
        private readonly Random _random;
        public Trainer(DictionaryManager dictionary)
        {
            this._dictionary = dictionary;
            _random = new Random();
        }
        public Word? GetRandomWord()
        {
            var randomWord = _dictionary.GetAllWords()
                    .OrderBy(w => _random.Next())
                    .FirstOrDefault();
            return randomWord;
        }
        public bool CheckAnswer(Word word, string userAnswer)
        {
            if (word.Translations.Any(t => t.Equals( userAnswer.Trim(), StringComparison.OrdinalIgnoreCase))) {
                word.CorrectCount++;
                return true;
            }
            else {
                word.WrongCount++;
                return false;
            }
        }

        internal void StartTraining()
        {
            var words = _dictionary.GetAllWords().OrderBy(w => _random.Next()).ToList();

            foreach (var word in words) {
                ConsoleHelper.PrintNormal($"Translate word (q - to quit): {word.Text}");
                var answer = (Console.ReadLine() ?? "").ToLower();

                if (answer == "q") {
                    ConsoleHelper.PrintWarning("Training has finished. Quit...");
                    break;
                }

                if (CheckAnswer(word, answer)) {
                    ConsoleHelper.PrintSuccess("✅ Right!");
                }
                else {
                    ConsoleHelper.PrintError($"❌ Wrong. Correct answer: {string.Join(", ", word.Translations)}");
                }
            }
        }

        public void PrintStatistics()
        {
            ConsoleHelper.PrintWarning("\nStatistics: [word] - correct | wrong");
            _dictionary.GetAllWords().ForEach(
                word => ConsoleHelper.PrintNormal($"{word.Text} — {word.CorrectCount} | {word.WrongCount}"));
        }
    }
}
