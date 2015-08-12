namespace Dynamic.Translator.Core.Dependency
{
    #region using

    using System;

    #endregion

    public interface IIocResolver
    {
        T Resolve<T>();
        T Resolve<T>(object argumentsAsAnonymousType);
        object Resolve(Type type);
        object Resolve(Type type, object argumentsAsAnonymousType);
        void Release(object obj);
        bool IsRegistered(Type type);
        bool IsRegistered<T>();
    }
}