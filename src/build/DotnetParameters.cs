using Nuke.Common.IO;

internal sealed record DotnetParameters
{
    internal readonly AbsolutePath ProjectOrSolutionPath;

    internal readonly string RuntimeIdentifier;

    internal readonly string TargetFramework;

    internal readonly bool PublishTrimmed;

    public DotnetParameters(AbsolutePath projectOrSolutionPath, string runtimeIdentifier, string targetFramework, bool publishTrimmed)
    {
        ProjectOrSolutionPath = projectOrSolutionPath;
        RuntimeIdentifier = runtimeIdentifier;
        TargetFramework = targetFramework;
        PublishTrimmed = publishTrimmed;
    }
}
