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
call "%SCRIPT_DIR%deploy-base.cmd" "%SCRIPT_DIR%Assembly-CSharp\bin\Release" "%UBERUPDATES%\Windows\UberStrike_Data\Managed"
exit /b %ERRORLEVEL%