using Core.Models;

namespace Core.Services;

public interface IPromptRepository
{
    Task<SessionPrompts> GetPromptsAsync(GameSettings settings);
}

public class PromptRepository(HttpClient http) : IPromptRepository
{
    private const string STRANGERS_PATH = "data/prompts-1-strangers.txt";
    private const string INTERLINKED_PATH = "data/prompts-2-interlinked.txt";
    private const string INTIMATE_PATH = "data/prompts-3-intimate.txt";

    public async Task<SessionPrompts> GetPromptsAsync(GameSettings settings)
    {
        IEnumerable<string> strangers = await LoadPromptsAsync(STRANGERS_PATH, settings.QuestionsPerLevel);
        IEnumerable<string> interlinked = await LoadPromptsAsync(INTERLINKED_PATH, settings.QuestionsPerLevel);
        IEnumerable<string> intimate = await LoadPromptsAsync(INTIMATE_PATH, settings.QuestionsPerLevel);

        return new SessionPrompts(strangers, interlinked, intimate);
    }

    private async Task<IEnumerable<string>> LoadPromptsAsync(string path, int count)
    {
        string text = await http.GetStringAsync(path);

        string[] prompts = [.. text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => p.ToLowerInvariant())];

        Random.Shared.Shuffle(prompts);
        return prompts.Take(count);
    }
}