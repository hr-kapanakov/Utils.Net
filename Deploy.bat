@echo off

::set /p UserInputPath=What Directory would you like?
::echo %UserInputPath%
::if %UserInputPath%=="" echo "empty"
::SET UserInputPath=
::goto END

set DEVENVFOLDER=%ProgramFiles(x86)%

if not exist "%DEVENVFOLDER%" set DEVENVFOLDER=%ProgramFiles%

set DEVENV2017="%DEVENVFOLDER%\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe"
set VSTEST2017="%DEVENVFOLDER%\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe"

set PROJECT=Utils.Net

echo.
echo Increasing assembly version...
powershell -ExecutionPolicy Bypass ./IncreaseVersion.ps1 %PROJECT%\Properties\AssemblyInfo.cs

echo.
echo Building %PROJECT%...
%DEVENV2017% %PROJECT%.sln /Rebuild Release
if %errorlevel% NEQ 0 goto BUILDERROR

echo.
echo Testing %PROJECT%...
%VSTEST2017% %PROJECT%Tests\bin\Release\%PROJECT%Tests.dll
if %errorlevel% NEQ 0 goto TESTSERROR


echo.
echo Generating documentation...
MarkdownGenerator.exe %PROJECT%\bin\Release\%PROJECT%.dll %PROJECT%.wiki


echo.
echo Packing %PROJECT%...
nuget.exe pack .\Utils.Net\ -properties Configuration=Release -symbols
if %errorlevel% NEQ 0 goto PUSHERROR

echo.
echo Pushing to NuGet...
set /p APIKEY=Please enter NuGet.org API key:
if "%APIKEY%" == "" goto PUSHERROR
nuget.exe push .\MyUtils.Net.*.symbols.nupkg %APIKEY% -Source https://api.nuget.org/v3/index.json
SET APIKEY=


goto END

:BUILDERROR
echo.
echo Error: Build errors occured.

:TESTSERROR
echo.
echo Error: Tests errors occured.

:PUSHERROR
echo.
echo Error: Cannot push to NuGet.org

:END
echo.
echo Clear NuGet package...
del *.nupkg

echo.
pause