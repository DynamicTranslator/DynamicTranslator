namespace DynamicTranslator.Core.Extensions
{
    using System.Collections.Generic;
    using Configuration.UniqueIdentifier;

    public static class UniqueIdProviderExtensions
    {
        public static string BuildForAll(this IEnumerable<IUniqueIdentifierProvider> providers)
        {
            var uniqueId = string.Empty;

            providers.ForEach(provider => { uniqueId += provider.Get(); });

            return uniqueId;
        }
    }
}