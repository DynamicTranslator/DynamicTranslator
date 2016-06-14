using System;

namespace DynamicTranslator.Dependency.Manager
{
    public interface IIocResolver
    {
        bool IsRegistered(Type type);

        bool IsRegistered<T>();

        void Release(object obj);

        T Resolve<T>();

        T Resolve<T>(object argumentsAsAnonymousType);

        object Resolve(Type type);

        object Resolve(Type type, object argumentsAsAnonymousType);
    }
}