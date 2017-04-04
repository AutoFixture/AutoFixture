#### 4.57.4 - 29.03.2017
* Make exit code accessible - https://github.com/fsharp/FAKE/pull/1502

#### 4.57.3 - 29.03.2017
* Run parallel targets just in time - https://github.com/fsharp/FAKE/pull/1396

#### 4.56.0 - 17.03.2017
* Yarn-Helper - https://github.com/fsharp/FAKE/pull/1494
* Add F# 4.1 directory path to FSIHelper paths

#### 4.55.0 - 13.03.2017
* HockeyApp - create version and upload build to a specific version - https://github.com/fsharp/FAKE/pull/1487

#### 4.54.0 - 13.03.2017
* Unix msbuild probing enhancements - https://github.com/fsharp/FAKE/pull/1488

#### 4.53.0 - 12.03.2017
* New change log helper - https://github.com/fsharp/FAKE/pull/1467
* New output parameter to Dotnet BuildParams - https://github.com/fsharp/FAKE/pull/1481
* Added MSBuild location for VS2017 Build Tools - https://github.com/fsharp/FAKE/pull/1484
* BUGFIX: Fixed bug in getLastNuGetVersion when result is in JSON - https://github.com/fsharp/FAKE/pull/1482

#### 4.52.0 - 01.03.2017
* Implement --dotGraph command line option - https://github.com/fsharp/FAKE/pull/1469
* USABILITY: Improve error handling on SqlPackage - https://github.com/fsharp/FAKE/pull/1476
* USABILITY: Don't fail on git file status detection

#### 4.51.0 - 28.02.2017
* Teamcity build parameters - https://github.com/fsharp/FAKE/pull/1475
* Added updating of build details to AppVeyor - https://github.com/fsharp/FAKE/pull/1473
* New Expecto --parallel-workers and --summary-location parameters - https://github.com/fsharp/FAKE/pull/1450
* BREAKING: Git: recognize renamed (and other status) files - https://github.com/fsharp/FAKE/pull/1472
* BUGFIX: Using correct CLI parameter for silent xUnit output - https://github.com/fsharp/FAKE/pull/1464

#### 4.50.1 - 20.02.2017
* BUGFIX: Use sequenced parameter for expecto

#### 4.50.0 - 17.01.2017
* Visual Studio aware msbuild selection - https://github.com/fsharp/FAKE/pull/1453

#### 4.49.0 - 15.01.2017
* MSBuild 15.0/VS 2017RC support - https://github.com/fsharp/FAKE/pull/1442

#### 4.48.0 - 05.01.2017
* Add DisableParallel to dotnet restore params - https://github.com/fsharp/FAKE/pull/1443
* Added Expecto.CustomArgs property for new cli arguments - https://github.com/fsharp/FAKE/pull/1441
* New Expecto --verion parameter
* New Expecto --fail-on-focused-tests parameter
* New Expecto --summary parameter
* USABILITY: More verbose kill of processes

#### 4.47.0 - 17.12.2016
* New Expecto helper - https://github.com/fsharp/FAKE/pull/1435
* Displas original server response when failing to parse JSON in Fake.Deploy - https://github.com/fsharp/FAKE/pull/1432
* Added SQLCMD variable support - https://github.com/fsharp/FAKE/pull/1434
* USABILITY: Improve error logging in GitVersionHelper - https://github.com/fsharp/FAKE/pull/1429

#### 4.46.0 - 03.12.2016
* Decorated all *Params helper records with [<CLIMutable>] for C# access - https://github.com/fsharp/FAKE/pull/1428
* Added credentials parameter to ApplicationPoolConfig in IISHelper - https://github.com/fsharp/FAKE/pull/1425
* BUGFIX: Added a delay to prevent object disposed exceptions from process on macosx - https://github.com/fsharp/FAKE/pull/1426
* BUGFIX: Added try catch block to ignore error from setting console encoding - https://github.com/fsharp/FAKE/pull/1422
* BUGFIX: Disable NodeReuse for MSBuild on Jenkins too - https://github.com/fsharp/FAKE/pull/1418

#### 4.45.1 - 05.11.2016
* BREAKING CHANGE: Remove old DotNet helper and cleanup DotNetCli helper
* BUGFIX: Worked around breaking change in NuGet 3.5 - https://github.com/fsharp/FAKE/issues/1415
* BUGFIX: Added logic to allow parsing of git branch names which track - https://github.com/fsharp/FAKE/pull/1417
* BUGFIX: Set Console.OutputEncoding <- System.Text.Encoding.UTF8 - https://github.com/fsharp/FAKE/pull/1414
* REVERT: Enable ServiceConfig element configuration in WixHelper - https://github.com/fsharp/FAKE/pull/1412

#### 4.44.0 - 03.11.2016
* Enable ServiceConfig element configuration in WixHelper - https://github.com/fsharp/FAKE/pull/1412
* BUGFIX: Moved process stdout encoding out of is silent check - https://github.com/fsharp/FAKE/pull/1414 

#### 4.43.0 - 30.10.2016
* Better tracing of tasks in TeamCity - https://github.com/fsharp/FAKE/pull/1408
* BUGFIX: getBranchName works language independent - https://github.com/fsharp/FAKE/pull/1409
* Add support for the pin-project-references switch to PaketHelper - https://github.com/fsharp/FAKE/pull/1410

#### 4.42.0 - 25.10.2016
* Add option to emit SuppressMessage Attributes - https://github.com/fsharp/FAKE/pull/1406
* Add language in NuGetParams - https://github.com/fsharp/FAKE/pull/1407
* Change order of Dynamics NAV process killing 
* New SSH helper - https://github.com/fsharp/FAKE/pull/1405 
* BUGFIX: FAKE should work with old and new commit messages
* BUGFIX: Fixed bug in assembly info variable name suffixes - https://github.com/fsharp/FAKE/pull/1404
* BUGFIX: Make FAKE fail on failing git push
* BUGFIX: When generating AssemblyMetadata_XYZ for AssemblyInfo, pass just value - https://github.com/fsharp/FAKE/pull/1399
* BUGFIX: Fixed AssemblyVersion bug - https://github.com/fsharp/FAKE/pull/1397
* BUGFIX: Fixing the famous chinese FAKE bug

#### 4.41.1 - 06.10.2016
* Add all assembly metadata to AssemblyVersionInformation typehttps://github.com/fsharp/FAKE/pull/1392
* Allow additional properties to be specified when running the SQL dacpac tooling - https://github.com/fsharp/FAKE/pull/1386
* Support for dotnet publish
* BUGFIX: wixHelper AllowDowngrades - https://github.com/fsharp/FAKE/pull/1389
* REVERT: Use nuget instead of referenced dlls. Fix SQL Server version issue - https://github.com/fsharp/FAKE/pull/1377

#### 4.40.0 - 19.09.2016
* Allow to pass parameter to SonarQube end - https://github.com/fsharp/FAKE/pull/1381
* New helper: Search for files with a given pattern also in subdirectories - https://github.com/fsharp/FAKE/pull/1354
* Adds comment on top of auto-generated AssemblyInfo.fs files - https://github.com/fsharp/FAKE/pull/1373
* Use nuget instead of referenced dlls. Fix SQL Server version issue - https://github.com/fsharp/FAKE/pull/1377
* BUGFIX: NuGetVersion: adds application/xml to request accept header - https://github.com/fsharp/FAKE/pull/1383
* BUGFIX: Replace process cache with safe alternative - https://github.com/fsharp/FAKE/pull/1378
* BUGFIX: Call 'traceEndTask' in the finally block of a try-finally, so that it is always closed, even if the task throws an exception. https://github.com/fsharp/FAKE/pull/1379
* USABILITY: Check for npm on path variable in NpmHelper on Windows - https://github.com/fsharp/FAKE/pull/1371

#### 4.39.0 - 25.08.2016
* Added Checksum[64][Type] in ChocoHelper - https://github.com/fsharp/FAKE/pull/1367
* Better support for multiple versions of SqlPackage - https://github.com/fsharp/FAKE/pull/1368
* Hint shown in for ArgumentException #1355 instead of trying to set it directly - https://github.com/fsharp/FAKE/pull/1366
* Added isWindows helper - https://github.com/fsharp/FAKE/pull/1356
* BUGFIX: Made GitVersionHelper PreReleaseNumber Nullable - https://github.com/fsharp/FAKE/pull/1365
* BUGFIX: TERM environment property should be upper case - https://github.com/fsharp/FAKE/pull/1363

#### 4.38.0 - 22.08.2016
* BUGFIX: System.ArgumentNullException thrown for colored output on mono - https://github.com/fsharp/FAKE/pull/1362
* BUGFIX: Trim assembly info attribute value - https://github.com/fsharp/FAKE/pull/1361
* BUGFIX: Not printing MSBUILD command line twice - https://github.com/fsharp/FAKE/pull/1359
* BUGFIX: Semver parse fix to handle prereleases and build parts - https://github.com/fsharp/FAKE/pull/1325
* BUGFIX: Fixed FSCHelper -https://github.com/fsharp/FAKE/pull/1351, https://github.com/fsharp/FAKE/pull/1352

#### 4.37.0 - 09.08.2016
* New Octopus command for push - https://github.com/fsharp/FAKE/pull/1349
* New GitVersionHelper - https://github.com/fsharp/FAKE/pull/1319
* BUGFIX: Fixed multiple references for DLL in Fsc helper - https://github.com/fsharp/FAKE/pull/1350
* BUGFIX: Fixed NugetHelper.fs: GetPackageVersion - https://github.com/fsharp/FAKE/pull/1343
* BUGFIX: Fixed detection of GitLab CI with current multi-runner - https://github.com/fsharp/FAKE/pull/1345

#### 4.36.0 - 01.08.2016
* Added methods to cover manipulation of Content Nodes in project files - https://github.com/fsharp/FAKE/pull/1335
* BUGFIX: Fix Fsc short toggle argument format - https://github.com/fsharp/FAKE/pull/1339
* BUGFIX: Update search pattern in NuGet helper - https://github.com/fsharp/FAKE/pull/1334
* BUGFIX: Expanded typescript search paths on windows to cover all new typescript compilers - https://github.com/fsharp/FAKE/pull/1308

#### 4.35.0 - 24.07.2016
* New registry support in WiXHelper - https://github.com/fsharp/FAKE/pull/1331
* BREAKING CHANGE: Changed DotNet helper to DotNetCLI - https://github.com/fsharp/FAKE/pull/1332
* BUGFIX: Fixed exception when dotnet cli is not installed - https://github.com/fsharp/FAKE/pull/1332
* BUGFIX: Fixed git reset helper to use checkout when file resets are requested - https://github.com/fsharp/FAKE/pull/1326
* BUGFIX: Masked octo api key when tracing - https://github.com/fsharp/FAKE/pull/1327 

#### 4.34.5 - 21.07.2016
* DotNet version support - https://github.com/fsharp/FAKE/pull/1310
* DotNet test support - https://github.com/fsharp/FAKE/pull/1311
* DotNet build support - https://github.com/fsharp/FAKE/pull/1318
* DotNet pack support - https://github.com/fsharp/FAKE/pull/1313
* Allows to set version in project.json
* Allow to run arbitrary dotnet CLI commands
* Allow to add arbitrary args to all dotnet CLI commands
* DotNet restore support - https://github.com/fsharp/FAKE/pull/1309
* BUGFIX: Update DACPAC module - https://github.com/fsharp/FAKE/pull/1307

#### 4.32.0 - 18.07.2016
* BUGFIX: Convert relative path to absolute path when creating NAntXmlTraceListen - https://github.com/fsharp/FAKE/pull/1305
* BUGFIX: Update DACPAC module - https://github.com/fsharp/FAKE/pull/1306
* BUGFIX: Fixed FscParam.References issue - https://github.com/fsharp/FAKE/pull/1304
* REVERT: Better Old-Style Arg parsing - https://github.com/fsharp/FAKE/pull/1301

#### 4.31.0 - 14.07.2016
* BUGFIX: Better Old-Style Arg parsing - https://github.com/fsharp/FAKE/pull/1301
* BUGFIX: Fixed typo in continuous web job path - https://github.com/fsharp/FAKE/pull/1297 https://github.com/fsharp/FAKE/pull/1300
* COSMETICS: Update XUnit2 module to fail gracefully - https://github.com/fsharp/FAKE/pull/1302

#### 4.30.0 - 12.07.2016
* Improved default npm path handling - https://github.com/fsharp/FAKE/pull/1278
* BUGFIX: Fixed Fake.Deploy downloadString - https://github.com/fsharp/FAKE/pull/1288
* BUGFIX: update fix for mono encoding in ProcessHelper.fs - https://github.com/fsharp/FAKE/pull/1276
* BUGFIX: XamarinHelper - file name was not quoted by calling zipalign - https://github.com/fsharp/FAKE/pull/1294

#### 4.29.0 - 19.06.2016
* New helper to execute Sysinternals PsExec - https://github.com/fsharp/FAKE/pull/1266
* Add initial support for Fuchu tests - https://github.com/fsharp/FAKE/pull/1268
* New Bower helper - https://github.com/fsharp/FAKE/pull/1258
* FAKE cache is now local to the build script - https://github.com/fsharp/FAKE/pull/1250
* BUGFIX: Correct waiting for android packaging and fix for mono processes encoding - https://github.com/fsharp/FAKE/pull/1275
* BUGFIX: Fixed issue in bulk component creation functions where IDs where invalid - https://github.com/fsharp/FAKE/pull/1264
* BUGFIX: Support VB.NET's case-insensitive assembly attributes - https://github.com/fsharp/FAKE/pull/1255
* BUGFIX: Bamboo buildNumber environment variable in case sensitive behavior - https://github.com/fsharp/FAKE/pull/1252
* BUGFIX: Final Targets are no longer lowercased - https://github.com/fsharp/FAKE/pull/1261

#### 4.28.0 - 30.05.2016
* New DocFx helper - https://github.com/fsharp/FAKE/pull/1251
* Added basic support for bitbuckets piplines CI - https://github.com/fsharp/FAKE/pull/1248
* BUGFIX: XamarinHelper: surround jarsigner input file path with quotes - https://github.com/fsharp/FAKE/pull/1249
* BUGFIX: NUnit3: don't set a timeout on the nunit3-console process - https://github.com/fsharp/FAKE/pull/1247
* BUGFIX: Changed the cache path to be relative to script location - https://github.com/fsharp/FAKE/pull/1250

#### 4.27.0 - 19.05.2016
* New Slack argument for Link_Names - https://github.com/fsharp/FAKE/pull/1245
* Extended WiXHelper types for supporting creation of 64bit setups - https://github.com/fsharp/FAKE/pull/1244            
* BUGFIX: Corrected NuGet install verbosity parameter - https://github.com/fsharp/FAKE/pull/1239

#### 4.26.0 - 11.05.2016
* Added new function for appending signatures - https://github.com/fsharp/FAKE/pull/1223
* New separate environ variable helpers - https://github.com/fsharp/FAKE/pull/1133
* Reversed the order of actions in traceStartTarget - https://github.com/fsharp/FAKE/pull/1222
* Update Pickles helper to reflect the latest changes to pickles - https://github.com/fsharp/FAKE/pull/1236
* New parameter 'AppId' in HockeyAppHelper - https://github.com/fsharp/FAKE/pull/1234
* MSBuildHelper: Add BuildWebsite(s)Config - https://github.com/fsharp/FAKE/pull/1230
* BUGFIX: OpenCoverHelper does not assume AppData and ProgramFiles exists by default - https://github.com/fsharp/FAKE/pull/1229
* BUGFIX: Disable node reuse on Team foundation builds - https://github.com/fsharp/FAKE/pull/1237
* BUGFIX: Fixed FAKE parameter split - https://github.com/fsharp/FAKE/pull/1228 
* USABILITY: Look into packages folder for findToolPath 
* COSMETICS: Added open/close block to teamcity target tracing - https://github.com/fsharp/FAKE/pull/1219 

#### 4.25.0 - 12.04.2016
* Use FSharp.Compiler.Service 3.0
* BUGFIX: Added TeamCity to the list of build servers that fails on error stream output - https://github.com/fsharp/FAKE/pull/1216
* BUGFIX: Fixed failure to handle spaces or other special characters when running mono exes - https://github.com/fsharp/FAKE/pull/1214
* BUGFIX: Use UTF-8 encoding for process output on Mono - https://github.com/fsharp/FAKE/pull/1215

#### 4.24.0 - 10.04.2016
* New Kudu feature to copy recursively - https://github.com/fsharp/FAKE/pull/1203
* Support for source code deployments on Azure websites through KuduSync - https://github.com/fsharp/FAKE/pull/1200
* Expose 'GetErrors' on TargetHelper - https://github.com/fsharp/FAKE/pull/1209
* BUGFIX: Call WaitForExit twice to catch all output - https://github.com/fsharp/FAKE/pull/1211
* BUGFIX: Only write to STDERR if an error happened - https://github.com/fsharp/FAKE/pull/1210
* BUGFIX: Prevent false-positive in EnvironmentHelper.isMacOS on Windows - https://github.com/fsharp/FAKE/pull/1204
* BUGFIX: Accept the cached assembly when the public token of the given assembly is null - https://github.com/fsharp/FAKE/pull/1205

#### 4.23.0 - 01.04.2016
* Make a new method for sending a coverage file to TeamCity - https://github.com/fsharp/FAKE/pull/1195
* Added more deployment options for OctoTools - https://github.com/fsharp/FAKE/pull/1192
* Added contents of `AssemblyInformationalVersionAttribute` to the `AssemblyVersionInformation` class - https://github.com/fsharp/FAKE/pull/1183
* Added HarvestDirectory helper to the WixHelper Library - https://github.com/fsharp/FAKE/pull/1179
* Added support for SQL DacPac - https://github.com/fsharp/FAKE/pull/1185
* Provide CurrentTargetOrder for build scripts
* Added namespace/class/method args for xunit2 - https://github.com/fsharp/FAKE/pull/1180 
* If build failed then kill all created processes at end of build
* Make DynamicsNAV errors a known FAKE exception
* BUGFIX: Fixed hard coded path in artifact publishing to AppVeyor - https://github.com/fsharp/FAKE/pull/1176

#### 4.22.0 - 13.03.2016
* Added artifact publishing to AppVeyor - https://github.com/fsharp/FAKE/pull/1173 
* Azure Web Jobs now get created during deploy if they do not already exist - https://github.com/fsharp/FAKE/pull/1174
* BUGFIX: New Sonar options - https://github.com/fsharp/FAKE/pull/1172
* BUGFIX: Fixed issue with IDs that did not start with a letter - https://github.com/fsharp/FAKE/pull/1167
* BUGFIX: Fixed IgnoreTestCase helper - https://github.com/fsharp/FAKE/pull/1159
* BUGFIX: use compileFiles in compile - https://github.com/fsharp/FAKE/pull/1165
* BUGFIX: Fixed bug in WiXDir function, that would set plain directory name as id - https://github.com/fsharp/FAKE/pull/1164
* BUGFIX: Fixed bug that prevented using directory names with spaces in WiX - https://github.com/fsharp/FAKE/pull/1160

#### 4.21.0 - 29.02.2016
* New helper for chocolatey - http://fsharp.github.io/FAKE/chocolatey.html
* New helper for Slack - http://fsharp.github.io/FAKE/slacknotification.html
* New helper for SonarQube - http://fsharp.github.io/FAKE/sonarcube.html
* New helper for creating paket.template files for Paket - https://github.com/fsharp/FAKE/pull/1148
* New version of WatchChanges that support options - https://github.com/fsharp/FAKE/pull/1144
* Improved AppVeyor test results upload - https://github.com/fsharp/FAKE/pull/1138
* Added support for Paket's minimum-from-lock-file in pack command - https://github.com/fsharp/FAKE/pull/1149
* Added support for NUnit3 --labels parameter - https://github.com/fsharp/FAKE/pull/1153
* BUGFIX: Fixed Issue #1142: Arguments of CombinePaths are switched in WixHelper - https://github.com/fsharp/FAKE/pull/1145 
* BUGFIX: NuGet auto version bug fix - https://github.com/fsharp/FAKE/pull/1146
* WORKAROUND: nuget.org changed base url

#### 4.20.0 - 06.02.2016
* Allows to create full Wix directory hierarchy - https://github.com/fsharp/FAKE/pull/1116
* New PicklesHelper for generating living documentation with Pickles - https://github.com/fsharp/FAKE/pull/1126
* BUGFIX: Replaced system directory separator with "/" in ArchiveHelper - https://github.com/fsharp/FAKE/pull/1127

#### 4.19.0 - 02.02.2016
* New FSC task - https://github.com/fsharp/FAKE/pull/1122
* Disable warning from #1082 for now because it created lots of confusion 

#### 4.18.0 - 02.02.2016
* New helpers which allow to send .NET coverage settings to TeamCity - https://github.com/fsharp/FAKE/pull/1117
* Disabled NodeReuse on TeamCity, it can lead to consecutive builds failing - https://github.com/fsharp/FAKE/pull/1110
* Added IncludeReferencedProjects property to the Packet.Pack params - https://github.com/fsharp/FAKE/pull/1112
* BUGFIX: Ensure that traceEndTask is called in DotCover - https://github.com/fsharp/FAKE/pull/1118
* BUGFIX: WiXHelper: fixed typos in WiXDir.ToString - https://github.com/fsharp/FAKE/pull/1120

#### 4.17.0 - 23.01.2016
* Renamed internal FSharp.Compiler.Service to avoid clashes - https://github.com/fsharp/FAKE/pull/1097
* Added support for "paket restore" - https://github.com/fsharp/FAKE/pull/1108
* WiX service install - https://github.com/fsharp/FAKE/pull/1099
* Passing timeout value also to solution exchanger in DynamicsCRMHelper - https://github.com/fsharp/FAKE/pull/1102
* BUGFIX: Fallback to recompile when caching of build script fails - https://github.com/fsharp/FAKE/pull/1093
* BUGFIX: Commit message will be retrieved for older and newer git versions - https://github.com/fsharp/FAKE/pull/1098
* BUGFIX: Fixed case sensitivity on package name when search references in Paket.lock - https://github.com/fsharp/FAKE/pull/1089
* COSMETICS: Don't show the obsolete usage - https://github.com/fsharp/FAKE/pull/1094
 
#### 4.16.0 - 20.01.2016
* General FAKE improvements - https://github.com/fsharp/FAKE/pull/1088
* Hockey app UploadTimeout - https://github.com/fsharp/FAKE/pull/1087

#### 4.15.0 - 19.01.2016
* Add support for appcast generation - https://github.com/fsharp/FAKE/pull/1057
* Function to remove Compile elems missing files - https://github.com/fsharp/FAKE/pull/1078
* AssemblyInfoFile: added functions to read and update attributes -https://github.com/fsharp/FAKE/pull/1073
* Added support for packing symbols via PaketHelper - https://github.com/fsharp/FAKE/pull/1071
* Tell the clr to use the cached assemblies even when it tries to reload them with a different context - https://github.com/fsharp/FAKE/pull/1056
* BUGFIX: Fix failure when space in temp path - https://github.com/fsharp/FAKE/pull/1076
* BUGFIX: Fix app.config files
* BUGFIX: Cache invalidate on changing fsiOptions - https://github.com/fsprojects/ProjectScaffold/issues/231

#### 4.14.0 - 12.01.2016
* NuGet automatic version increment - https://github.com/fsharp/FAKE/pull/1063
* Added support for the Paket pack parameter buildPlatform - https://github.com/fsharp/FAKE/pull/1066
* Added possibility to bulk update assembly infos with file includes - https://github.com/fsharp/FAKE/pull/1067

#### 4.13.0 - 11.01.2016
* NUnit 3 support - https://github.com/fsharp/FAKE/pull/1064
* Automatic discovery of octo.exe - https://github.com/fsharp/FAKE/pull/1065
* Prefer git from cmd path to get ssh key - https://github.com/fsharp/FAKE/pull/1062

#### 4.12.0 - 28.12.2015
* Change SignToolHelper syntax to reflect common call syntax - https://github.com/fsharp/FAKE/pull/1051
* New Open/Close block helpers for TeamCity - https://github.com/fsharp/FAKE/pull/1049
* BUGFIX: Use UTF-8 encoding of AssemblyInfo as written by ReplaceAssemblyInfoVersions - https://github.com/fsharp/FAKE/pull/1055

#### 4.11.0 - 19.12.2015
* Add specific version parameter in PaketPackParams - https://github.com/fsharp/FAKE/pull/1046
* Fixed isMacOS function - https://github.com/fsharp/FAKE/pull/1044
* Added more comfortable types to WiXHelper, flagged old ones obsolete - https://github.com/fsharp/FAKE/pull/1036
* Use FSharp.Compiler.Service 1.4.2.3
* Only add relative path prefix if not rooted path in MSBuildHelper - https://github.com/fsharp/FAKE/pull/1033
* Replaced hard reference on gacutil path with automatic search  - https://github.com/fsharp/FAKE/pull/1040
* Wrap OutputPath in quotes in paket helper - https://github.com/fsharp/FAKE/pull/1027
* Allow override of the signature algorithm and message digest algorithm in Xamarin helper - https://github.com/fsharp/FAKE/pull/1025
* Expose excluded templates in Pack helper - https://github.com/fsharp/FAKE/pull/1026
* Added initial implementation of DynamicsCRMHelper - https://github.com/fsharp/FAKE/pull/1009

#### 4.10.0 - 30.11.2015
* Added support for Squirrel's --no-msi option - https://github.com/fsharp/FAKE/pull/1013
* Upload has longer timeout - https://github.com/fsharp/FAKE/pull/1004
* Added the History Directory argument of ReportGenerator - https://github.com/fsharp/FAKE/pull/1003
* Support for Bamboo build server - https://github.com/fsharp/FAKE/pull/1015
* Added APPVEYOR_JOB_NAME appveyor environment variable - https://github.com/fsharp/FAKE/pull/1022
* Updated octopus sample to reflect 3.3.0 package - https://github.com/fsharp/FAKE/pull/1021
* Added functionality for deleting files and folders in FTP Helper - https://github.com/fsharp/FAKE/pull/1018
* BASH completion for FAKE targets - https://github.com/fsharp/FAKE/pull/1020
* BUGFIX: Fix case on MsBuild LogFile option - https://github.com/fsharp/FAKE/pull/1008
* BUGFIX: Fix git version on Mac - https://github.com/fsharp/FAKE/pull/1006

#### 4.9.1 - 11.11.2015
* Added support for channels to OctoTools - https://github.com/fsharp/FAKE/pull/1001
* BUGFIX: Create AssemblyInfo directory only where required - https://github.com/fsharp/FAKE/pull/997
* COSMETICS: Renamed confusing parameter in FSI helper - https://github.com/fsharp/FAKE/pull/1000

#### 4.8.0 - 04.11.2015
* Basic npm support - https://github.com/fsharp/FAKE/pull/993
* New RoboCopy helper - https://github.com/fsharp/FAKE/pull/988
* Option ignore failing tests DotCover https://github.com/fsharp/FAKE/pull/990
* Add code to replace new assemblyinfo attributes - https://github.com/fsharp/FAKE/pull/991
* Cleanup Registry helpers - https://github.com/fsharp/FAKE/pull/980
* FAKE.Deploy scans for default scripts - https://github.com/fsharp/FAKE/pull/981
* BUGFIX: Use WorkingDir in Paket helpers
* BUGFIX: support caching even when running RazorEngine as part of the build script - https://github.com/fsharp/FAKE/pull/979

#### 4.6.0 - 14.10.2015
* New Registry functions - https://github.com/fsharp/FAKE/pull/976
* Add attribute filters to DotCover - https://github.com/fsharp/FAKE/pull/974
* Always use FullName of nuspec for NuGet pack
* DotCover support for MSTest - https://github.com/fsharp/FAKE/pull/972
* Added new functions: replace and poke for inner xml - https://github.com/fsharp/FAKE/pull/970
* Adding TestFile helper - https://github.com/fsharp/FAKE/pull/962

#### 4.5.0 - 07.10.2015
* Ensure FSI-ASSEMBLY.dll path exists - https://github.com/fsharp/FAKE/pull/969
* New dotCover runner for Xunit2 - https://github.com/fsharp/FAKE/pull/965
* Make FAKE compatible with Microsoft Dynamics 2016
* Don't assume that mono is on the path for El Capitan - https://github.com/fsharp/FAKE/pull/963/files
* Better target handling - https://github.com/fsharp/FAKE/pull/954
* Ignore group lines in paket.references parser
* Revert breaking change in FCS
* Support for Android-MultiPackages - https://github.com/fsharp/FAKE/pull/964
* BUGFIX: Exclude long directories from globbing - https://github.com/fsharp/FAKE/pull/955
* BUGFIX: Encode script path in cache - https://github.com/fsharp/FAKE/pull/956

#### 4.4.0 - 11.09.2015
* iOSBuild relies on xbuild instead of mdtool - https://github.com/fsharp/FAKE/pull/945
* New method to return whether or not a value exists for a registry key - https://github.com/fsharp/FAKE/pull/944
* Extended ReportGeneratorHelper to add Badges report type - https://github.com/fsharp/FAKE/pull/943
* HockeyAppHelper download team restriction - https://github.com/fsharp/FAKE/pull/939
* Use TFS variables as fallback, fixes #933 - https://github.com/fsharp/FAKE/pull/937
* Deployment configurable timeouts in FAKE.Deploy - https://github.com/fsharp/FAKE/pull/927
* Fixed bug where only first 1024 bytes were uploaded using FTP - https://github.com/fsharp/FAKE/pull/932
* FAKE 4.2 or newer started with wrong Target - https://github.com/fsharp/FAKE/pull/931
* Better user input helper - https://github.com/fsharp/FAKE/pull/930
* Add support for new Xunit2 runner -noappdomain flag - https://github.com/fsharp/FAKE/pull/928

#### 4.3.0 - 26.08.2015
* FluentMigrator helper library - http://fsharp.github.io/FAKE/fluentmigrator.html

#### 4.2.0 - 24.08.2015
* Support for soft dependencies for targets - http://fsharp.github.io/FAKE/soft-dependencies.html
* Added support for builds within Team Foundation Server (and VSO) - https://github.com/fsharp/FAKE/pull/915
* New options in the SquirrelHelper - https://github.com/fsharp/FAKE/pull/910
* Logging improvement in Fake.Deploy - https://github.com/fsharp/FAKE/pull/914
* New RunTargetOrListTargets function - https://github.com/fsharp/FAKE/pull/921
* Added date to ReleaseNotes type definition - https://github.com/fsharp/FAKE/pull/917
* Added `createClientWithToken` & `createRelease` to Octokit.fsx - https://github.com/fsharp/FAKE/pull/913
* Fixed WatchChanges not properly removing subdirectories from watch list - https://github.com/fsharp/FAKE/pull/908
* Added ability to optionally pass in SiteId to configure IIS Site - https://github.com/fsharp/FAKE/pull/905
* Pass OutputDataReceived to logfn instead of trace in shellExec - https://github.com/fsharp/FAKE/pull/906 
* Add GetDependenciesForReferencesFile

#### 4.1.0 - 10.08.2015
* Using FSharp.Compiler.Server for F# 4.0
* Added Squirrel helpers to generate Squirrel installers - https://github.com/fsharp/FAKE/pull/899
* Added Ability to specify Identity for AppPool - https://github.com/fsharp/FAKE/pull/902
* Dynamics NAV: version helpers - https://github.com/fsharp/FAKE/pull/900
* Added ReleaseNotes to NugetHelper - https://github.com/fsharp/FAKE/pull/893
* BUGFIX: running from a network drive - https://github.com/fsharp/FAKE/pull/892
* BUGFIX: Align NUnitDomainModel with NUnit documentation - https://github.com/fsharp/FAKE/pull/897
* BUGFIX: Skip Octokit retry logic on Mono where it causes a crash - https://github.com/fsharp/FAKE/pull/895
* BUGFIX: FAKE removes mono debug file after cache is saved - https://github.com/fsharp/FAKE/pull/891
* BUGFIX: Nunit Domain Fix - https://github.com/fsharp/FAKE/pull/883
* BUGGFIX: Dynamic assembly handling for caching - https://github.com/fsharp/FAKE/pull/884
* BUGFIX: Loaded dlls versions are used to invalidate FAKE's cache - https://github.com/fsharp/FAKE/pull/882

#### 4.0.0 - 23.07.2015
* Automatic caching of FAKE build scripts - https://github.com/fsharp/FAKE/pull/859
* Added MSBuild properties to AndroidPackageParams - https://github.com/fsharp/FAKE/pull/863
* Add support for outputting NUnit style test result XML to Fake.Testing.XUnit2  - https://github.com/fsharp/FAKE/pull/870
* Add support for VS2015 VSTest executable - https://github.com/fsharp/FAKE/pull/877
* Add lock-dependencies parameter to Paket.Pack - https://github.com/fsharp/FAKE/pull/876

#### 3.36.0 - 13.07.2015
* NoLogo parameter for MSBuildHelper - https://github.com/fsharp/FAKE/pull/850
* Expose Globbing.isMatch for use by external code - https://github.com/fsharp/FAKE/pull/860
* VB6 dependency updater - https://github.com/fsharp/FAKE/pull/857
* Added BuildConfig/TemplateFile options to PaketHelper's Pack command - https://github.com/fsharp/FAKE/pull/854
* Add a UserInputHelper to allow interactive input - https://github.com/fsharp/FAKE/pull/858
* Look for MSTest in VS2015 location - https://github.com/fsharp/FAKE/pull/843
* Add caching to globbing 
* BUGFIX: Fix for single * glob not working - https://github.com/fsharp/FAKE/pull/836 
* BUGFIX: Get package version from nuspec file - https://github.com/fsharp/FAKE/pull/829
* Report all NuGet errors, even if ExitCode = 0

#### 3.35.0 - 09.06.2015
* Added Raygun.io helper - https://github.com/fsharp/FAKE/pull/826
* Re-added internal class generated for AssemblyInfo.vb - https://github.com/fsharp/FAKE/pull/827
* Allow test nUnit test assemblies containing SetupFixture attributes be compatible with NUnitParallel - https://github.com/fsharp/FAKE/pull/824
* Fix FtpHelper
* Trace no. of files in a patch
* CMake support improvements - https://github.com/fsharp/FAKE/pull/821
* Wix Helper Improvements - https://github.com/fsharp/FAKE/pull/818
* Wix Helper Improvements - https://github.com/fsharp/FAKE/pull/817
* Wix Helper Improvements - https://github.com/fsharp/FAKE/pull/815
* Added SemVerHelper.isValidSemVer - https://github.com/fsharp/FAKE/pull/811

#### 3.34.0 - 25.05.2015
* Support for CMake configuration and builds - https://github.com/fsharp/FAKE/pull/785
* New task to create C++ AssemblyInfo files - https://github.com/fsharp/FAKE/pull/812
* New environVarOrFail helper - https://github.com/fsharp/FAKE/pull/814
* New WiX helper functions - https://github.com/fsharp/FAKE/pull/804

#### 3.33.0 - 20.05.2015
* IMPORTANT: Rewrite of the xUnit tasks. Deprecating existing xUnit and xUnit2 tasks - https://github.com/fsharp/FAKE/pull/800
* Better NUnit docs - https://github.com/fsharp/FAKE/pull/802

#### 3.32.4 - 18.05.2015
* Add test adapter path to vs test params - https://github.com/fsharp/FAKE/pull/793
* BUGFIX: Fix WatchChanges on Mac, fix Dispose, improve Timer usage - https://github.com/fsharp/FAKE/pull/799
* REVERT: FCS simplification - https://github.com/fsharp/FAKE/pull/773
* BUGFIX: Don't use MSBuild from invalid path
* BUGFIX: Improved detection of MSBuild.exe on TeamCity - https://github.com/fsharp/FAKE/pull/789

#### 3.31.0 - 06.05.2015
* BUGFIX: close stdin in asyncShellExec to avoid hangs - https://github.com/fsharp/FAKE/pull/786
* Fix FAKE not working on machines with only F# 4.0 installed - https://github.com/fsharp/FAKE/pull/784
* Fix for watching files via relative paths - https://github.com/fsharp/FAKE/pull/782
* Fix package id parsing and avoid NPE when feed is missing some properties - https://github.com/fsharp/FAKE/pull/776

#### 3.30.1 - 29.04.2015
* FCS simplification - https://github.com/fsharp/FAKE/pull/773
* Paket push task runs in parallel - https://github.com/fsharp/FAKE/pull/768

#### 3.29.2 - 27.04.2015
* New file system change watcher - http://fsharp.github.io/FAKE/watch.html
* NuGet pack task treats non csproj files as nuspec files - https://github.com/fsharp/FAKE/pull/767
* New helpers to start and stop DynamicsNAV ServiceTiers
* Automatically replace Win7ToWin8 import files for Dynamics NAV during Import
* OpenSourced DynamicsNAV replacement helpers
* Use Microsoft.AspNet.Razor 2.0.30506 for FAKE.Deploy - https://github.com/fsharp/FAKE/pull/756
* New build parameter functions
* Fix http://stackoverflow.com/questions/29572870/f-fake-unable-to-get-fake-to-merge-placeholder-arguments-in-nuspec-file
* New environment variable helpers

#### 3.28.0 - 09.04.2015
* Don't run package restore during MSBuild run from FAKE - https://github.com/fsharp/FAKE/pull/753
* Added support for Mage's CertHash parameter - https://github.com/fsharp/FAKE/pull/750
* Force build server output in xUnit2 if the user wishes to - https://github.com/fsharp/FAKE/pull/749
* Reverting 0df4569b3bdeef99edf2eec6013dab784e338b7e due to backwards compat issues
* Improvements for FAKE.Deploy - https://github.com/fsharp/FAKE/pull/745
* Set debug flag on mono - https://github.com/fsharp/FAKE/pull/744

#### 3.27.0 - 07.04.2015
* New Android publisher - http://fsharp.github.io/FAKE/androidpublisher.html
* New Archive helpers allow to build zip, gzip, bzip2, tar, and tar.gz/tar.bz2 - https://github.com/fsharp/FAKE/pull/727
* Download Status Parameter for HockeyAppHelper - https://github.com/fsharp/FAKE/pull/741
* Added more parameters for HockeyApp Upload API - https://github.com/fsharp/FAKE/pull/723
* `NuGetPack` task allows to set framework references - https://github.com/fsharp/FAKE/pull/721
* New task `NuGetPackDirectly` works without template files.
* Find NuGet.exe in current folder (and sub-folders) first, then look in PATH - https://github.com/fsharp/FAKE/pull/718
* New tutorial about Vagrant - http://fsharp.github.io/FAKE/vagrant.html
* REVERTING: SystemRoot also works on mono - https://github.com/fsharp/FAKE/pull/706 (see https://github.com/fsharp/FAKE/issues/715)
* BUGFIX: Use DocumentNamespace for Nuspec files - https://github.com/fsharp/FAKE/pull/736
* BUGFIX: Display agent success / error messages in UI for FAKE.Deploy - https://github.com/fsharp/FAKE/pull/735
* BUGFIX: Add build directory for doc generation - https://github.com/fsharp/FAKE/pull/734

#### 3.26.0 - 25.03.2015
* Detect GitLab CI as build server - https://github.com/fsharp/FAKE/pull/712

#### 3.25.2 - 24.03.2015
* Look into PATH when scanning for NuGet.exe - https://github.com/fsharp/FAKE/pull/708
* SystemRoot also works on mono - https://github.com/fsharp/FAKE/pull/706
* Use EditorConfig - http://editorconfig.org/

#### 3.25.1 - 24.03.2015
* More AppVeyor properties added - https://github.com/fsharp/FAKE/pull/704

#### 3.25.0 - 23.03.2015
* Look into PATH when scanning for tools - https://github.com/fsharp/FAKE/pull/703

#### 3.24.0 - 22.03.2015
* BREAKING CHANGE: Better support for AssemblyMetadata in AssemblyInfoHelper - https://github.com/fsharp/FAKE/pull/694
* Added modules for building VB6 projects with SxS manifest - https://github.com/fsharp/FAKE/pull/697
* Use parameter quoting for Paket helpers

#### 3.23.0 - 12.03.2015
* BREAKING CHANGE: Adjusted Xamarin.iOS archive helper params - https://github.com/fsharp/FAKE/pull/693
* New operator </> allows to combine paths similar to @@ but with no trimming operations - https://github.com/fsharp/FAKE/pull/695

#### 3.22.0 - 12.03.2015
* Globbing allows to grab folders without a trailing slash
* Removed long time obsolete globbing functions

#### 3.21.0 - 11.03.2015
* FAKE allows to run parallel builds - http://fsharp.github.io/FAKE/parallel-build.html

#### 3.20.1 - 10.03.2015
* Proper source index - https://github.com/fsharp/FAKE/issues/678

#### 3.20.0 - 10.03.2015
* Always use FCS in FAKE and FSI in FAke.Deploy
* Modify VM size on a .csdef for Azure Cloud Services - https://github.com/fsharp/FAKE/pull/687
* Added ZipHelper.ZipOfIncludes - https://github.com/fsharp/FAKE/pull/686
* Added AppVeyorEnvironment.RepoTag & .RepoTagName - https://github.com/fsharp/FAKE/pull/685
* New tutorial about Azure Cloud Service - http://fsharp.github.io/FAKE/azurecloudservices.html
* Added basic support for creating Azure Cloud Services - http://fsharp.github.io/FAKE/apidocs/fake-azure-cloudservices.html
* Added metadata property for AssemblyInfoReplacementParams - https://github.com/fsharp/FAKE/pull/675

#### 3.18.0 - 04.03.2015
* Remvoved internal class generated in AssemblyInfo.Vb - https://github.com/fsharp/FAKE/pull/673
* Adding ability to control type library export (/tlb flag) of RegAsm - https://github.com/fsharp/FAKE/pull/668
* Adding ability to run nuget package restore on a visual studio solution - https://github.com/fsharp/FAKE/pull/662
* Add OwnerId, type docs, and better error handling for HockeyAppHelper - https://github.com/fsharp/FAKE/pull/661
* Don't report unit test failure twice to TeamCity - https://github.com/fsharp/FAKE/pull/659
* New tasks for `paket pack` and `paket push`- http://fsprojects.github.io/Paket/index.html
* Allow csproj being passed as a NuSpec file - https://github.com/fsharp/FAKE/pull/644 
* Helper for uploading mobile apps to HockeyApp - https://github.com/fsharp/FAKE/pull/656  
* SCPHelper does allow copying single files - https://github.com/fsharp/FAKE/issues/671
* BUGFIX: Paket helper should not submit the endpoint if no endpoint was given - https://github.com/fsharp/FAKE/issues/667
* BUGFIX: Paket helper should not override version for project packages - https://github.com/fsharp/FAKE/issues/666
* BUGFIX: Allow endpoint in push task - https://github.com/fsprojects/Paket/pull/652
* BUGFIX: Use correct apikey for paket push - https://github.com/fsharp/FAKE/pull/664

#### 3.17.0 - 12.02.2015
* Revert to fsi in Fake.Deploy - https://github.com/fsharp/FAKE/pull/653    
* Added MergeByHash option for OpenCover - https://github.com/fsharp/FAKE/pull/650
* New functions to replace text in one or more files using regular expressions - https://github.com/fsharp/FAKE/pull/649
* BUGFIX: Fix SpecFlow MSTest integration - https://github.com/fsharp/FAKE/pull/652
* BUGFIX: Fix TeamCity integration - https://github.com/fsharp/FAKE/pull/651

#### 3.15.0 - 07.02.2015
* New VSTest module for working with VSTest.Console - https://github.com/fsharp/FAKE/pull/648
* Add Verbose to argument list for NuGet update - https://github.com/fsharp/FAKE/pull/645
* BUGFIX: Fix jarsigner executing on Windows environment - https://github.com/fsharp/FAKE/pull/640
* Adding UploadTestResultsXml function to the AppVeyor module - https://github.com/fsharp/FAKE/pull/636
* Adding the NoDefaultExcludes NugGet parameter - https://github.com/fsharp/FAKE/pull/637
* Adding `SpecificMachines` option to OctoTools - https://github.com/fsharp/FAKE/pull/631
* Allow to run gacutil on mono
* Ignore unknown project references in MSBuild task - https://github.com/fsharp/FAKE/pull/630

#### 3.14.0 - 14.01.2015
* BUGFIX: Added a reset step before starting a deployment - https://github.com/fsharp/FAKE/pull/621
* Report fatal git errors to command line

#### 3.13.0 - 03.01.2015
* New FAKE.Lib nuget package which contains the FakeLib - https://github.com/fsharp/FAKE/pull/607
* New AppVeyor properties - https://github.com/fsharp/FAKE/pull/605
* Use FSharp.Core from NuGet - https://github.com/fsharp/FAKE/pull/602
* Build and deploy Azure web jobs - https://github.com/fsharp/FAKE/pull/613

#### 3.11.0 - 03.12.2014
* Dual-license under Apache 2 and MS-PL, with Apache as default - https://github.com/fsharp/FAKE/pull/598
* BUGFIX: FSC compilation fix - https://github.com/fsharp/FAKE/pull/601
* BUGFIX: Unescape special MSBuild characters - https://github.com/fsharp/FAKE/pull/600

#### 3.10.0 - 27.11.2014
* Support for MSBuild 14.0 - https://github.com/fsharp/FAKE/pull/595
* New C# compiler helper - https://github.com/fsharp/FAKE/pull/592/files
* Added support for NUnit Fixture parameter - https://github.com/fsharp/FAKE/pull/591
* OpenSourcing some DynamicsNAV helpers from gitnav
* BUGFIX: Fix 64bit mode
* BUGFIX: Dynamics NAV helper - "Ignored" tests should report the message

#### 3.9.0 - 07.11.2014
* Create a new package with a x64 version - https://github.com/fsharp/FAKE/pull/582
* Added a Xamarin.iOS Archiving helper - https://github.com/fsharp/FAKE/pull/581
* DynamicsNAV helper should use the correct ServiveTier

#### 3.8.0 - 30.10.2014
* [xUnit 2](http://xunit.github.io/) support - https://github.com/fsharp/FAKE/pull/575
* New RegistryKey helpers for a 64bit System - https://github.com/fsharp/FAKE/pull/580
* New XDTHelper - https://github.com/fsharp/FAKE/pull/556
* Version NAV 800 added - https://github.com/fsharp/FAKE/pull/576
* Feature/list targets in command line - http://fsharp.github.io/FAKE/specifictargets.html
* Use priority list for nuget.exe selection - https://github.com/fsharp/FAKE/issues/572
* BUGFIX: RoundhouseHelper was setting an incorrect switch for CommandTimoutAdmin - https://github.com/fsharp/FAKE/pull/566

#### 3.7.0 - 16.10.2014
* BUGFIX: --single-target didn't work
* NDepend support - https://github.com/fsharp/FAKE/pull/564

#### 3.6.0 - 14.10.2014
* FAKE got a new logo - https://github.com/fsharp/FAKE/pull/553
* Use Paket to handle dependencies - http://fsprojects.github.io/Paket/
* Single target mode --single-target - http://fsharp.github.io/FAKE/specifictargets.html
* New recursive copy functions - https://github.com/fsharp/FAKE/pull/559
* NuGetPack allows to manipulate nuspec files - https://github.com/fsharp/FAKE/pull/554
* Support for MSpec --xml parameter - https://github.com/fsharp/FAKE/pull/545
* Make GetPackageVersion work with Paket - http://fsprojects.github.io/Paket/
* Added missing schemaName parameter for Roundhouse helper - https://github.com/fsharp/FAKE/pull/551
* Roundhouse Cleanup - https://github.com/fsharp/FAKE/pull/550
* Update FSharp.Compiler.Service to 0.0.62
* BUGFIX: If site exists then the site will be modified by IISHelper with the given parameters - https://github.com/fsharp/FAKE/pull/548
* BUGFIX: Messages in FSC task to stderr stream can break the build - https://github.com/fsharp/FAKE/pull/546
* BUGFIX: Use AppVeyor's build version instead of the build number - https://github.com/fsharp/FAKE/pull/560

#### 3.5.0 - 19.09.2014
* Added new SignToolHelper - https://github.com/fsharp/FAKE/pull/535
* Look first in default path for a tool - https://github.com/fsharp/FAKE/pull/542
* Add support for MSBuild Distributed Loggers - https://github.com/fsharp/FAKE/pull/536
* Don't fail on nuget path scanning - https://github.com/fsharp/FAKE/pull/537

#### 3.4.0 - 28.08.2014
* New Xamarin.iOS and Xamarin.Android helpers - https://github.com/fsharp/FAKE/pull/527

#### 3.3.0 - 25.08.2014
* Using JSON.NET 6.0.4
* FAKE.Deploy switched to FCS - https://github.com/fsharp/FAKE/pull/519 
* FAKE.Deploy WorkDirectory fix - https://github.com/fsharp/FAKE/pull/520
* HipChat notification helper - https://github.com/fsharp/FAKE/pull/523
* Don't crash during tool discovery
* NuGet: support fallback framework groups - https://github.com/fsharp/FAKE/pull/514
* New pushd/popd command in FileUtils - https://github.com/fsharp/FAKE/pull/513
* New AppVeyor properties
* FSC - support of compilation for different versions of F#
* Provide env var access to --fsiargs build script args so works on FAKE
* Adding NGen Install task
* Allow to use gacutil
* Allow to use ngen.exe
* Allow to use all sn.exe features
* Adding DisableVerification for StrongNames
* Adding helpers which allow to strong name assemblies
* Allow to use empty MSBuild targets
* Adding setProcessEnvironVar and clearProcessEnvironVar
* Try to reference local nuspec in order to fix https://github.com/fsprojects/FSharp.TypeProviders.StarterPack/pull/33
* Better log messages to fix https://github.com/fsprojects/FSharp.TypeProviders.StarterPack/pull/33
* Fix fsiargs and -d options - https://github.com/fsharp/FAKE/pull/498 https://github.com/fsharp/FAKE/pull/500
* Change RemoveDuplicateFiles & FixMissingFiles to only save on change - https://github.com/fsharp/FAKE/pull/499

#### 3.2.0 - 07.07.2014
* BREAKING CHANGE: API for CreateAssemblyInfoWithConfig was set back to original version
  This resets the breaking change introduced in https://github.com/fsharp/FAKE/pull/471
* Automatic tool search for SpecFlowHelper - https://github.com/fsharp/FAKE/pull/496
* GuardedAwaitObservable was made public by accident - this was fixed
* Add support for remote service admin - https://github.com/fsharp/FAKE/pull/492

#### 3.1.0 - 04.07.2014
* New FSC helper allows to call F# compiler directly from FAKE - https://github.com/fsharp/FAKE/pull/485
* "CustomDictionary" support for FxCop - https://github.com/fsharp/FAKE/pull/489
* Check if file exists before delete in AssemblyInfoFile
* Use FSharp.Compiler.Service 0.0.58
* Report all targets if a target error occurs
* Use FSharp.Compiler.Service with better FSharp.Core resolution - https://github.com/fsharp/FSharp.Compiler.Service/issues/156
* Don't break in MSBuildHelper
* Put FSharp.Core.optdata and FSharp.Core.sigdata into nuget package
* Fixed TargetTracing
* Fixed SourceLinking of FAKE
* Added new exception trap for Fsi creation
* -br in command line will run debugger in F# scripts - https://github.com/fsharp/FAKE/pull/483
* Null check in NuGet helper - https://github.com/fsharp/FAKE/pull/482

#### 3.0.0 - 27.06.2014
* Use FSharp.Compiler.Service 0.0.57 instead of fsi.exe
* Better error message for registry access
* Fall back to 32bit registry keys if 64bit cannot be found
* Improved SqlServer Disconnect error message
* Log "kill all processes" only when needed
* Try to run as x86 due to Dynamics NAV problems
* Allow to use /gac for FxCop
* Make NuGet description fit into single line
* Use Nuget.Core 2.8.2
* Fix NUnitProcessModel.SeparateProcessModel - https://github.com/fsharp/FAKE/pull/474
* Improved CLI documentation - https://github.com/fsharp/FAKE/pull/472
* Added Visual Basic support to AssemblyFileInfo task and make Namespace optional in config - https://github.com/fsharp/FAKE/pull/471
* Added support for OctoTools ignoreExisting flag - https://github.com/fsharp/FAKE/pull/470
* OctoTools samples fixed - https://github.com/fsharp/FAKE/pull/468 https://github.com/fsharp/FAKE/pull/469
* Added support for FxCop /ignoregeneratedcode parameter - https://github.com/fsharp/FAKE/pull/467
* CreateAssemblyInfo works with nonexisting directories - https://github.com/fsharp/FAKE/pull/466

#### 2.18.0 - 11.06.2014
* New (backwards compat) CLI for FAKE that includes FSI cmd args passing - https://github.com/fsharp/FAKE/pull/455
* New updateApplicationSetting method - https://github.com/fsharp/FAKE/pull/462
* Support for msbuild /noconlog - https://github.com/fsharp/FAKE/pull/463
* RoundhouseHelper - https://github.com/fsharp/FAKE/pull/456
* Pass optional arguments to deployment scripts
* Support building source packages without project file
* Display messages when deploy fails
* Fix formatting in FAKE.Deploy docs
* Fix memory usage in FAKE.Deploy
* Increase WebClient's request timeout to 20 minutes - https://github.com/fsharp/FAKE/pull/442
* Mainly Layout fixes and disabling authenticate in FAKE.Deploy https://github.com/fsharp/FAKE/pull/441
* Deploy PDBs via nuget https://github.com/fsharp/FAKE/issues/435
* Release Notes parser should not drop asterisk at end of lines
* Corrected location of @files@ in nuspec sample
* Allow to report tests to AppVeyor
* fix appveyor msbuild logger
* Don't add Teamcity logger if not needed

#### 2.17.0 - 23.05.2014
* Fake.Deploy agent requires user authentication
* Remove AutoOpen von AppVeyor
* fix order of arguments in call to CopyFile
* Support MSTest test settings - https://github.com/fsharp/FAKE/pull/428
* If the NAV error file contains no compile errors return the length

#### 2.16.0 - 21.05.2014
* Promoted the master branch as default branch and removed develop branch
* Remove AutoOpen from TaskRunnerHelper
* Adding Metadata to AsssemblyInfo
* Analyze the Dynamics NAV log file and report the real error count
* Allow to retrieve version no. from assemblies
* Fix issue with symbol packages in NugetHelper
* Fix issues in the ProcessHelper - https://github.com/fsharp/FAKE/pull/412 and https://github.com/fsharp/FAKE/pull/411
* Allow to register BuildFailureTargets - https://github.com/fsharp/FAKE/issues/407
* UnionConverter no longer needed for Json.Net

#### 2.15.0 - 24.04.2014
* Handle problems with ProgramFilesX86 on mono - https://github.com/tpetricek/FsLab/pull/32
* Change the MSBuild 12.0 path settings according to https://github.com/tpetricek/FsLab/pull/32
* Silent mode for MSIHelper - https://github.com/fsharp/FAKE/issues/400

#### 2.14.0 - 22.04.2014
* Support for OpenCover - https://github.com/fsharp/FAKE/pull/398
* Support for ReportsGenerator - https://github.com/fsharp/FAKE/pull/399
* Adding AppVeyor environment variables 
* New BulkReplaceAssemblyInfoVersions task - https://github.com/fsharp/FAKE/pull/394
* Fixed default nuspec file
* "Getting started" tutorial uses better folder structure
* Allows explicit file specification on the NuGetParams Type
* Fix TypeScript output dir
* Add better docs for the TypeScript compiler.
* Don't call the TypeScript compiler more than once
* New parameters for TypeScript

#### 2.13.0 - 04.04.2014
* Enumerate the files lazily in the File|Directory active pattern
* Using Nuget 2.8.1
* Added TypeScript 1.0 support
* Added TypeScript support
* Fixed ProcessTestRunner
* Fixed mono build on Travis

#### 2.12.0 - 31.03.2014
* Add getDependencies to NugetHelper
* SourceLink support
* NancyFx instead of ASP.NET MVC for Fake.Deploy
* Allows to execute processes as unit tests.
* Adding SourceLinks
* Move release management back to the local machine (using this document)
* Allow to run MsTest test in isolation
* Fixed Nuget.packSymbols
* Fixed bug in SemVer parser
* New title property in Nuspec parameters
* Added option to disabled FAKE's automatic process killing
* Better AppyVeyor integration
* Added ability to define custom MSBuild loggers
* Fix for getting the branch name with Git >= 1.9
* Added functions to write and delete from registry
* NUnit NoThread, Domain and StopOnError parameters
* Add support for VS2013 MSTest
* Lots of small fixes

#### 2.2
* Created new packages on nuget:
	* Fake.Deploy - allows to use FAKE scripts in deployment.
	* Fake.Experimental - new stuff where we aren't sure if we want to support it.
	* Fake.Gallio - contains the Gallio runner support.
	* Fake.SQL - Contains tasks for SQL Server.
	* Fake.Core - All the basic features and FAKE.exe.
* Created documentation and tutorials - see http://fsharp.github.io/FAKE/
* New tasks:
	* Added ReleaseNotes parser
	* Added Dynamics NAV helper
	* Added support for MSTest and fixie
	* Parallel NUnit task
	* New AssemblyInfoFile task
	* Support for Octopus Deploy
	* Support for MAGE
	* Suppport for Xamarin's xpkg
	* Many other new tasks
* Fake.Boot
* New Globbing system
* Tons of bug fixes
* Bundles F# 3.0 compiler and FSI.

#### 1.72.0.0

* "RestorePackages" allows to restore nuget packages

#### 1.70.0.0

* FAKE nuget package comes bundles with a fsi.exe
* Self build downloads latest FAKE master via nuget

#### 1.66.1.0

* Fixed bug where FAKE.Deploy didn't run the deploy scripts where used as a windows service
* It's possible to add file loggers for MSBuild
* Fixed path resolution for fsi on *nix
* BREAKING CHANGE: Removed version normalization from NuGet package creation
* Fixes for NUNit compatibility on mono 
* Fixes in ProcessHelper for mono compatibility
* Fixes in the mono build
* Improved error reporting in Fake.exe
* Added a SpecFlow helper
* Fixed some issues in file helper routines when working with no existing directory chain

#### 1.64.1.0

* Fixed bug where FAKE didn't run the correct build script

#### 1.64.0.0

* New conditional dependency operator =?>
* BREAKING CHANGE: Some AssemblyInfo task parameters are now option types. See type hints.

#### 1.62.0.0

* New RegAsm task, allows to create TLBs from a dll.
* New MSI task, allows to install or uninstall msi files.
* StringHelper.NormalizeVersion fixed for WiX.

#### 1.58.9.0

* Allow to choose specific nunit-console runner.

#### 1.58.6.0

* Using nuget packages for mspec.
* FAKE tries to kill all MSBuild and FSI processes at the end of a build.

#### 1.58.1.0

* Removed message system for build output. Back to simpler tracing.

#### 1.58.0.0

* ReplaceAssemblyInfoVersions task allows to replace version info in AssemblyVersion-files
* New task ConvertFileToWindowsLineBreaks

#### 1.56.10.0

* Allows to build .sln files

#### 1.56.0.0

* Allows to publish symbols via nuget.exe
* Autotrim trailing .0 from version in order to fullfill nuget standards.

#### 1.54.0.0

* If the publishment of a Nuget package fails, then FAKE will try it again.
* Added Changelog.markdown to FAKE deployment
* Added RequireExactly helper function in order to require a specific nuget dependency.
* NugetHelper.GetPackageVersion - Gets the version no. for a given package in the packages folder.
* EnvironmentHelper.getTargetPlatformDir - Gets the directory for the given target platform.

#### 1.52.0.0

* Some smaller bugfixes
* New dependency syntax with ==> and <=>
* Tracing of StackTrace only if TargetHelper.PrintStackTraceOnError was set to true

#### 1.50.0.0

* New task DeleteDirs allows to delete multiple directories.
* New parameter for NuGet dependencies.

#### 1.48.0.0

* Bundled with docu.exe compiled against .Net 4.0.
* Fixed docu calls to run with full filenames.
* Added targetplatform, target and log switches for ILMerge task.
* Added Git.Information.getLastTag() which gets the last git tag by calling git describe.
* Added Git.Information.getCurrentHash() which gets the last current sha1.

#### 1.46.0.0

* Fixed Nuget support and allows automatic push.

#### 1.44.0.0

* Tracing of all external process starts.
* MSpec support.
