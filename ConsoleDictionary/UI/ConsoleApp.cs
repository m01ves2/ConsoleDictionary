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
            //if (File.Exists(path))
            //    _repository.Load(path);
            //else
            //    _repository = new FileWordRepository(FillWordList());

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
            Word word;

            _console.PrintNormal("Input Word Text:");
            string Text = _console.ReadLine();

            _console.PrintNormal("Input Translations (q - quit):");
            List<string> translations = new List<string>();
            int tNum = 0;
            while(true){
                tNum++;
                _console.PrintNormal($"#{tNum}:");
                string translation = _console.ReadLine();

                if(string.Equals(translation, "q", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                if (!string.IsNullOrEmpty(translation))
                    translations.Add(translation);
                else
                    _console.PrintError("Empty string. skipped."); 
            }

            _console.PrintNormal("Input category:");
            string category = _console.ReadLine();

            word = new Word(Text, translations, category);
            OperationResult result = _repository.Add(word);
            PrintResult(result);
        }
        private void DeleteWord()
        {
            _console.PrintNormal("Input word to delete:");
            string text = _console.ReadLine();
            if (!string.IsNullOrEmpty(text)) {
                OperationResult result = _repository.Delete(text);
                PrintResult(result);
            }
            else {
                _console.PrintError("Empty word. Nothing to delete");
            }
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
    }
}
