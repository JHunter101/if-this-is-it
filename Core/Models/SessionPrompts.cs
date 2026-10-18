namespace Core.Models;

public class SessionPrompts(IEnumerable<string> strangers, IEnumerable<string> surface, IEnumerable<string> closeFriends)
{
    private readonly Queue<string> _strangers = new(strangers);
    private readonly Queue<string> _surface = new(surface);
    private readonly Queue<string> _closeFriends = new(closeFriends);

    public PromptDepth CurrentLevel { get; private set; } = PromptDepth.Strangers;

    public string? Next()
    {
        while (true)
        {
            if (GetQueue(CurrentLevel).TryDequeue(out string? prompt))
            {
                return prompt;
            }

            PromptDepth nextLevel = CurrentLevel + 1;

            if (!Enum.IsDefined(nextLevel))
            {
                return null;
            }

            CurrentLevel = nextLevel;
        }
    }

    public string? PeekNext()
        => GetQueue(CurrentLevel).TryPeek(out string? prompt) ? prompt : null;

    internal int GetQuestionsLeft(PromptDepth promptDepth) => GetQueue(promptDepth).Count;

    private Queue<string> GetQueue(PromptDepth level)
        => level switch
        {
            PromptDepth.Strangers => _strangers,
            PromptDepth.Interlinked => _surface,
            PromptDepth.Intimate => _closeFriends,
            _ => throw new InvalidOperationException($"No queue exists for level {level}.")
        };
}