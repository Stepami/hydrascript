using MartinCostello.Logging.XUnit;

namespace HydraScript.IntegrationTests;

public class ImplicitTestOutputHelperAccessor : ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper
    {
        get => TestContext.Current.TestOutputHelper;
        set { }
    }
}