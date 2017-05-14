using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Requests;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application.Orchestrators.Finders
{
    public abstract class AbstractMeanFinder : IMeanFinder, ITransientDependency
    {
        public abstract Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}
