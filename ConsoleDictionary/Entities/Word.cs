namespace ConsoleDictionary.Entities
{
    public class Word
    {
        public string Text { get; set; }
        public List<string> Translations { get; set; } = new();
        public string Category { get; set; }
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
                   "\nTranslations:" + string.Join(",\n ", Translations) +
                   "\nCategory:" + Category;
        }
    }
}