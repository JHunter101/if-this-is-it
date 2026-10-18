using System.Globalization;

namespace Test.Shared;

[TestClass]
public sealed class TestAssembly
{
    [AssemblyInitialize]
    public static void Initialize(TestContext testContext)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    }
}