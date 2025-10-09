using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDictionary.Helpers;
using ConsoleDictionary.Entities;
using ConsoleDictionary.Managers;
using ConsoleDictionary.Repositories;


namespace ConsoleDictionary.tests
{
    public class FileWordRepositoryTests
    {
        [Fact]
        public void AddWord_ShouldIncreaseCount()
        {
            var repository = new FileWordRepository();
            repository.Add(new Word("apple", new List<string> { "яблоко" }));

            Assert.Single(repository.GetAll());
        }

        [Fact]
        public void DeleteWord_ShouldRemoveWord()
        {
            var manager = new FileWordRepository(new List<Word> { new Word("apple", new List<string> { "яблоко" }) });
            manager.Delete("apple");

            Assert.Empty(manager.GetAll());
        }

        [Fact]
        public void FindWord_ShouldFindWord()
        {
            var repository = new FileWordRepository(new List<Word> { new Word("apple", new List<string> { "яблоко" }) });
            Assert.Equal("apple", repository.Find("apple").Text);
        }
    }
}
