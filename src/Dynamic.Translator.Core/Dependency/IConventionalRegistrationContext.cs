namespace DynamicTranslator.Core.Dependency
{
    #region using

    using System.Reflection;
    using Manager;

    #endregion

    public interface IConventionalRegistrationContext
    {
        /// <summary>
        ///     Gets the registering Assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        ///     Reference to the IOC Container to register types.
        /// </summary>
        IIocManager IocManager { get; }

        /// <summary>
        ///     Registration configuration.
        /// </summary>
        ConventionalRegistrationConfig Config { get; }
    }
}