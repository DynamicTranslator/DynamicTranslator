namespace DynamicTranslator.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Model;

    public interface ITranslator
    {
        TranslatorType Type { get; }
        Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken = default);
    }
}