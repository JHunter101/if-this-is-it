namespace Specifications.Stubs;

public sealed class PromptRepositoryStub : IPromptRepository
{
    private List<string> _strangers = ["Stranger 1"];
    private List<string> _surface = ["Surface 1"];
    private List<string> _closeFriends = ["Intimate 1"];

    public Task<SessionPrompts> GetPromptsAsync(GameSettings settings) => Task.FromResult(new SessionPrompts(_strangers, _surface, _closeFriends));

    public void SetPrompts(IEnumerable<string> strangers, IEnumerable<string> surface, IEnumerable<string> closeFriends)
    {
        _strangers = [.. strangers];
        _surface = [.. surface];
        _closeFriends = [.. closeFriends];
    }
}