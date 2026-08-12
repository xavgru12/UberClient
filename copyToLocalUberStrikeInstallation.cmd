@echo off
call "%~dp0deploy-base.cmd" "%~dp0Assembly-CSharp\bin\Release" "C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed"
exit /b %ERRORLEVEL%
