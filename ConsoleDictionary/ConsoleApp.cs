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
        private readonly string _path = @"words.json";
        public ConsoleApp()
        {
            var words = FillWordList();
            _repository = new FileWordRepository(words);
            _trainer = new Trainer(_repository);
            _console = new ConsoleHelper();
        }

        public void Run()
        {   
            bool isQuit = false;
            while (!isQuit) {
                PrintMenu();
                string input = Console.ReadLine() ?? "".Trim().ToLower();

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
            string Text = Console.ReadLine() ?? "";

            _console.PrintNormal("Input Translations (q - quit):");
            List<string> translations = new List<string>();
            while(true){
                string translation = Console.ReadLine() ?? "";

                if(string.Equals(translation, "q", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                if (!string.IsNullOrEmpty(translation))
                    translations.Add(translation);
                else {
                    _console.PrintError("Empty string. skipped."); 
                }
            }

            _console.PrintNormal("Input category:");
            string category = Console.ReadLine() ?? "";

            word = new Word(Text, translations, category);
            _repository.Add(word);
        }
        private void DeleteWord()
        {
            _console.PrintNormal("Input word to delete:");
            string text = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(text))
                _repository.Delete(text);
            else {
                _console.PrintError("Empty word. Nothing to delete");
            }
        }
        private void ListWords()
        {
            _repository.GetAll().ToList().ForEach(w => _console.PrintNormal("==========" + w.ToString()));
            _console.PrintSuccess("Total: " + _repository.GetAll().Count + " words.");
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
            _repository.Load(_path);
        }
        private void SaveWords()
        {
            _console.PrintWarning("Your dictionary has been changed! Would you like to save it in file (y/n) ?");
            if ((Console.ReadLine() ?? "").ToLower() == "y") {
                _repository.Save(_path);
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
