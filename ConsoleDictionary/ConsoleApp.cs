using ConsoleDictionary.Managers;
using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Interfaces;
using ConsoleDictionary.Repositories;

namespace ConsoleDictionary
{
    public class ConsoleApp
    {
        private readonly Trainer _trainer;
        private readonly IWordRepository _repository;
        private readonly IConsole _console;
        private readonly string _path;
        public ConsoleApp(string path)
        {
            var words = FillWordList();
            _repository = new FileWordRepository(words);
            _trainer = new Trainer(_repository);
            _console = new ConsoleHelper();
            _path = path;
        }

        public void Run()
        {   
            bool isQuit = false;
            while (!isQuit) {
                PrintMenu();
                string input = _console.ReadLine().Trim().ToLower();

                switch (input) {
                    case "a": AddWord();
                        Pause();
                        break;

                    case "d": DeleteWord();
                        Pause();
                        break;

                    case "l": ListWords();
                        Pause();
                        break;

                    case "t": Train();
                        Pause();
                        break;

                    case "s":
                        Statistics();
                        Pause();
                        break;

                    case "r": LoadWords();
                        Pause();
                        break;

                    case "w":
                        if (_repository.IsModified) {
                            SaveWords();
                        }
                        break;

                    case "q":
                        if (_repository.IsModified) {
                            SaveWords();
                        }
                        isQuit = true;
                        break;

                    default:
                        _console.PrintError("Unexpected input...");
                        Pause();
                        break;
                }
            }

        }

        private void Pause()
        {
            _console.PrintNormal("Press any key to continue.");
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

            if(result.Success)
                _console.PrintSuccess(result.Message);
            else
                _console.PrintWarning(result.Message);
        }
        private void DeleteWord()
        {
            _console.PrintNormal("Input word to delete:");
            string text = _console.ReadLine();
            if (!string.IsNullOrEmpty(text)) {
                OperationResult result = _repository.Delete(text);

                if (result.Success)
                    _console.PrintSuccess(result.Message);
                else
                    _console.PrintWarning(result.Message);    
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
            _trainer.StartTraining();
        }
        private void Statistics()
        {
            _trainer.PrintStatistics();
        }
        private void LoadWords()
        {
            OperationResult result = _repository.Load(_path);

            if (result.Success)
                _console.PrintSuccess(result.Message);
            else
                _console.PrintWarning(result.Message);
        }
        private void SaveWords()
        {
            _console.PrintWarning("Your dictionary has been changed! Would you like to save it in file (y/n) ?");
            if (_console.ReadLine().ToLower() == "y") {
                OperationResult result = _repository.Save(_path);

                if(result.Success)
                    _console.PrintSuccess(result.Message);
                else
                    _console.PrintWarning(result.Message);
            }
        }

        private void PrintMenu()
        {
            Console.Clear();
            _console.PrintNormal("Please, choose option:" +
                                      "\na - add word" +
                                      "\nd - delete word" +
                                      "\nl - list of words" +
                                      "\nt - train" +
                                      "\ns - get statistics" +
                                      "\nr - read words from file" +
                                      "\nw - write words to file" +
                                      "\nq - quit");
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
