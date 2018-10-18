@echo off
echo This script grabs any changes you've made to the repo, asks for your branch name, and saves your updates to GitHub.
pause
git status
Echo Are you on the correct branch?
set /p correct="y/n:"
if %correct%==y (
git add -u
git status
set /p change="Add a description on what you've changed:"
git commit -m '%change%'
timeout /t 1
git push
)
if %correct%==n (
git branch -v -a
Echo Please ignore anything that starts with remote
Echo Type in your branch, ex 'FirstLast-develop'
set /p branch="Enter branch:"
Echo you entered %branch%
timeout /t 2
git checkout %branch%
timeout /t 2
git add -u
git status
set /p change="Add a description on what you've changed:"
git commit -m '%change%'
timeout /t 1
git push
)
echo Success, if you look on the GitHub page you'll see that your branch now has your updated changes!
pause
exit