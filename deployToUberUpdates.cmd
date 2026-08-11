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
if not exist "%UBERUPDATES%" (
    echo Error: UberUpdates folder not found: %UBERUPDATES%
    echo Use --path to specify the correct UberUpdates repo folder.
    exit /b 4
)
if not exist "%UBERUPDATES%\Windows\UberStrike_Data\Managed" (
    echo Error: Destination folder not found: %UBERUPDATES%\Windows\UberStrike_Data\Managed
    echo Use --path to specify the correct UberUpdates repo folder.
    exit /b 5
)

set "DEST=%UBERUPDATES%\Windows\UberStrike_Data\Managed"
echo Deploying to: %DEST%
copy /Y "%SCRIPT_DIR%Assembly-CSharp\bin\Release\Assembly-CSharp.dll" "%DEST%\"
if errorlevel 1 (
    echo Copy failed.
    exit /b 6
)
copy /Y "%SCRIPT_DIR%Assembly-CSharp\bin\Release\Assembly-CSharp-firstpass.dll" "%DEST%\"
if errorlevel 1 (
    echo Copy failed.
    exit /b 6
)
copy /Y "%SCRIPT_DIR%Assembly-CSharp\bin\Release\UnityEngine.dll" "%DEST%\"
if errorlevel 1 (
    echo Copy failed.
    exit /b 6
)
echo Copy complete.

cd /d "%UBERUPDATES%"
echo Running bin.exe...
"%UBERUPDATES%\bin.exe"
if errorlevel 1 exit /b 7
echo Running create_zip.py...
python "%UBERUPDATES%\create_zip.py"
if errorlevel 1 exit /b 8
exit /b 0