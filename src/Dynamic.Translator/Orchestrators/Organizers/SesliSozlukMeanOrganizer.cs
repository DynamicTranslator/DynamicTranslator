namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using HtmlAgilityPack;

    public class SesliSozlukMeanOrganizer : IMeanOrganizer
    {
        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            return await Task.Run(() =>
            {
                var output = new StringBuilder();

                var document = new HtmlDocument();
                document.LoadHtml(text);

                (from x in document.DocumentNode.Descendants()
                    where x.Name == "pre"
                    from y in x.Descendants()
                    where y.Name == "ol"
                    from z in y.Descendants()
                    where z.Name == "li"
                    select z.InnerHtml).AsParallel().ToList().ForEach(mean => output.AppendLine(mean));

                if (string.IsNullOrEmpty(output.ToString()))
                {
                    (from x in document.DocumentNode.Descendants()
                        where x.Name == "pre"
                        from y in x.Descendants()
                        where y.Name == "span"
                        select y.InnerHtml).AsParallel().ToList().ForEach(mean => output.AppendLine(mean));
                }

                return new Maybe<string>(output.ToString());
            });
        }

        public TranslatorType TranslatorType => TranslatorType.SESLISOZLUK;
    }
}