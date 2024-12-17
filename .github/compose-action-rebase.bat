@echo off
REM Configuration: GitHub repository and paths
set REPO_URL=https://github.com/Natel210/SimpleComposeActions
set BRANCH=main
set ACTIONS_FOLDER=actions
set TARGET_FOLDER=actions

REM Temporary directory setup
set TEMP_DIR=%TEMP%\github_actions_download

REM Clean up existing temporary folder
if exist "%TEMP_DIR%" (
    echo Cleaning up previous temporary files...
    rmdir /s /q "%TEMP_DIR%"
)

REM Create a temporary folder
mkdir "%TEMP_DIR%"

REM Download the actions folder from GitHub
echo Downloading actions folder from GitHub...
curl -L "%REPO_URL%/archive/%BRANCH%.zip" -o "%TEMP_DIR%\repo.zip"

REM Extract the downloaded ZIP file
echo Extracting downloaded files...
powershell -Command "Expand-Archive -Path '%TEMP_DIR%\repo.zip' -DestinationPath '%TEMP_DIR%'"

REM Find the actions folder within the extracted content
for /d %%D in ("%TEMP_DIR%\SimpleComposeActions-%BRANCH%") do (
    set SRC_FOLDER=%%D\%ACTIONS_FOLDER%
)

REM Clean up the existing target actions folder
if exist "%TARGET_FOLDER%" (
    echo Clearing existing actions folder...
    rmdir /s /q "%TARGET_FOLDER%"
)
mkdir "%TARGET_FOLDER%"

REM Copy the actions content to the target folder
echo Copying actions contents to %TARGET_FOLDER%...
xcopy /s /e "%SRC_FOLDER%" "%TARGET_FOLDER%\" > nul

REM Clean up temporary files
echo Cleaning up temporary files...
rmdir /s /q "%TEMP_DIR%"

echo Actions have been successfully updated in %TARGET_FOLDER%.
pause
