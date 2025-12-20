using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace HydraScript.IntegrationTests;

public class InputTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void Invoke_Input_Success()
    {
        var console = Substitute.For<IConsole>();
        console.ReadLine().ReturnsForAnyArgs("1");

        using var runner = fixture.GetRunner(
            new TestHostFixture.Options(
                InMemoryScript: "<<< $SOME_NUMBER >>> $SOME_NUMBER"),
            services => services.Replace(ServiceDescriptor.Singleton(console)));

        var env = runner.ServiceProvider.GetRequiredService<IEnvironmentVariableProvider>();
        env.When(x => x.SetEnvironmentVariable("SOME_NUMBER", "1"))
            .Do(_ => env.GetEnvironmentVariable("SOME_NUMBER").Returns("1"));

        runner.Invoke();

        console.Received(1).WriteLine("1");
    }
}