namespace Dynamic.Translator.Core.Dependency.Manager
{
    #region using

    using System;
    using Castle.Windsor;

    #endregion

    public interface IIocManager : IIocRegistrar, IIocResolver, IDisposable
    {
        IWindsorContainer IocContainer { get; }

        new bool IsRegistered(Type type);
        new bool IsRegistered<T>();
    }
}