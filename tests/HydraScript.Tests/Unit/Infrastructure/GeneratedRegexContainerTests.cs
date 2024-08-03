using HydraScript.Infrastructure;
using Xunit;

namespace HydraScript.Tests.Unit.Infrastructure;

public class GeneratedRegexContainerTests
{
    [Fact]
    public void GetRegex_Generated_ManualIsUpToDate() =>
        GeneratedRegexContainer.Pattern.Trim().Should().Be(
            GeneratedRegexContainer.GetRegex().ToString(),
            "because В атрибут GeneratedRegex не подставлена актуальная сгенерированная регулярка");
}