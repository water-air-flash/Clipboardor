@echo off
echo type "commit" or "update"
:: cd "curl"

set GIT_PATH="C:\Program Files\Git\bin\git.exe"
:: set TGIT_PATH="C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe"
set BRANCH = "origin"

:P
set ACTION=
set /P ACTION=Action: %=%
if "%ACTION%"=="c" (
 ::  %TGIT_PATH% /command:commit /logmsg:"Auto-committed on %date%" /closeonend:3
 	%GIT_PATH% add .
 	%GIT_PATH% commit -m "changed"
	%GIT_PATH% pull %BRANCH%
	%GIT_PATH% push %BRANCH%
)
if "%ACTION%"=="u" (
	%GIT_PATH% pull %BRANCH%
)
if "%ACTION%"=="exit" exit /b
goto P