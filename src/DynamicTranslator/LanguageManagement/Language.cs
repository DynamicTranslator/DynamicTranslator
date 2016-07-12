namespace DynamicTranslator.LanguageManagement
{
    public class Language
    {
        public string Extension { get; set; }

        public string Name { get; set; }

        public Language(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
    }
}