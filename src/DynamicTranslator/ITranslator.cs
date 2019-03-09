using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Model;

namespace DynamicTranslator
{
    public interface ITranslator
    {
        TranslatorType Type { get; }
        Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken = default);
    }
}