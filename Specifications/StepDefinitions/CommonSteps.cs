using Beauo.Components;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace Specifications.StepDefinitions;

public abstract class CommonSteps(BlazorTestContext context, IGameStateService stateService)
{
    protected BlazorTestContext Context { get; } = context;
    protected IGameStateService StateService { get; } = stateService;
    protected IRenderedComponent<Home> RenderedPage => Context.RenderedPage;
    protected PromptRepositoryStub Repository => (PromptRepositoryStub)Context.BunitContext.Services.GetRequiredService<IPromptRepository>();

    [Then("I should see the main menu")]
    public void ThenIShouldSeeTheMainMenu() => AssertComponentExists<MainMenu>();

    [Then("the current prompt state should be '{PromptDepth}'")]
    public void ThenTheCurrentPromptStateShouldBe(PromptDepth state) => AssertPromptDepth(state);

    protected void ClickButton(string text)
    {
        IRenderedComponent<Button> button = RenderedPage
            .FindComponents<Button>()
            .Single(c => c.Instance.Text.Equals(text, StringComparison.OrdinalIgnoreCase));

        button.Find("button").Click();
    }

    protected void ClickPromptCard()
    {
        IRenderedComponent<PromptDisplay> promptDisplay = RenderedPage.FindComponent<PromptDisplay>();
        promptDisplay.Find(".prompt-card").Click();
    }

    protected void ClickPromptCard(int times)
    {
        for (int i = 0; i < times; i++)
        {
            ClickPromptCard();
        }
    }

    protected void AssertComponentExists<TComponent>() where TComponent : IComponent => RenderedPage.WaitForComponent<TComponent>().ShouldNotBeNull();

    protected void AssertSessionState(SessionState expectedState)
    {
        StateService.Session.ShouldNotBeNull();
        StateService.Session.GameState.ShouldBe(expectedState);
    }

    protected void AssertPromptDepth(PromptDepth expectedDepth)
    {
        StateService.Session.ShouldNotBeNull();
        StateService.Session.PromptState.ShouldBe(expectedDepth);
    }
}