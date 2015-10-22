namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class SesliSozlukMeanOrganizer : IMeanOrganizer, ITransientDependency
    {
        public TranslatorType TranslatorType => TranslatorType.SESLISOZLUK;

        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            throw new NotImplementedException();
        }
    }
}