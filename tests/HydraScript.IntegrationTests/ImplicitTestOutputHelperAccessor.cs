using MartinCostello.Logging.XUnit;

namespace HydraScript.IntegrationTests;

internal sealed class ImplicitTestOutputHelperAccessor : ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper
    {
        get => TestContext.Current.TestOutputHelper;
        set { }
    }
}