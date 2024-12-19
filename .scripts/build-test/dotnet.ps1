param (
    [string]$ProjectFilePath,
    [string]$BuildConfiguration
)

if (-not $ProjectFilePath -or -not $BuildConfiguration) {
    Write-Host "Usage: .\dotnet-build.ps1 -ProjectFilePath <ProjectFilePath> -BuildConfiguration <BuildConfiguration>"
    exit 1
}

# 빌드 실행
Write-Host "Building project: $ProjectFilePath with configuration: $BuildConfiguration"
dotnet build $ProjectFilePath -c $BuildConfiguration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed."
    exit 1
}

Write-Host "Build succeeded."
