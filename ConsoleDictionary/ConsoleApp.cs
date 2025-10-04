using ConsoleDictionary.Managers;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleDictionary
{
    public class ConsoleApp
    {
        private readonly Trainer _trainer;
        private readonly DictionaryManager _dictionaryManager;
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
                string input = Console.ReadLine().Trim().ToLower();

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

                    case "q": SaveWords();
                        isQuit = true;
                        break;

                    default: Console.WriteLine("Unexpected input...");
                        Pause();
                        break;
                }
            }

        }

        private void Pause()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void AddWord()
        {
            Word word;

            Console.WriteLine("Input Word Text:");
            string Text = Console.ReadLine();

            Console.WriteLine("Input Translations (q - quit):");
            List<string> translations = new List<string>();
            while(true){
                string translation = Console.ReadLine();

                if(string.Equals(translation, "q", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                if (!string.IsNullOrEmpty(translation))
                    translations.Add(translation);
                else { 
                    Console.WriteLine("Empty string. skipped."); 
                }
            }

            Console.WriteLine("Input category:");
            string category = Console.ReadLine();

            word = new Word(Text, translations, category);
            _dictionaryManager.AddWord(word);
        }
        private void DeleteWord()
        {
            Console.WriteLine("Input word to delete:");
            string text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text))
                _dictionaryManager.DeleteWord(text);
            else {
                Console.WriteLine("Empty word. Nothing to delete");
            }
        }
        private void ListWords()
        {
            _dictionaryManager.GetAllWords().ForEach(w =>  Console.WriteLine(w.ToString()));
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
            throw new NotImplementedException();
        }
        private void SaveWords()
        {
            //throw new NotImplementedException();
        }

        private void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Please, choose option:");
            Console.WriteLine("a - add word");
            Console.WriteLine("d - delete word");
            Console.WriteLine("w - list of words");
            Console.WriteLine("t - train");
            Console.WriteLine("s - get statistics");
            Console.WriteLine("l - get words from file");
            Console.WriteLine("q - save and quit");
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
