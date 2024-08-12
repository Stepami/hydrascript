using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace HydraScript.IntegrationTests;

internal static class ServiceCollectionTestExtensions
{
    internal static void SetupInMemoryScript(this IServiceCollection services, string script)
    {
        var fileSystem = Substitute.For<IFileSystem>();
        var file = Substitute.For<IFile>();
        file.ReadAllText(default!).ReturnsForAnyArgs(script);
        fileSystem.File.ReturnsForAnyArgs(file);
        services.AddSingleton(fileSystem);
    }
}