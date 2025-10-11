namespace ConsoleDictionary.Entities
{
    public record class Word
    {
        public string Text { get; init; }
        public List<string> Translations { get; init; } = new();
        public string Category { get; init; } = "";
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }

        public Word(string text, List<string> translations, string category = "")
        {
            Text = text;
            Translations = translations;
            Category = category;
            CorrectCount = 0;
            WrongCount = 0;
        }

        public override string ToString()
        {
            return "\nText: " + Text +
                   "\nTranslations:" + string.Join(", ", Translations) +
                   "\nCategory:" + Category;
        }
    }
}