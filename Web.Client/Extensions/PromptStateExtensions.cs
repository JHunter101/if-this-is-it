using Core.Models;

namespace Web.Client.Extensions;

public static class PromptStateExtensions
{
    public static string ToDisplayText(this PromptDepth state)
    {
        return state switch
        {
            PromptDepth.Strangers => "strangers",
            PromptDepth.Interlinked => "interlinked",
            PromptDepth.Intimate => "intimate",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}