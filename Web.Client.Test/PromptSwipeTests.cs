using Bunit;
using Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Web.Client.Components;

namespace Web.Client.Test;

[TestClass]
public sealed class PromptDisplayTests : BunitContext
{
    [TestMethod]
    public async Task WhenDraggingCard_ThenTransformUpdates()
    {
        SetupJsInterop();
        IRenderedComponent<PromptDisplay> cut = RenderSut();

        await PointerDown(cut, 100, 100);
        await PointerMove(cut, 150, 120);

        string style = cut.Find(".current-card").GetAttribute("style").ShouldNotBeNull();

        style!.ShouldContain("translate(50px, 20px)");
        style!.ShouldContain("rotate(2,5deg)");
        style!.ShouldContain("transition: none");
    }

    [TestMethod]
    public async Task WhenReleasedBelowThreshold_ThenCardResetsWithoutAdvancing()
    {
        SetupJsInterop();
        CallbackSpy onNextPrompt = SubstituteCallback();
        IRenderedComponent<PromptDisplay> cut = RenderSut(onNextPrompt);

        await PointerDown(cut);
        await PointerMove(cut, 100, 0);
        await PointerUp(cut, 100, 0);

        cut.Find(".current-card")
            .GetAttribute("style").ShouldNotBeNull()
            .ShouldContain("translate(0px, 0px)");

        onNextPrompt.CallCount.ShouldBe(0);
    }

    [TestMethod]
    public async Task WhenReleasedAboveThreshold_ThenNextPromptIsInvoked()
    {
        SetupJsInterop();
        CallbackSpy onNextPrompt = SubstituteCallback();
        IRenderedComponent<PromptDisplay> cut = RenderSut(onNextPrompt);

        await PointerDown(cut);
        await PointerMove(cut, 450, 0);
        await PointerUp(cut, 450, 0);

        onNextPrompt.CallCount.ShouldBe(1);

        cut.Find(".current-card")
            .GetAttribute("style").ShouldNotBeNull()
            .ShouldContain("translate(0px, 0px)");
    }

    [TestMethod]
    public async Task WhenDragIsCancelled_ThenCardResetsWithoutAdvancing()
    {
        SetupJsInterop();
        CallbackSpy onNextPrompt = SubstituteCallback();
        IRenderedComponent<PromptDisplay> cut = RenderSut(onNextPrompt);

        await PointerDown(cut);
        await PointerMove(cut, 150, 100);
        await PointerCancel(cut);

        cut.Find(".current-card")
            .GetAttribute("style").ShouldNotBeNull()
            .ShouldContain("translate(0px, 0px)");

        onNextPrompt.CallCount.ShouldBe(0);
    }

    private void SetupJsInterop()
        => JSInterop.Mode = JSRuntimeMode.Loose;

    private IRenderedComponent<PromptDisplay> RenderSut(
        Action? onNextPrompt = null)
        => Render<PromptDisplay>(parameters => parameters
            .Add(p => p.State, PromptDepth.Strangers)
            .Add(p => p.Prompt, "Current prompt")
            .Add(p => p.NextPrompt, "Next prompt")
            .Add(p => p.OnNextPrompt,
                EventCallback.Factory.Create(
                    this, onNextPrompt ?? (() => { }))));

    private static PointerEventArgs Pointer(double x, double y)
        => new()
        {
            ClientX = x,
            ClientY = y,
            PointerId = 1
        };

    private static Task PointerDown(IRenderedComponent<PromptDisplay> cut, double x = 0, double y = 0)
        => cut.Find(".prompt-card-container")
            .TriggerEventAsync("onpointerdown", Pointer(x, y));

    private static Task PointerMove(IRenderedComponent<PromptDisplay> cut, double x, double y)
        => cut.Find(".prompt-card-container")
            .TriggerEventAsync("onpointermove", Pointer(x, y));

    private static Task PointerUp(IRenderedComponent<PromptDisplay> cut, double x, double y)
        => cut.Find(".prompt-card-container")
            .TriggerEventAsync("onpointerup", Pointer(x, y));

    private static Task PointerCancel(IRenderedComponent<PromptDisplay> cut)
        => cut.Find(".prompt-card-container")
            .TriggerEventAsync("onpointercancel", Pointer(0, 0));

    private static CallbackSpy SubstituteCallback() => new();

    private sealed class CallbackSpy
    {
        public int CallCount { get; private set; }

        public void Invoke() => CallCount++;

        public static implicit operator Action(CallbackSpy spy)
            => spy.Invoke;
    }
}