@echo off
cls

SET SCRIPT_DIR=%~dp0
SET BUILD_DIR=%SCRIPT_DIR%\build
SET TOOLS_DIR=%BUILD_DIR%\tools
SET NUGET_PATH=%TOOLS_DIR%\nuget.exe

IF NOT EXIST %TOOLS_DIR%\ (
  mkdir %TOOLS_DIR%
)

IF NOT EXIST %NUGET_PATH% (
  echo Downloading NuGet.exe ...
  powershell -Command "Start-BitsTransfer -Source https://dist.nuget.org/win-x86-commandline/v4.3.0/nuget.exe -Destination %NUGET_PATH%"
)

IF NOT EXIST "%TOOLS_DIR%\NUnit.Runners.2.6.2\" (
  %NUGET_PATH% install "NUnit.Runners" -Version 2.6.2 -OutputDirectory %TOOLS_DIR% 
)

dotnet tool restore

echo Running FAKE Build...
dotnet tool run fake --silent run %SCRIPT_DIR%\build.fsx %*
