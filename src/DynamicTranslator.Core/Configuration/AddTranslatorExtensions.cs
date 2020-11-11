namespace DynamicTranslator.Core.Configuration
{
    using System;
    using Google;
    using Microsoft.Extensions.DependencyInjection;
    using Prompt;
    using SesliSozluk;
    using Tureng;

    public static class AddTranslatorExtensions
    {
        public static IServiceCollection AddGoogleTranslator(this IServiceCollection services,
            Action<GoogleTranslatorConfiguration> configure)
        {
            services.AddTransient<ITranslator, GoogleTranslator>();
            services.AddSingleton(sp =>
            {
                var conf = new GoogleTranslatorConfiguration(
                    sp.GetService<ActiveTranslatorConfiguration>(),
                    sp.GetService<IApplicationConfiguration>()
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