@echo off

powershell.exe -NoProfile -ExecutionPolicy unrestricted -command ".\build.ps1 -task %1 ;exit $LASTEXITCODE"

if %ERRORLEVEL% == 0 goto OK
	echo Error running build. 
exit /B %ERRORLEVEL%

:OK