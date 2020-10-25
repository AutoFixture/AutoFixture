@echo off
cls

SET SCRIPT_DIR=%~dp0

dotnet tool restore

echo Running FAKE Build...
dotnet tool run fake --silent run %SCRIPT_DIR%\build.fsx %*
