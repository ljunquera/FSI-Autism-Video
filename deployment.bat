@echo off
echo changing directory...
echo %DEPLOYMENT_SOURCE%
echo %DEPLOYMENT_TARGET%
cd %DEPLOYMENT_SOURCE%\player
echo installing packages...
npm install && npm run build && xcopy %DEPLOYMENT_SOURCE%\player\dist\player %DEPLOYMENT_TARGET% /Y /s /i
