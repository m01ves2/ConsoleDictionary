using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDictionary.Managers
{
    internal class Trainer
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
                Console.WriteLine($"Translate word (q - to quit): {word.Text}");
                var answer = Console.ReadLine().ToLower();

                if (answer == "q") {
                    Console.WriteLine("Training has finished. Quit...");
                    break;
                }

                if (CheckAnswer(word, answer)) {
                    Console.WriteLine("✅ Right!");
                }
                else {
                    Console.WriteLine($"❌ Wrong. Correct answer: {string.Join(", ", word.Translations)}");
                }
            }
        }

        public void PrintStatistics()
        {
            Console.WriteLine("\nStatistics: [word] - correct | wrong");
            _dictionary.GetAllWords().ForEach(
                word => Console.WriteLine($"{word.Text} — {word.CorrectCount} | {word.WrongCount}"));
        }
    }
}
