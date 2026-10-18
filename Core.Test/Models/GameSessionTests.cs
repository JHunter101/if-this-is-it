using Core.Models;
using Shouldly;

namespace Core.Test.Models;

[TestClass]
public sealed class GameSessionTests
{
    [TestMethod]
    public void NextPrompt_WhenPromptAvailable_SetsCurrentPrompt()
    {
        GameSession sut = Sut(strangers: ["Q1"]);
        sut.NextPrompt();
        sut.CurrentPrompt.ShouldBe("Q1");
        sut.GameState.ShouldBe(SessionState.InProgress);
    }

    [TestMethod]
    public void NextPrompt_WhenNoPromptsLeft_SetsStateToOver()
    {
        GameSession sut = Sut();
        sut.NextPrompt();
        sut.CurrentPrompt.ShouldBeNull();
        sut.GameState.ShouldBe(SessionState.Over);
    }

    [TestMethod]
    public void GetProgress_WhenAtStartOfLevel_ReturnsZero()
    {
        GameSession sut = Sut(strangers: ["Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7", "Q8", "Q9", "Q10"]);
        sut.GetProgress(PromptDepth.Strangers).ShouldBe(0.0);
    }

    [TestMethod]
    public void GetProgress_WhenHalfwayThroughLevel_ReturnsCorrectFraction()
    {
        GameSession sut = Sut(strangers: ["Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7", "Q8", "Q9", "Q10"]);

        for (int i = 0; i <= 5; i++)
        {
            sut.NextPrompt();
        }

        sut.GetProgress(PromptDepth.Strangers).ShouldBe(0.5);
    }

    [TestMethod]
    public void GetProgress_WhenLevelCompleted_ReturnsOne()
    {
        GameSession sut = Sut(
            strangers: ["Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7", "Q8", "Q9", "Q10"],
            interlinked: ["Q11", "Q12", "Q13", "Q14", "Q15", "Q16", "Q17", "Q18", "Q19", "Q20"]);

        for (int i = 0; i <= 10; i++)
        {
            sut.NextPrompt();
        }

        sut.GetProgress(PromptDepth.Strangers).ShouldBe(1.0);
        sut.GetProgress(PromptDepth.Interlinked).ShouldBe(0.0);
    }

    private static GameSession Sut(
        IEnumerable<string>? strangers = null,
        IEnumerable<string>? interlinked = null,
        IEnumerable<string>? intimate = null)
    {
        var settings = new GameSettings();
        var prompts = new SessionPrompts(
            strangers ?? [],
            interlinked ?? [],
            intimate ?? []);

        return new GameSession(settings, prompts);
    }
}