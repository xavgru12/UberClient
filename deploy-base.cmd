@echo off
setlocal
set "SOURCE=%~1"
set "DEST=%~2"
call :deploy "%SOURCE%" "%DEST%"
set "EXITCODE=%ERRORLEVEL%"
endlocal & exit /b %EXITCODE%

:deploy
set "SRC=%~1"
set "DST=%~2"
if not exist "%SRC%" goto nosource
if not exist "%DST%" goto nodest
echo Deploying to: %DST%
xcopy "%SRC%\*" "%DST%" /E /Y /I
if errorlevel 1 goto fail
echo Copy complete.
exit /b 0

:nosource
echo Source folder not found: %SRC%
exit /b 1

:nodest
echo Destination folder not found: %DST%
exit /b 1

:fail
echo Copy failed.
exit /b 2