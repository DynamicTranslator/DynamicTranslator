using System.Collections.Generic;
using DynamicTranslator.Configuration.UniqueIdentifier;

namespace DynamicTranslator.Extensions
{
    public static class UniqueIdProviderExtensions
    {
        public static string BuildForAll(this IEnumerable<IUniqueIdentifierProvider> providers)
        {
            string uniqueId = string.Empty;

            providers.ForEach(provider => { uniqueId += provider.Get(); });

            return uniqueId;
        }
    }
}