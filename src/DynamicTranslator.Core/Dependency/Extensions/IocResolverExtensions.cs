namespace DynamicTranslator.Core.Dependency.Extensions
{
    #region using

    using System;
    using Manager;

    #endregion

    public static class IocResolverExtensions
    {
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, iocResolver.Resolve<T>());
        }

        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, Type type)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, (T) iocResolver.Resolve(type));
        }

        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocResolver iocResolver, Type type)
        {
            return new DisposableDependencyObjectWrapperOfT(iocResolver, iocResolver.Resolve(type));
        }

        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, iocResolver.Resolve<T>(argumentsAsAnonymousType));
        }

        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, (T) iocResolver.Resolve(type, argumentsAsAnonymousType));
        }

        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocResolver iocResolver, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapperOfT(iocResolver, iocResolver.Resolve(type, argumentsAsAnonymousType));
        }

        public static void Using<T>(this IIocResolver iocResolver, Action<T> action)
        {
            using (var wrapper = new DisposableDependencyObjectWrapper<T>(iocResolver, iocResolver.Resolve<T>()))
                action(wrapper.Object);
        }
    }
}