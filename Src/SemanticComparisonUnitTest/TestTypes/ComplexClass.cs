using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class ComplexClass
    {
        public int Number { get; set; }

        public string Text { get; set; }

        public Version Version { get; set; }

        public OperatingSystem OperatingSystem { get; set; }

        public ValueObject Value { get; set; }

        public Entity Entity { get; set; }
    }

#if !SYSTEM_OPERATINGSYSTEM
    /* A copy of the OperatingSystem class for .NET Standard as it isn't present there.
       Source code has been copied from the System.OperatingSystem class.   
     */

    public enum PlatformID
    {
        Win32S = 0,
        Win32Windows = 1,
        Win32NT = 2,
        WinCE = 3,
        Unix = 4,
        Xbox = 5,
        MacOSX = 6
    }

    public sealed class OperatingSystem
    {
        private readonly string _servicePack;
        private string _versionString;

        public OperatingSystem(PlatformID platform, Version version) : this(platform, version, null)
        {
        }

        private OperatingSystem(PlatformID platform, Version version, string servicePack)
        {
            if (platform < PlatformID.Win32S || platform > PlatformID.MacOSX)
            {
                throw new ArgumentOutOfRangeException(nameof(platform), "Invalid platform");
            }
            if (version == null) throw new ArgumentNullException(nameof(version));

            Platform = platform;
            Version = new Version(version.ToString());
            _servicePack = servicePack;
        }

        public PlatformID Platform { get; }

        public string ServicePack => _servicePack ?? string.Empty;

        public Version Version { get; }

        public override string ToString() => VersionString;

        public string VersionString
        {
            get
            {
                if (_versionString != null)
                {
                    return _versionString;
                }

                string os;
                switch (Platform)
                {
                    case PlatformID.Win32NT:
                        os = "Microsoft Windows NT ";
                        break;
                    case PlatformID.Win32Windows:
                        if (Version.Major > 4 ||
                            (Version.Major == 4 && Version.Minor > 0))
                            os = "Microsoft Windows 98 ";
                        else
                            os = "Microsoft Windows 95 ";
                        break;
                    case PlatformID.Win32S:
                        os = "Microsoft Win32S ";
                        break;
                    case PlatformID.WinCE:
                        os = "Microsoft Windows CE ";
                        break;
                    case PlatformID.MacOSX:
                        os = "Mac OS X ";
                        break;
                    default:
                        os = "<unknown> ";
                        break;
                }

                if (string.IsNullOrEmpty(_servicePack))
                {
                    _versionString = os + Version;
                }
                else
                {
                    _versionString = os + Version.ToString(3) + " " + _servicePack;
                }

                return _versionString;
            }
        }
    }
#endif
}
