namespace Specifications.StepDefinitions;

[Binding]
[Scope(Feature = "Game Flow")]
public sealed class GameFlowSteps(BlazorTestContext context, IGameStateService stateService) : CommonSteps(context, stateService)
{
    [Given("I have started a game")]
    public async Task GivenIHaveStartedAGame()
    {
        Repository.SetPrompts(["S1", "S2"], ["I1", "I2"], ["C1", "C2"]);
        await StateService.StartNewGameAsync(new GameSettings());
    }

    [Given("I have started a game with 1 question per level")]
    public async Task GivenIHaveStartedAGameWith1QuestionPerLevel()
    {
        Repository.SetPrompts(["S1"], ["I1"], ["C1"]);
        await StateService.StartNewGameAsync(new GameSettings());
    }

    [Given("I have reached the game over screen")]
    public async Task GivenIHaveReachedTheGameOverScreen()
    {
        Repository.SetPrompts(["S1"], ["I1"], ["C1"]);
        await StateService.StartNewGameAsync(new GameSettings());

        await ClickPromptCard(3);
        AssertSessionState(SessionState.Over);
    }

    [When("I click the prompt card")]
    public async Task WhenIClickThePromptCardAsync() => await ClickPromptCard();

    [When("I click the prompt card {int} times")]
    public async Task WhenIClickThePromptCardTimesAsync(int count) => await ClickPromptCard(count);

    [When("I click the 'main menu' button")]
    public void WhenIClickTheMainMenuButton() => ClickButton("main menu");

    [Then("I should see the game over screen")]
    public void ThenIShouldSeeTheGameOverScreen() => AssertSessionState(SessionState.Over);
}