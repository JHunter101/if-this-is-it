using Core.Models;
using Shouldly;

namespace Core.Test.Models;

[TestClass]
public sealed class SessionPromptsTests
{
    [TestMethod]
    public void Next_WhenStrangersHasPrompts_ReturnsPromptAndKeepsLevel()
    {
        SessionPrompts sut = Sut(strangers: ["Q1", "Q2"]);
        sut.Next().ShouldBe("Q1");
        sut.CurrentLevel.ShouldBe(PromptDepth.Strangers);
    }

    [TestMethod]
    public void Next_WhenStrangersEmpty_AdvancesToInterlinked()
    {
        SessionPrompts sut = Sut(strangers: [], interlinked: ["Q3"]);
        sut.Next().ShouldBe("Q3");
        sut.CurrentLevel.ShouldBe(PromptDepth.Interlinked);
    }

    [TestMethod]
    public void Next_WhenEarlierLevelsEmpty_ReturnsIntimatePromptAndSetsLevelToIntimate()
    {
        SessionPrompts sut = Sut(strangers: [], interlinked: [], intimate: ["Q1"]);
        sut.Next().ShouldBe("Q1");
        sut.CurrentLevel.ShouldBe(PromptDepth.Intimate);
    }

    [TestMethod]
    public void Next_WhenAllLevelsEmpty_ReturnsNullAndKeepsLevelAtIntimate()
    {
        SessionPrompts sut = Sut(strangers: [], interlinked: [], intimate: []);
        sut.Next().ShouldBeNull();
        sut.CurrentLevel.ShouldBe(PromptDepth.Intimate);
        sut.Next().ShouldBeNull();
        sut.CurrentLevel.ShouldBe(PromptDepth.Intimate);
    }

    [TestMethod]
    public void GetQuestionsLeft_ReturnsCorrectCountForLevel()
    {
        SessionPrompts sut = Sut(strangers: ["Q1", "Q2"], interlinked: ["Q3", "Q4", "Q5"], intimate: []);
        sut.GetQuestionsLeft(PromptDepth.Strangers).ShouldBe(2);
        sut.GetQuestionsLeft(PromptDepth.Interlinked).ShouldBe(3);
        sut.GetQuestionsLeft(PromptDepth.Intimate).ShouldBe(0);
    }

    private static SessionPrompts Sut(
        IEnumerable<string>? strangers = null,
        IEnumerable<string>? interlinked = null,
        IEnumerable<string>? intimate = null)
    {
        return new SessionPrompts(
            strangers ?? [],
            interlinked ?? [],
            intimate ?? []);
    }
}
