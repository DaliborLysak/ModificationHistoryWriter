@echo off
setlocal

set PROJECT=ModificationHistoryWriterForm\ModificationHistoryWriterForm.csproj
set OUTPUT=publish

echo Building release...
dotnet publish %PROJECT% ^
    --configuration Release ^
    --runtime win-x64 ^
    --no-self-contained ^
    --output %OUTPUT% ^
    /p:PublishSingleFile=false

if %ERRORLEVEL% neq 0 (
    echo.
    echo Build FAILED.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Build OK. Output: %~dp0%OUTPUT%
pause
