using HydraScript.Infrastructure;

namespace HydraScript.IntegrationTests;

public class SuccessfulProgramsTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory, MemberData(nameof(SuccessfulProgramsNames))]
    public void Invoke_NoError_ReturnCodeIsZero(string fileName)
    {
        var runner = fixture.GetRunner();
        var code = runner.Invoke([$"Samples/{fileName}"]);
        code.Should().Be(Executor.ExitCodes.Success);
    }

    public static TheoryData<string> SuccessfulProgramsNames =>
        new(
        [
            "abs.js",
            "arraddremove.js",
            "arreditread.js",
            "ceil.js",
            "cycled.js",
            "defaultarray.js",
            "equals.js",
            "exprtest.js",
            "fastpow.js",
            "forwardref.js",
            "gcd.js",
            "lcm.js",
            "linkedlist.js",
            "objeditread.js",
            "overload.js",
            "overload_object.js",
            "posneg.js",
            "prime.js",
            "primefactor.js",
            "quicksort.js",
            "range.js",
            "recur.js",
            "scope.js",
            "searchinll.js",
            "settable.js",
            "squareroot.js",
            "summator.js",
            "tern.js",
            "this.js",
            "typeresolving.js",
            "vec2d.js",
            "xxx.js"
        ]);
}