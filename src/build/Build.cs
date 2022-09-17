using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static AppVersion;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("The target framework - Default is [net6.0]")]
    readonly string PublishFramework = "net6.0";

    [Parameter("The target runtime - Default is [win-x64]")]
    readonly string PublishRuntime = "win-x64";

    [Parameter("The project to publish - Default is [NotepadBasedCalculator.Desktop]")]
    readonly string PublishProject = "NotepadBasedCalculator.Desktop";

    [Parameter("https://bit.ly/2OEU0KO - Enabled by default")]
    readonly bool PublishSelfContained = true;

    [Parameter("https://bit.ly/3xvq7FA - Enabled by default")]
    readonly bool PublishSingleFile = true;

    [Parameter("https://bit.ly/3RSEo7w")]
    readonly bool PublishReadyToRun;

    [Parameter("https://bit.ly/3RKZkNH")]
    readonly bool PublishTrimmed;

    [Parameter("Runs unit tests")]
    readonly bool RunTests;

    [Solution]
    readonly Solution Solution;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            RootDirectory.GlobDirectories("bin", "obj", "publish").ForEach(DeleteDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.Quiet));
        });

    Target SetVersion => _ => _
        .DependentFor(Compile)
        .After(Restore)
        .OnlyWhenDynamic(() => Configuration == Configuration.Release)
        .Executes(() =>
        {
            SetAppVersion(RootDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.Quiet)
                .EnableNoRestore());
        });

    Target UnitTests => _ => _
        .DependentFor(Publish)
        .After(Compile)
        .OnlyWhenDynamic(() => RunTests)
        .Executes(() =>
        {
            RootDirectory
                .GlobFiles("**/*.Tests.csproj")
                .ForEach(f =>
                    DotNetTest(s => s
                    .SetProjectFile(f)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .EnableNoBuild()
                    .SetVerbosity(DotNetVerbosity.Quiet)));
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetProject(Solution.GetProject(PublishProject))
                .SetConfiguration(Configuration)
                .SetFramework(PublishFramework)
                .SetRuntime(PublishRuntime)
                .SetSelfContained(PublishSelfContained)
                .SetPublishSingleFile(PublishSingleFile)
                .SetPublishReadyToRun(PublishReadyToRun)
                .SetPublishTrimmed(PublishTrimmed)
                .SetVerbosity(DotNetVerbosity.Quiet)
                .SetOutput(RootDirectory / "publish" / PublishProject + "-" + PublishFramework + "-" + PublishRuntime));
        });
}
