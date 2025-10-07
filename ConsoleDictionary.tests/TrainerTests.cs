using ConsoleDictionary.Entities;
using ConsoleDictionary.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDictionary.tests
{
    public class TrainerTests
    {
        [Fact]
        public void CheckAnswer_ShouldReturnTrue_IfTranslationMatches()
        {
            var word = new Word("apple", new List<string> { "яблоко" });
            var manager = new DictionaryManager(new List<Word> { word });
            var trainer = new Trainer(manager);

            bool result = trainer.CheckAnswer(word, "яблоко");

            Assert.True(result);
            Assert.Equal(1, word.CorrectCount);
        }
    }
}
