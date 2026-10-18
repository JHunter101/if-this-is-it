using Core.Models;
using Core.Services;
using Shouldly;

namespace Core.Test.Services;

[TestClass]
public sealed class PromptRepositoryTests
{
    [TestMethod]
    public async Task GetPromptsAsync_TrimsLowercasesAndSkipsBlankLines()
    {
        using var handler = new StubHttpHandler(_ => "\r\n  FIRST Prompt  \r\n\t \r\nSECOND PROMPT\n\n Third Prompt\t\n");
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.test/") };
        var sut = new PromptRepository(http);

        SessionPrompts prompts = await sut.GetPromptsAsync(new GameSettings());

        foreach (PromptDepth level in Enum.GetValues<PromptDepth>())
        {
            prompts.GetQuestionsLeft(level).ShouldBe(3);
            var actual = new List<string?> { prompts.Next(), prompts.Next(), prompts.Next() };
            actual.ShouldBe(["first prompt", "second prompt", "third prompt"], ignoreOrder: true);
            prompts.CurrentLevel.ShouldBe(level);
        }

        prompts.Next().ShouldBeNull();
    }

    [TestMethod]
    public async Task GetPromptsAsync_MapsRenamedPathsToTheirLevels()
    {
        var responses = new Dictionary<string, string>
        {
            ["/data/prompts-1-strangers.txt"] = "Stranger Prompt",
            ["/data/prompts-2-interlinked.txt"] = "Interlinked Prompt",
            ["/data/prompts-3-intimate.txt"] = "Intimate Prompt"
        };
        var requestedPaths = new List<string>();
        using var handler = new StubHttpHandler(request =>
        {
            string path = request.RequestUri!.AbsolutePath;
            requestedPaths.Add(path);
            return responses[path];
        });
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.test/") };
        var sut = new PromptRepository(http);

        SessionPrompts prompts = await sut.GetPromptsAsync(new GameSettings());

        requestedPaths.ShouldBe(responses.Keys, ignoreOrder: true);
        prompts.Next().ShouldBe("stranger prompt");
        prompts.CurrentLevel.ShouldBe(PromptDepth.Strangers);
        prompts.Next().ShouldBe("interlinked prompt");
        prompts.CurrentLevel.ShouldBe(PromptDepth.Interlinked);
        prompts.Next().ShouldBe("intimate prompt");
        prompts.CurrentLevel.ShouldBe(PromptDepth.Intimate);
        prompts.Next().ShouldBeNull();
    }

    [TestMethod]
    public async Task GetPromptsAsync_LimitsPromptsPerLevelToSettings()
    {
        var settings = new GameSettings();
        string text = string.Join('\n', Enumerable.Range(1, settings.QuestionsPerLevel + 5).Select(i => $"Prompt {i}"));
        using var handler = new StubHttpHandler(_ => text);
        using var http = new HttpClient(handler) { BaseAddress = new Uri("https://example.test/") };
        var sut = new PromptRepository(http);

        SessionPrompts prompts = await sut.GetPromptsAsync(settings);

        foreach (PromptDepth level in Enum.GetValues<PromptDepth>())
        {
            prompts.GetQuestionsLeft(level).ShouldBe(settings.QuestionsPerLevel);
        }
    }

    private sealed class StubHttpHandler(Func<HttpRequestMessage, string> response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(response(request))
            });
    }
}
