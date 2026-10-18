namespace Specifications.StepDefinitions;

[Binding]
[Scope(Feature = "Initialization")]
public sealed class InitializationSteps(BlazorTestContext context, IGameStateService stateService) : CommonSteps(context, stateService)
{
    [Given("I am on the home screen")]
    public void GivenIAmOnTheHomeScreen() => StateService.ClearSession();

    [When("I click the 'are we really strangers?' button")]
    public void WhenIClickTheAreWeReallyStrangersButton() => ClickButton("are we really strangers?");

    [Then("the game play view should be visible")]
    public void ThenTheGamePlayViewShouldBeVisible() => AssertComponentExists<GamePlayView>();
}