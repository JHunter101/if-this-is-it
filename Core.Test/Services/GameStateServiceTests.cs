using Core.Models;
using Core.Services;
using Moq;
using Shouldly;

namespace Core.Test.Services;

[TestClass]
public sealed class GameStateServiceTests
{
    private readonly Mock<IPromptRepository> _mockPromptRepository = new();
    private GameStateService _sut = null!;

    [TestInitialize]
    public void Initialize() => _sut = new GameStateService(_mockPromptRepository.Object);

    [TestMethod]
    public async Task StartNewGameAsync_WhenCalled_CreatesSessionAndFetchesFirstPrompt()
    {
        var settings = new GameSettings();
        var prompts = new SessionPrompts(["Q1"], [], []);
        _mockPromptRepository.Setup(r => r.GetPromptsAsync(settings)).ReturnsAsync(prompts);

        bool changed = false;
        _sut.OnChange += () => changed = true;
        await _sut.StartNewGameAsync(settings);

        _sut.Session.ShouldNotBeNull();
        _sut.Session.CurrentPrompt.ShouldBe("Q1");
        _sut.Session.GameState.ShouldBe(SessionState.InProgress);
        changed.ShouldBeTrue();
    }

    [TestMethod]
    public async Task NextPrompt_WhenSessionExists_AdvancesPromptAndInvokesOnChange()
    {
        var settings = new GameSettings();
        var prompts = new SessionPrompts(["Q1", "Q2"], [], []);
        _mockPromptRepository.Setup(r => r.GetPromptsAsync(settings)).ReturnsAsync(prompts);

        await _sut.StartNewGameAsync(settings);
        bool changed = false;
        _sut.OnChange += () => changed = true;
        _sut.NextPrompt();

        _sut.Session.ShouldNotBeNull();
        _sut.Session.CurrentPrompt.ShouldBe("Q2");
        changed.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ClearSession_WhenCalled_SetsSessionToNullAndInvokesOnChange()
    {
        var settings = new GameSettings();
        var prompts = new SessionPrompts(["Q1", "Q2"], [], []);
        _mockPromptRepository.Setup(r => r.GetPromptsAsync(settings)).ReturnsAsync(prompts);

        await _sut.StartNewGameAsync(settings);
        bool changed = false;
        _sut.OnChange += () => changed = true;
        _sut.ClearSession();

        _sut.Session.ShouldBeNull();
        changed.ShouldBeTrue();
    }
}