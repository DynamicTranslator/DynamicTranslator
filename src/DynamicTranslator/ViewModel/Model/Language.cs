namespace DynamicTranslator.ViewModel.Model
{
    public class Language
    {
        public Language(string name,string extension)
        {
            this.Name = name;
            this.Extension = extension;
        }
        public string Extension { get; set; }

        public string Name { get; set; }
    }
}