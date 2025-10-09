using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Interfaces;
using System;

namespace ConsoleDictionary.Managers
{
    public class Trainer
    {
        private IWordRepository _repository;
        private readonly Random _random;
        private readonly IConsole _console;
        public Trainer(IWordRepository repository)
        {
            this._repository = repository;
            _random = new Random();
            _console = new ConsoleHelper();
        }
        public Word? GetRandomWord()
        {
            var randomWord = _repository.GetAll()
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
            var words = _repository.GetAll().OrderBy(w => _random.Next()).ToList();

            foreach (var word in words) {
                _console.PrintNormal($"Translate word (q - to quit): {word.Text}");
                var answer = (Console.ReadLine() ?? "").ToLower();

                if (answer == "q") {
                    _console.PrintWarning("Training has finished. Quit...");
                    break;
                }

                if (CheckAnswer(word, answer)) {
                    _console.PrintSuccess("✅ Right!");
                }
                else {
                    _console.PrintError($"❌ Wrong. Correct answer: {string.Join(", ", word.Translations)}");
                }
            }
        }

        public void PrintStatistics()
        {
            _console.PrintWarning("\nStatistics: [word] - correct | wrong");
            _repository.GetAll().ToList().ForEach(
                word => _console.PrintNormal($"{word.Text} — {word.CorrectCount} | {word.WrongCount}"));
        }
    }
}
