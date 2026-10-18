namespace Specifications.Transformations;

[Binding]
public static class PromptDepthTransformations
{
    [StepArgumentTransformation]
    public static PromptDepth TransformPromptDepth(string value) => Enum.Parse<PromptDepth>(value, ignoreCase: true);
}