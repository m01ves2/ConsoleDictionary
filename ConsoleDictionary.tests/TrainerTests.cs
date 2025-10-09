using ConsoleDictionary.Entities;
using ConsoleDictionary.Managers;
using ConsoleDictionary.Repositories;

namespace ConsoleDictionary.tests
{
    public class TrainerTests
    {
        [Fact]
        public void CheckAnswer_ShouldReturnTrue_IfTranslationMatches()
        {
            var word = new Word("apple", new List<string> { "яблоко" });
            var repository = new FileWordRepository(new List<Word> { word });
            var trainer = new Trainer(repository);

            bool result = trainer.CheckAnswer(word, "яблоко");

            Assert.True(result);
            Assert.Equal(1, word.CorrectCount);
        }
    }
}
