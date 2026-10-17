namespace Core.Models;

public class GameSession(GameSettings settings, SessionPrompts prompts)
{
    private readonly SessionPrompts _prompts = prompts;
    public GameSettings Settings { get; } = settings;
    public string? CurrentPrompt { get; private set; }

    public SessionState GameState { get; private set; } = SessionState.InProgress;
    public PromptDepth PromptState => _prompts.CurrentLevel;

    internal void NextPrompt()
    {
        string? next = _prompts.Next();

        if (next is not null)
        {
            CurrentPrompt = next;
        }
        else
        {
            GameState = SessionState.Over;
        }
    }

    public double GetProgress(PromptDepth promptDepth)
    {
        if (Settings.QuestionsPerLevel == 0)
        {
            return 0;
        }

        double questionsLeft = _prompts.GetQuestionsLeft(promptDepth);

        if (PromptState == promptDepth)
        {
            questionsLeft += 1;
        }

        double totalQuestions = Settings.QuestionsPerLevel;

        return Math.Clamp(1.0 - (questionsLeft / totalQuestions), 0.0, 1.0);
    }
}