using System.Text.Json;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Entities;

namespace ConsoleDictionary.Managers
{
    public class DictionaryManager
    {
        private List<Word> _words;
        private bool _isModified = false;

        public bool IsModified
        {
            get { return _isModified; }
        }

        public DictionaryManager(List<Word> words)
        {
            this._words = words;
        }

        public void AddWord(Word word)
        {
            this._words.Add(word);
            this._isModified = true;
        }

        public void DeleteWord(string text)
        {
            int numOfModified = 0;
            numOfModified = _words.RemoveAll(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
            
            if (numOfModified > 0) {
                _isModified = true;
            }
        }
        public Word? FindWord(string text)
        {
            return _words.FirstOrDefault(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
        }
        public List<Word> GetAllWords() 
        {  
            return this._words; 
        }
        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) {
                ConsoleHelper.PrintError("File not found. Working with an old dictionary.");
                return;
            }

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                var loaded = JsonSerializer.Deserialize<List<Word>>(fs) ?? new List<Word>();
                _words.Clear();
                _words.AddRange(loaded);
                _isModified = true;
                ConsoleHelper.PrintSuccess($"Loaded {_words.Count} words from file.");
            }
        }
        public void SaveToFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                JsonSerializer.Serialize<List<Word>>(fs, _words, new JsonSerializerOptions { WriteIndented = true });
                ConsoleHelper.PrintSuccess("Dictionary saved to file.");
            }
        }
    }
}
