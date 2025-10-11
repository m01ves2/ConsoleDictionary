using ConsoleDictionary.Managers;
using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Interfaces;
using ConsoleDictionary.Repositories;

namespace ConsoleDictionary.UI
{
    public class ConsoleApp
    {
        private readonly Trainer _trainer;
        private readonly ConsoleTrainerUI _trainerUI;
        private readonly IWordRepository _repository;
        private readonly IConsole _console;
        private readonly string _path;
        public ConsoleApp(string path)
        {
            var words = FillWordList();
            _repository = new FileWordRepository(words);
            if (File.Exists(path))
                _repository.Load(path);
            else
                _repository = new FileWordRepository(FillWordList());

            _trainer = new Trainer(_repository);
            _console = new ConsoleHelper();
            _trainerUI = new ConsoleTrainerUI(_trainer, _console);
            _path = path;
        }

        public void Run()
        {
            bool isQuit = false;
            
            Dictionary<string, Action> commands = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase) {
                { "a", () => AddWord()},
                { "d", () => DeleteWord()},
                { "u", () => UpdateWord()},
                { "l", () => ListWords()},
                { "t", () => Train()},
                { "s", () => TrainStatistics()},
                { "r", () => LoadWords()},
                { "w", () => { if(_repository.IsModified) SaveWords(); } },
                { "q", () => { if(_repository.IsModified) SaveWords(); isQuit = true; } }
            };

            while (!isQuit) {
                PrintMenu();
                string input = _console.ReadLine().Trim().ToLower();

                if (string.IsNullOrEmpty(input)) {
                    _console.PrintError("Please enter a command.");
                    continue;
                }

                if (commands.TryGetValue(input, out var command)) {
                    command();
                }
                else {
                    _console.PrintError("Unexpected input...");
                }
                
                Pause();
            }
        }

        private void Pause(string message = "Press any key to continue...")
        {
            _console.PrintWarning(message);
            Console.ReadKey();
        }

        private void AddWord()
        {
            string? text = ReadNonEmptyInput("Input text of the word to add");
            if (text == null) return;

            var translations = ReadTranslations();
            if (translations.Count == 0) {
                _console.PrintError("No translations entered. Aborting.");
                return;
            }

            string category = ReadOptionalInput("Input category of the word");

            var word = new Word(text, translations, category);
            var result = _repository.Add(word);
            PrintResult(result);
        }
        private void DeleteWord()
        {
            string? text = ReadNonEmptyInput("Input text of the word to delete");
            if (text == null) return;

            OperationResult result = _repository.Delete(text);
            PrintResult(result);
        }

        private void UpdateWord()
        {
            string? oldWordText = ReadNonEmptyInput("Input text of the word to update");
            if (oldWordText == null) return;
            Word? oldWord = _repository.Find(oldWordText);
            if(oldWord == null) {
                _console.PrintError("Word not found. Aborting.");
                return;
            }

            string? text = ReadNonEmptyInput("Input new text of the word");
            if (text == null) return;

            var translations = ReadTranslations();
            if (translations.Count == 0) {
                _console.PrintError("No translations entered. Aborting.");
                return;
            }

            string category = ReadOptionalInput("Input category of the word");

            var newWord = new Word(text, translations, category);
            var result = _repository.Update(oldWord, newWord);
            PrintResult(result);
        }

        private List<string> ReadTranslations()
        {
            var translations = new List<string>();
            int count = 0;

            _console.PrintNormal("Input translations (q - quit):");

            while (true) {
                count++;
                _console.PrintNormal($"#{count}:");
                string translation = _console.ReadLine().Trim();

                if (string.Equals(translation, "q", StringComparison.OrdinalIgnoreCase))
                    break;

                if (!string.IsNullOrEmpty(translation))
                    translations.Add(translation);
                else
                    _console.PrintError("Empty string skipped.");
            }

            return translations;
        }

        private void ListWords()
        {
            IReadOnlyList<Word> words = _repository.GetAll();

            words.ToList().ForEach(w => _console.PrintNormal("==========" + w.ToString()));
            _console.PrintSuccess("Total: " + words.Count + " words.");
        }
        private void Train()
        {
            _trainerUI.Start();
        }
        private void TrainStatistics()
        {
            _trainerUI.PrintStatistics();
        }
        private void LoadWords()
        {
            OperationResult result = _repository.Load(_path);
            PrintResult(result);
        }
        private void SaveWords()
        {
            _console.PrintWarning("Your dictionary has been changed! Would you like to save it in file (y/n) ?");
            if (_console.ReadLine().ToLower() == "y") {
                OperationResult result = _repository.Save(_path);
                PrintResult(result);
            }
        }

        private void PrintResult(OperationResult result)
        {
            if (result.Success)
                _console.PrintSuccess(result.Message);
            else
                _console.PrintWarning(result.Message);
        }

        private void PrintMenu()
        {
            Console.Clear();
            _console.PrintNormal("──────────────────────────────\n" +
                                 "         MAIN MENU\n" + 
                                 "──────────────────────────────\n" +
                                 "a - add word\n" +
                                 "d - delete word\n" +
                                 "l - list of words\n" +
                                 "t - train\n" +
                                 "s - get statistics\n" +
                                 "r - read words from file\n" +
                                 "w - write words to file\n" +
                                 "q - quit\n");
        }

        private List<Word> FillWordList()
        {
            List<Word> words = new List<Word>
            {
                new Word("apple", new List<string> { "яблоко"}, "фрукт"),
                new Word("pear", new List<string> {"груша"}, "фрукт"),
                new Word("apricot", new List<string> {"абрикос"}, "фрукт"),
                new Word("tomato", new List<string> { "помидор"}, "ягода"),
                new Word("potato", new List<string> {"картофель"}, "овощ"),
                new Word("egg", new List<string> {"яйцо"}),
                new Word("plane", new List<string> {"самолёт"}),
            };
            return words;
        }
        private string? ReadNonEmptyInput(string prompt)
        {
            _console.PrintNormal(prompt);
            string input = _console.ReadLine().Trim();
            if (string.IsNullOrEmpty(input)) {
                _console.PrintError("Empty input. Operation cancelled.");
                return null;
            }
            return input;
        }
        private string ReadOptionalInput(string prompt, string defaultValue = "")
        {
            _console.PrintNormal($"{prompt} (optional, press Enter to skip):");
            return _console.ReadLine()?.Trim() ?? defaultValue;
        }
    }
}
