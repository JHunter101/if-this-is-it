using Core.Models;

namespace Core.Services;

public interface IPromptRepository
{
    Task<SessionPrompts> GetPromptsAsync(GameSettings settings);
}

public class PromptRepository(HttpClient http) : IPromptRepository
{
    private const string StrangersPath = "data/prompts-1-strangers.txt";
    private const string SurfacePath = "data/prompts-2-surface.txt";
    private const string CloseFriendsPath = "data/prompts-3-closefriends.txt";

    public async Task<SessionPrompts> GetPromptsAsync(GameSettings settings)
    {
        IEnumerable<string> strangers = await LoadPromptsAsync(StrangersPath, settings.QuestionsPerLevel);
        IEnumerable<string> surface = await LoadPromptsAsync(SurfacePath, settings.QuestionsPerLevel);
        IEnumerable<string> closeFriends = await LoadPromptsAsync(CloseFriendsPath, settings.QuestionsPerLevel);

        return new SessionPrompts(strangers, surface, closeFriends);
    }

    private async Task<IEnumerable<string>> LoadPromptsAsync(string path, int count)
    {
        string text = await http.GetStringAsync(path);

        string[] prompts = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => p.ToLowerInvariant())
            .ToArray();

        Random.Shared.Shuffle(prompts);
        return prompts.Take(count);
    }
}