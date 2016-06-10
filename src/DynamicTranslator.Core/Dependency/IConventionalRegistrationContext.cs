using System.Reflection;

using DynamicTranslator.Core.Dependency.Manager;

namespace DynamicTranslator.Core.Dependency
{
    #region using

    

    #endregion

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