using CommunityToolkit.Diagnostics;
using Core.Models;

namespace Core.Services;

public interface IGameStateService
{
    event Action? OnChange;

    GameSession? Session { get; }

    Task StartNewGameAsync(GameSettings? settings = null);

    void NextPrompt();

    void ClearSession();
}

public class GameStateService(IPromptRepository promptRepository) : IGameStateService
{
    public event Action? OnChange;

    public GameSession? Session { get; private set; }

    public async Task StartNewGameAsync(GameSettings? settings = null)
    {
        settings ??= Session?.Settings;
        Guard.IsNotNull(settings);
        SessionPrompts prompts = await promptRepository.GetPromptsAsync(settings);

        Session = new GameSession(settings, prompts);
        NextPrompt();
    }

    public void NextPrompt()
    {
        Session?.Next();
        OnChange?.Invoke();
    }

    public void ClearSession()
    {
        Session = null;
        OnChange?.Invoke();
    }
}