using ConsoleDictionary.Managers;
using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;

namespace ConsoleDictionary
{
    public class ConsoleApp
    {
        private readonly Trainer _trainer;
        private readonly DictionaryManager _dictionaryManager;
        private readonly string _path = @"words.json";
        public ConsoleApp()
        {
            var words = FillWordList();
            _dictionaryManager = new DictionaryManager(words);
            _trainer = new Trainer(_dictionaryManager);
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

                    case "w": ListWords();
                        Pause();
                        break;

                    case "t": Train();
                        Pause();
                        break;

                    case "s":
                        Statistics();
                        Pause();
                        break;

                    case "l": LoadWords();
                        Pause();
                        break;

                    case "q":
                        if (_dictionaryManager.IsModified) {
                            SaveWords();
                        }
                        isQuit = true;
                        break;

                    default:
                        ConsoleHelper.PrintError("Unexpected input...");
                        Pause();
                        break;
                }
            }

        }

        private void Pause()
        {
            ConsoleHelper.PrintNormal("Press any key to continue.");
            Console.ReadKey();
        }

        private void AddWord()
        {
            Word word;

            ConsoleHelper.PrintNormal("Input Word Text:");
            string Text = Console.ReadLine() ?? "";

            ConsoleHelper.PrintNormal("Input Translations (q - quit):");
            List<string> translations = new List<string>();
            while(true){
                string translation = Console.ReadLine() ?? "";

                if(string.Equals(translation, "q", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                if (!string.IsNullOrEmpty(translation))
                    translations.Add(translation);
                else {
                    ConsoleHelper.PrintError("Empty string. skipped."); 
                }
            }

            ConsoleHelper.PrintNormal("Input category:");
            string category = Console.ReadLine() ?? "";

            word = new Word(Text, translations, category);
            _dictionaryManager.AddWord(word);
        }
        private void DeleteWord()
        {
            ConsoleHelper.PrintNormal("Input word to delete:");
            string text = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(text))
                _dictionaryManager.DeleteWord(text);
            else {
                ConsoleHelper.PrintError("Empty word. Nothing to delete");
            }
        }
        private void ListWords()
        {
            _dictionaryManager.GetAllWords().ForEach(w =>  ConsoleHelper.PrintNormal("==========" + w.ToString()));
            ConsoleHelper.PrintSuccess("Total: " + _dictionaryManager.GetAllWords().Count + " words.");
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
            _dictionaryManager.LoadFromFile(_path);
        }
        private void SaveWords()
        {
            ConsoleHelper.PrintWarning("Your dictionary has been changed! Would you like to save it in file (y/n) ?");
            if ((Console.ReadLine() ?? "").ToLower() == "y") {
                _dictionaryManager.SaveToFile(_path);
            }
        }

        private void PrintMenu()
        {
            Console.Clear();
            ConsoleHelper.PrintNormal("Please, choose option:" +
                                      "\na - add word" +
                                      "\nd - delete word" +
                                      "\nw - list of words" +
                                      "\nt - train" +
                                      "\ns - get statistics" +
                                      "\nl - get words from file" +
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
