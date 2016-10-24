namespace DynamicTranslator.LanguageManagement
{
    public class Language
    {
        public Language(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        public string Extension { get; set; }

        public string Name { get; set; }
    }
}
