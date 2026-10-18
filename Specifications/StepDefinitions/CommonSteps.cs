using Beauo.Components;
using Bunit;

namespace Specifications.StepDefinitions;

public abstract class CommonSteps(BlazorTestContext context, IGameStateService stateService)
{
    protected BlazorTestContext Context { get; } = context;
    protected IGameStateService StateService { get; } = stateService;
    protected IRenderedComponent<Home> RenderedPage => Context.RenderedPage;
    protected PromptRepositoryStub Repository => (PromptRepositoryStub)Context.BunitContext.Services.GetRequiredService<IPromptRepository>();

    [Then("I should see the main menu")]
    public void ThenIShouldSeeTheMainMenu() => AssertSessionState(SessionState.MainMenu);

    [Then("the current prompt state should be '{PromptDepth}'")]
    public void ThenTheCurrentPromptStateShouldBe(PromptDepth state) => AssertPromptDepth(state);

    protected void ClickButton(string text)
    {
        IRenderedComponent<Button> button = RenderedPage
            .FindComponents<Button>()
            .Single(c => c.Instance.Text.Equals(text, StringComparison.OrdinalIgnoreCase));

        button.Find("button").Click();
    }

    protected async Task ClickPromptCard() => StateService.NextPrompt();

    protected async Task ClickPromptCard(int times)
    {
        for (int i = 0; i < times; i++)
        {
            await ClickPromptCard();
        }
    }

    protected void AssertSessionState(SessionState expectedState)
    {
        if (expectedState == SessionState.MainMenu)
        {
            StateService.Session.ShouldBeNull();
            return;
        }

        StateService.Session.ShouldNotBeNull();
        StateService.Session.GameState.ShouldBe(expectedState);
    }

    protected void AssertPromptDepth(PromptDepth expectedDepth)
    {
        StateService.Session.ShouldNotBeNull();
        StateService.Session.PromptState.ShouldBe(expectedDepth);
    }
}