using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Entities;
using ConsoleDictionary.Managers;


namespace ConsoleDictionary.tests
{
    public class DictionaryManagerTests
    {
        [Fact]
        public void AddWord_ShouldIncreaseCount()
        {
            var manager = new DictionaryManager(new List<Word>());
            manager.AddWord(new Word("apple", new List<string> { "яблоко" }));

            Assert.Single(manager.GetAllWords());
        }

        [Fact]
        public void DeleteWord_ShouldRemoveWord()
        {
            var manager = new DictionaryManager(new List<Word> { new Word("apple", new List<string> { "яблоко" }) });
            manager.DeleteWord("apple");

            Assert.Empty(manager.GetAllWords());
        }

        [Fact]
        public void FindWord_ShouldFindWord()
        {
            var manager = new DictionaryManager(new List<Word> { new Word("apple", new List<string> { "яблоко" }) });
            Assert.Equal("apple", manager.FindWord("apple").Text);
        }
    }
}
