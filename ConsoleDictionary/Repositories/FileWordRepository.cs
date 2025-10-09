using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Interfaces;
using System;
using System.Text.Json;

namespace ConsoleDictionary.Repositories
{
    public class FileWordRepository : IWordRepository
    {
        private readonly List<Word> _words; //we can only read this list outside
        private bool _isModified = false;
        private readonly IConsole _console;

        public bool IsModified
        {
            get { return _isModified; }
        }

        public FileWordRepository()
        {
            this._words = new List<Word>();
            _console = new ConsoleHelper();
            _isModified = false;
        }

        public FileWordRepository(List<Word> words)
        {
            this._words = new List<Word>(words);
            _console = new ConsoleHelper();
            _isModified = true;
        }

        public void Add(Word word)
        {
            this._words.Add(word);
            this._isModified = true;
        }

        public void Delete(string text)
        {
            if(_words.RemoveAll(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase)) > 0)
                _isModified = true;
        }
        public Word? Find(string text)
        {
            return _words.FirstOrDefault(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
        }
        public IReadOnlyList<Word> GetAll()
        {
            return this._words.AsReadOnly();
        }

        /// <summary>
        /// loads list of word from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true - success, false - failure</returns>
        public void Load(string path)
        {
            if (!File.Exists(path)) {
                _console.PrintError("File not found. ");
                return;
            }

            try {
                var json = File.ReadAllText(path);
                var loaded = (JsonSerializer.Deserialize<List<Word>>(json) ?? new List<Word>());
                _words.Clear();
                _words.AddRange(loaded);
                _isModified = false;
                _console.PrintSuccess($"Dictionary read from file. Total {_words.Count} words");

            }
            catch (Exception ex) {
                _console.PrintError($"Failed to load dictionary: {ex.Message}");
                //_words = new List<Word>();
            }
        }

        /// <summary>
        /// writes list of word to file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true - success, false - failure</returns>
        public void Save(string path)
        {
            try {
                var json = JsonSerializer.Serialize(this._words, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                _console.PrintSuccess("Dictionary saved to file.");
            }
            catch (Exception ex) {
                _console.PrintError($"Failed to save dictionary: {ex.Message}");
            }
        }
    }
}
