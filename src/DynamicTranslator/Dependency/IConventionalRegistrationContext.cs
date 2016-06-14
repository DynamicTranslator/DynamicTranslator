using System.Reflection;

using DynamicTranslator.Dependency.Manager;

namespace DynamicTranslator.Dependency
{
    public interface IConventionalRegistrationContext
    {
        /// <summary>
        ///     Gets the registering Assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        ///     Registration configuration.
        /// </summary>
        ConventionalRegistrationConfig Config { get; }

        /// <summary>
        ///     Reference to the IOC Container to register types.
        /// </summary>
        IIocManager IocManager { get; }
    }
}