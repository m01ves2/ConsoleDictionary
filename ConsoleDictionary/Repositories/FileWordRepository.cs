using ConsoleDictionary.Entities;
using ConsoleDictionary.Interfaces;
using ConsoleDictionary.Helpers;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace ConsoleDictionary.Repositories
{
    public class FileWordRepository : IWordRepository
    {
        private readonly List<Word> _words; //we can only read this list outside
        private bool _isModified = false;

        public bool IsModified
        {
            get { return _isModified; }
        }

        public FileWordRepository()
        {
            this._words = new List<Word>();
            _isModified = false;
        }

        public FileWordRepository(List<Word> words)
        {
            this._words = new List<Word>(words);
            _isModified = false;
        }

        public OperationResult Add(Word word)
        {
            // looking for this word in dictionary
            var existingWord = _words.FirstOrDefault(w => w.Text.Equals(word.Text, StringComparison.OrdinalIgnoreCase));

            if (existingWord != null) { //found this word in dictionary
                // unit translations
                var newTranslations = word.Translations
                    .Where(t => !existingWord.Translations.Any(et => et.Equals(t, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                if (newTranslations.Count > 0) {
                    existingWord.Translations.AddRange(newTranslations);
                    _isModified = true;
                    return new OperationResult(true, $"Word '{word.Text}' exists. Added {newTranslations.Count} new translations.", _words.Count);
                }
                else {
                    return new OperationResult(true, $"Word '{word.Text}' exists. No new translations added.", _words.Count);
                }
            }

            // a new word, no in dictionary
            _words.Add(word);
            _isModified = true;
            return new OperationResult(true, $"Word '{word.Text}' added with {word.Translations.Count} translations.", _words.Count);
        }

        public OperationResult Delete(string text)
        {
            if (_words.RemoveAll(w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase)) > 0) {
                _isModified = true;
                return new OperationResult(true, $"Found and deleted word \"{text}\"");
            }
            else {
                return new OperationResult(false, "Word \"{text}\" not found");
            }
            
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
        public OperationResult Load(string path)
        {
            if (!File.Exists(path)) {
                return new OperationResult(false, "File not found.");
            }

            try {
                var json = File.ReadAllText(path);
                var loaded = (JsonSerializer.Deserialize<List<Word>>(json) ?? new List<Word>());


                if (loaded.Count == 0) {
                    return new OperationResult(false, "Empty file. No words added.");
                }
                else {
                    _words.Clear();
                    _words.AddRange(loaded);
                    _isModified = false;
                    return new OperationResult(true, $"Dictionary read from file. Total {_words.Count} words");
                }
            }
            catch (Exception ex) {
                return new OperationResult(false, $"Failed to load dictionary: {ex.Message}");
            }
        }

        /// <summary>
        /// writes list of word to file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true - success, false - failure</returns>
        public OperationResult Save(string path)
        {
            try {
                var json = JsonSerializer.Serialize(this._words, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                return new OperationResult(true, "Dictionary saved to file.");
            }
            catch (Exception ex) {
                return new OperationResult(false, $"Failed to save dictionary: {ex.Message}");
            }
        }

        public OperationResult Update(Word oldWord, Word newWord)
        {
            Word? w = Find(oldWord.Text);
            if (w != null) {
                w.Text = newWord.Text;
                w.Translations.Clear();
                w.Translations.AddRange(newWord.Translations);
                w.Category = newWord.Category;
                w.CorrectCount = newWord.CorrectCount;
                w.WrongCount = newWord.WrongCount;
                return new OperationResult(true, $"word {oldWord.Text} updated. There are {w.Translations.Count} translations");
            }
            return new OperationResult(false, $"word {oldWord.Text} not found");
        }
    }
}
