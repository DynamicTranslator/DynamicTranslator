namespace Dynamic.Tureng.Translator.Orchestrators.Organizers
{
    using System;
    using System.Threading.Tasks;
    using Dynamic.Translator.Core;
    using Dynamic.Translator.Core.Dependency.Markers;
    using Dynamic.Translator.Core.Orchestrators;
    using Dynamic.Translator.Core.ViewModel.Constants;

    public class SesliSozlukMeanOrganizer : IMeanOrganizer, ITransientDependency
    {
        public TranslatorType TranslatorType => TranslatorType.SESLISOZLUK;

        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            throw new NotImplementedException();
        }
    }
}