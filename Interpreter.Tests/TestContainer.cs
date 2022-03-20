using System;
using Interpreter.MappingProfiles;
using Interpreter.Services;
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
            ServiceCollection.AddTransient<ILexerCreatorService, LexerCreatorService>();
            ServiceCollection.AddTransient<IParserCreatorService, ParserCreatorService>();

            ServiceCollection.AddAutoMapper(typeof(TokenTypeProfile));

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public T Get<T>() => ServiceProvider.GetService<T>();
    }
}