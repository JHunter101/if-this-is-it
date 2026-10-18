using Bunit;

namespace Specifications;

public sealed class BlazorTestContext(IGameStateService gameStateService) : IDisposable
{
    public BunitContext BunitContext => field ??= new BunitContext();
    public GameSession? Session => gameStateService.Session;

    public IRenderedComponent<Home> RenderedPage
        => field ??= BunitContext.Render<Home>();

    public void Dispose() => BunitContext.Dispose();
}