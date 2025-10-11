using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Interfaces;
using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace ConsoleDictionary.Managers
{
    public class Trainer
    {
        private IWordRepository _repository;
        private readonly Random _random;

        public Trainer(IWordRepository repository)
        {
            this._repository = repository;
            _random = new Random();
        }
        public Word? GetRandomWord()
        {
            var all = _repository.GetAll();
            if (all.Count == 0) 
                return null;
            int index = _random.Next(all.Count);
            return all[index];
        }
        public bool CheckAnswer(Word word, string userAnswer)
        {
            if (word.Translations.Any(t => t.Equals(userAnswer.Trim(), StringComparison.OrdinalIgnoreCase))) {
                word.CorrectCount++;
                return true;
            }
            else {
                word.WrongCount++;
                return false;
            }
        }

        public IEnumerable<(string Text, int Correct, int Wrong)> GetStatistics()
        {
            return _repository.GetAll().Select(w => (w.Text, w.CorrectCount, w.WrongCount));
        }
    }
}
