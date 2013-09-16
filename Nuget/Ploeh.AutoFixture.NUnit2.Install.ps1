param($installPath, $toolsPath, $package, $project)

$projectPath = Split-Path -Parent $project.FullName
$projAssemblyInfo = Join-Path $projectPath "Properties\AssemblyInfo.cs"
$addinContent = "[assembly: NUnit.Framework.RequiredAddin(Ploeh.AutoFixture.NUnit2.Constants.AutoDataExtension)]";

$c = Get-Content $projAssemblyInfo

if(-not $c.Contains($addinContent)) 
{
	Add-Content $projAssemblyInfo $addinContent
}