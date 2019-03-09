using System;
using DynamicTranslator.Google;
using DynamicTranslator.Prompt;
using DynamicTranslator.SesliSozluk;
using DynamicTranslator.Tureng;
using DynamicTranslator.Yandex;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTranslator.Configuration
{
    public static class AddTranslatorExtensions
    {
        public static IServiceCollection AddGoogleTranslator(this IServiceCollection services,
            Action<GoogleTranslatorConfiguration> configure)
        {
            services.AddSingleton<ITranslator, GoogleTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new GoogleTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<ApplicationConfiguration>()
                );

                configure?.Invoke(conf);
                return conf;
            });
            return services;
        }

        public static IServiceCollection AddYandexTranslator(this IServiceCollection services,
            Action<YandexTranslatorConfiguration> configure)
        {
            services.AddTransient<ITranslator, YandexTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new YandexTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<ApplicationConfiguration>()
                );

                configure?.Invoke(conf);
                return conf;
            });
            return services;
        }

        public static IServiceCollection AddSesliSozlukTranslator(this IServiceCollection services,
            Action<SesliSozlukTranslatorConfiguration> configure)
        {
            services.AddTransient<ITranslator, SesliSozlukTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new SesliSozlukTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<ApplicationConfiguration>()
                );

                configure?.Invoke(conf);
                return conf;
            });
            return services;
        }

        public static IServiceCollection AddTurengTranslator(this IServiceCollection services,
            Action<TurengTranslatorConfiguration> configure)
        {
            services.AddTransient<ITranslator, TurengTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new TurengTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<ApplicationConfiguration>()
                );

                configure?.Invoke(conf);
                return conf;
            });
            return services;
        }

        public static IServiceCollection AddPromptTranslator(this IServiceCollection services,
            Action<PromptTranslatorConfiguration> configure)
        {
            services.AddTransient<ITranslator, PromptTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new PromptTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<ApplicationConfiguration>()
                );

                configure?.Invoke(conf);
                return conf;
            });
            return services;
        }
    }
}