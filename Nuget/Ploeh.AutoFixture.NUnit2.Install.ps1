param($installPath, $toolsPath, $package, $project)

$projectPath = Split-Path -Parent $project.FullName
$assemblyInfo = "Properties\AssemblyInfo.cs"
$projAssemblyInfo = Join-Path $projectPath $assemblyInfo
$addinContent = "[assembly: NUnit.Framework.RequiredAddin(Ploeh.AutoFixture.NUnit2.Constants.AutoDataExtension)]";

$hasAddinContent = Get-Content $projAssemblyInfo | Select-String $addinContent -SimpleMatch -Quiet

if(-not $hasAddinContent) 
{
	Write-Host "Apended $addinContent to $assemblyInfo"
	Add-Content $projAssemblyInfo $addinContent
}