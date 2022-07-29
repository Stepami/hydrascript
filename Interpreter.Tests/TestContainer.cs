using System;
using Interpreter.MappingProfiles;
using Interpreter.Services.Providers;
using Interpreter.Services.Providers.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Interpreter.Tests
{
    internal class TestContainer
    {
        private IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        
        private IServiceProvider ServiceProvider { get; set; }
        
        public TestContainer()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            ServiceCollection.AddTransient<ILexerProvider, LexerProvider>();
            ServiceCollection.AddTransient<IParserProvider, ParserProvider>();

            ServiceCollection.AddAutoMapper(typeof(TokenTypeProfile));

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public T Get<T>() => ServiceProvider.GetService<T>();
    }
}