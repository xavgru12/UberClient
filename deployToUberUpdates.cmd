@echo off
set "SCRIPT_DIR=%~dp0"
set "UBERUPDATES=C:\code\UberUpdates"

:parse
if "%~1"=="" goto parsed
if /i not "%~1"=="--path" goto usage
set "UBERUPDATES=%~2"
shift
shift
goto parse

:usage
echo Usage: %~nx0 [--path ^<UberUpdates repo folder^>]
exit /b 3

:parsed
if not exist "%UBERUPDATES%" goto nodest

call "%SCRIPT_DIR%deploy-base.cmd" "%SCRIPT_DIR%Assembly-CSharp\bin\Release" "%UBERUPDATES%\Windows\UberStrike_Data\Managed"
if errorlevel 1 exit /b %ERRORLEVEL%

cd /d "%UBERUPDATES%"
echo Running bin.exe...
"%UBERUPDATES%\bin.exe"
if errorlevel 1 exit /b %ERRORLEVEL%
echo Running create_zip.py...
python "%UBERUPDATES%\create_zip.py"
exit /b %ERRORLEVEL%

:nodest
echo Error: UberUpdates folder not found: %UBERUPDATES%
echo Use --path to specify the correct UberUpdates repo folder.
exit /b 1
