using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Application.Prompt
{
    public class PromptMeanOrganizer : AbstractMeanOrganizer, IMeanOrganizer, ITransientDependency
    {
        public override TranslatorType TranslatorType => TranslatorType.Prompt;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            var promptResult = text.DeserializeAs<PromptResult>();
            return Task.FromResult(new Maybe<string>(promptResult.d.result));
        }
    }
}
