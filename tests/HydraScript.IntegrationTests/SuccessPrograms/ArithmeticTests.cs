using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests.SuccessPrograms;

public class ArithmeticTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    /// <summary>
    /// https://github.com/Stepami/hydrascript/issues/232
    /// </summary>
    [Fact]
    public void Equality_AdditionToTheLeft_Success()
    {
        const string script =
            """
            let i = 0
            const s = "abcdef"
            const sLen = ~s
            while (i < sLen) {
                if (i + 1 == sLen)
                    >>> "i is 5"
                i = i + 1
            }
            """;
        using var runner = fixture.GetRunner(
            new TestHostFixture.Options(
                InMemoryScript: script));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.Success);
        fixture.LogMessages.Should()
            .Contain(log => log.Contains("i is 5"));
    }
}