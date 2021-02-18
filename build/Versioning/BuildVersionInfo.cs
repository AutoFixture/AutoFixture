namespace Versioning
{
    public record BuildVersionInfo
    {
        public string AssemblyVersion { get; set; }
        public string FileVersion { get; set; }
        public string InfoVersion { get; set; }
        public string NuGetVersion { get; set; }

        public override string ToString()
            => $"Assembly: {AssemblyVersion}, Info: {InfoVersion}, File: {FileVersion} NuGet: {NuGetVersion}";
    }
}