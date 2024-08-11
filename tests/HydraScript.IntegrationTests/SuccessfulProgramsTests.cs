using System.CommandLine.Parsing;
using FluentAssertions;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests;

public class SuccessfulProgramsTests(
    TestHostFixture fixture,
    ITestOutputHelper testOutputHelper) : IClassFixture<TestHostFixture>
{
    [Theory, MemberData(nameof(SuccessfulProgramsNames))]
    public void Invoke_NoError_ReturnCodeIsZero(string fileName)
    {
        var runner = fixture.GetRunner(testOutputHelper);
        var code = runner.Invoke([$"Samples/{fileName}"]);
        testOutputHelper.WriteLine(fixture.Writer.ToString());
        code.Should().Be(0);
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
            "posneg.js",
            "prime.js",
            "primefactor.js",
            "quicksort.js",
            "range.js",
            "recur.js",
            "searchinll.js",
            "settable.js",
            "squareroot.js",
            "summator.js",
            "tern.js",
            "this.js",
            "typeresolving.js",
            "vec2d.js",
        ]);
}