using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

public class TextCoordinateSystemComputerTests
{
    private ITextCoordinateSystemComputer _sut = new TextCoordinateSystemComputer();

    [Fact]
    public void GetLines_NoNewLine_SingleIndexResult()
    {
        const string text = "let x = 0";
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([-1, text.Length + Environment.NewLine.Length - 1]);
    }
    
    [Fact]
    public void GetLines_HasNewLine_SingleIndexResult()
    {
        var text = "let x = 0" + Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([-1, text.Length - 1]);
    }
    
    [Fact]
    public void GetLines_HasNewLines_MultipleIndicesResult()
    {
        const string stmt1 = "let x = 0";
        const string stmt2 = "x = x + 1";
        const string stmt3 = """print("x")""";
        var text = stmt1 +
                   Environment.NewLine +
                   stmt2 +
                   Environment.NewLine +
                   stmt3 +
                   Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo(
        [
            -1,
            stmt1.Length + Environment.NewLine.Length - 1,
            stmt1.Length + stmt2.Length + Environment.NewLine.Length * 2 - 1,
            stmt1.Length + stmt2.Length + stmt3.Length + Environment.NewLine.Length * 3 - 1
        ]);
    }

    [Theory, MemberData(nameof(EmptySystems))]
    public void GetCoordinates_EmptySystem_StartCoordinatesReturned(IReadOnlyList<int> newLineList)
    {
        var coord = _sut.GetCoordinates(
            absoluteIndex: Random.Shared.Next(0, int.MaxValue),
            newLineList);
        coord.Should().Be(new Coordinates());
    }

    public static TheoryData<IReadOnlyList<int>> EmptySystems =>
        new([[], [-1]]);

    [Fact]
    public void GetCoordinates_NotEmptySystem_CorrectCoordinatesReturned()
    {
        // abcd\nabc\naaaa\n
        //     4    8     13
        IReadOnlyList<int> newLineList = [-1, 4, 8, 13];
        // координата символа b
        var coord = _sut.GetCoordinates(absoluteIndex: 6, newLineList);
        coord.Should().Be(new Coordinates(Line: 2, Column: 2));
    }
}