@echo off
setlocal

set "SOURCE=%~dp0Assembly-CSharp\bin\Release"
set "DEST=C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed"

if not exist "%SOURCE%" goto nosource
if not exist "%DEST%" goto nodest

echo Deploying to: %DEST%
xcopy "%SOURCE%\*" "%DEST%" /E /Y /I
if errorlevel 1 goto fail

echo Copy complete.
goto end

:nosource
echo Source folder not found: %SOURCE%
exit /b 1

:nodest
echo Destination folder not found: %DEST%
exit /b 1

:fail
echo Copy failed.
exit /b 1

:end
endlocal
