using Integration;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;

[AppVeyor(
    AppVeyorImage.VisualStudio2019,
    AutoGenerate = false,
    InvokedTargets = new[] { nameof(Verify), nameof(Cover), nameof(Pack), nameof(AppVeyorPublish) },
    Secrets = new[] { AppVeyorSecrets.NuGetApiKey, AppVeyorSecrets.MyGetApiKey })]
partial class Build
{
    [CI] readonly AppVeyor AppVeyor;

    Target AppVeyorUploadTestResults => _ => _
        .TriggeredBy(Test)
        .After(Test)
        .Before(Pack)
        .OnlyWhenDynamic(() => IsServerBuild && AppVeyor != null)
        .Executes(() =>
        {
            AppVeyor.UploadTestResults(TestType.MsTest, TestResultsDirectory, "*.trx");
            AppVeyor.UploadTestResults(TestType.NUnit, TestResultsDirectory, "*.xml");
        });

    Target AppVeyorSetBuildVersion => _ => _
        .TriggeredBy(Restore)
        .After(Restore)
        .OnlyWhenDynamic(() => IsServerBuild && AppVeyor != null)
        .Executes(() =>
        {
            AppVeyor.UpdateBuildVersion(
                $"{GitVersion.MajorMinorPatch}.{AppVeyor.BuildNumber}{GitVersion.PreReleaseTagWithDash}");
        });

    Target AppVeyorPublish => _ => _
        .OnlyWhenStatic(() => Trigger == BuildTrigger.SemVerTag)
        .Executes(PublishPackages);

    public static class AppVeyorSecrets
    {
        public const string NuGetApiKeyValue = "b/B9mFX99as5WjjR9Xzr7zAUDKwvCOmPgEkttJxcP+OClOv59lrcIE4OrsAdQRW3";
        public const string NuGetApiKey = Secrets.NuGetApiKey + ":" + NuGetApiKeyValue;

        public const string MyGetApiKeyValue = "hA4Ut1N2lrrdEtAN24Bty/FNiU0d/Ur/dLYSqpr8jKHOvoO7MU4jD+KwzUvATh+E";
        public const string MyGetApiKey = Secrets.MyGetApiKey + ":" + MyGetApiKeyValue;
    }
}