using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests;

public class SuccessfulProgramsTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory]
    [ClassData(typeof(SuccessfulPrograms))]
    public void Invoke_NoError_ReturnCodeIsZero(string relativePathToFile)
    {
        using var runner = fixture.GetRunner(
            new TestHostFixture.Options(
                FileName: relativePathToFile,
                MockFileSystem: false));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.Success);
    }

    public class SuccessfulPrograms : TheoryData<string>
    {
        public SuccessfulPrograms()
        {
            AddRange(Directory.GetFiles("Samples"));
        }
    }
}