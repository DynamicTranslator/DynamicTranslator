using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Core.Model;

namespace DynamicTranslator.Core
{
    public interface ITranslator
    {
        TranslatorType Type { get; }
        Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken = default);
    }
}