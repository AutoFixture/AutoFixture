function Get-VsVersion ()
{
    if ($env:VS140COMNTOOLS -ne $null)
    {
        " /property:VisualStudioVersion=14.0"
    }
    elseif ($env:VS120COMNTOOLS -ne $null)
    {
        " /property:VisualStudioVersion=12.0"
    }
}

Start-Process -NoNewWindow -Wait -FilePath ${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MsBuild.exe -ArgumentList ('BuildRelease.msbuild', (Get-VsVersion))