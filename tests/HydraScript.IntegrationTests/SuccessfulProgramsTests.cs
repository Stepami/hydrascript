using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests;

public class SuccessfulProgramsTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory]
    [ClassData(typeof(SamplesScriptsData))]
    public void Invoke_NoError_ReturnCodeIsZero(string relativePathToFile)
    {
        var runner = fixture.GetRunner(
            new TestHostFixture.Options(
                FileName: relativePathToFile,
                MockFileSystem: false));
        var code = runner.Invoke();
        code.Should().Be(Executor.ExitCodes.Success);
    }

    public class SamplesScriptsData : TheoryData<string>
    {
        public SamplesScriptsData()
        {
            AddRange(Directory.GetFiles("Samples"));
        }
    }
}