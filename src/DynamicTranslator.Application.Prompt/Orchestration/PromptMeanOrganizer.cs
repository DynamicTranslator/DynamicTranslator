using System.Threading.Tasks;

using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Application.Prompt.Orchestration
{
    public class PromptMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Prompt;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            var promptResult = text.DeserializeAs<PromptResult>();
            return Task.FromResult(new Maybe<string>(promptResult.d.result));
        }
    }
}
