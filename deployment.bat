@echo off
echo changing directory...
echo %DEPLOYMENT_SOURCE%
echo %DEPLOYMENT_TARGET%
echo %DEPLOYMENT_TEMP%
cd %DEPLOYMENT_TEMP%\player
echo installing packages...
npm install && npm run build && xcopy %DEPLOYMENT_TEMP%\player\dist\player\* %DEPLOYMENT_TARGET% /Y /s /i && del %DEPLOYMENT_TARGET%\hostingstart.html
