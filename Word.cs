namespace Entities
{
    class Word
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public string Category { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
    }
}