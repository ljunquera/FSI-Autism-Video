@echo off
echo changing directory...
cd %DEPLOYMENT_SOURCE%\player
echo installing packages...
npm install && npm run build && xcopy %DEPLOYMENT_SOURCE%\dist\player %DEPLOYMENT_TARGET% /Y /s /i
