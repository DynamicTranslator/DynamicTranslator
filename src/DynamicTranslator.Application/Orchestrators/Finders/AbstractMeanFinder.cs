using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Application.Orchestrators.Finders
{
    public abstract class AbstractMeanFinder<TConfiguration, TMeanOrganizer> : IMeanFinder, ITransientDependency
        where TConfiguration : ITranslatorConfiguration
        where TMeanOrganizer : IMeanOrganizer
    {
        public TConfiguration Configuration { get; set; }

        public TMeanOrganizer MeanOrganizer { get; set; }

        public virtual async Task<TranslateResult> FindMean(TranslateRequest translateRequest)
        {
            if (!Configuration.CanSupport())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            if (!Configuration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            return await Find(translateRequest);
        }

        protected abstract Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}
