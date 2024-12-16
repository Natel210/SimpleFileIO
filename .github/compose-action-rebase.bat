@echo off
REM 설정: GitHub 리포지토리 및 경로
set REPO_URL=https://github.com/Natel210/SimpleComposeActions
set BRANCH=main
set ACTIONS_FOLDER=actions
set TARGET_FOLDER=actions

REM 임시 디렉토리 설정
set TEMP_DIR=%TEMP%\github_actions_download

REM 기존 TEMP 폴더 정리
if exist "%TEMP_DIR%" (
    echo Cleaning up previous temporary files...
    rmdir /s /q "%TEMP_DIR%"
)

REM 임시 폴더 생성
mkdir "%TEMP_DIR%"

REM actions 폴더 다운로드
echo Downloading actions folder from GitHub...
curl -L "%REPO_URL%/archive/%BRANCH%.zip" -o "%TEMP_DIR%\repo.zip"

REM 압축 해제
echo Extracting downloaded files...
powershell -Command "Expand-Archive -Path '%TEMP_DIR%\repo.zip' -DestinationPath '%TEMP_DIR%'"

REM 복사할 actions 폴더 찾기
for /d %%D in ("%TEMP_DIR%\SimpleComposeActions-%BRANCH%") do (
    set SRC_FOLDER=%%D\%ACTIONS_FOLDER%
)

REM Target 폴더 정리
if exist "%TARGET_FOLDER%" (
    echo Clearing existing actions folder...
    rmdir /s /q "%TARGET_FOLDER%"
)
mkdir "%TARGET_FOLDER%"

REM actions 내용 복사
echo Copying actions contents to %TARGET_FOLDER%...
xcopy /s /e "%SRC_FOLDER%" "%TARGET_FOLDER%\"

REM 정리
echo Cleaning up temporary files...
rmdir /s /q "%TEMP_DIR%"

echo Actions have been successfully updated in %TARGET_FOLDER%.
pause