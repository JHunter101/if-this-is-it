using Bunit;
using Reqnroll.BoDi;

namespace Specifications;

[Binding]
public sealed class Hooks(IObjectContainer container)
{
    [BeforeScenario]
    public void BeforeScenario()
    {
        var promptRepository = new PromptRepositoryStub();
        container.RegisterInstanceAs<IPromptRepository>(promptRepository);
        container.RegisterInstanceAs<PromptRepositoryStub>(promptRepository);

        container.RegisterTypeAs<GameStateService, IGameStateService>();

        IGameStateService gameStateService = container.Resolve<IGameStateService>();
        var blazorTestContext = new BlazorTestContext(gameStateService);

        BunitServiceProvider services = blazorTestContext.BunitContext.Services;
        services.AddSingleton<IPromptRepository>(promptRepository);
        services.AddSingleton<IGameStateService>(gameStateService);

        blazorTestContext.BunitContext.JSInterop.Mode = JSRuntimeMode.Loose;

        container.RegisterInstanceAs(blazorTestContext);
    }

    [AfterScenario]
    public static void AfterScenario(BlazorTestContext context)
        => context.Dispose();
}