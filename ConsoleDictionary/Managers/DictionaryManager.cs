using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDictionary.Managers
{
    internal class DictionaryManager
    {
        private List<Word> words;

        public DictionaryManager(List<Word> words)
        {
            this.words = words;
        }

        public void AddWord(Word word)
        {
            this.words.Add(word);
        }

        public void DeleteWord(string text)
        {
            words.RemoveAll(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
        }
        public Word? FindWord(string text)
        {
            return words.FirstOrDefault(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
        }
        public List<Word> GetAllWords() 
        {  
            return this.words; 
        }
        public void LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }
        public void SaveToFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
