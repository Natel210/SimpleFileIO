param (
    [string]$ProjectFilePath,
    [string]$BuildConfiguration
)

. ./.scripts/colors.ps1

if (-not $ProjectFilePath -or -not $BuildConfiguration) {
    Write-Host "${BackgroundDarkRed}${TextRed}No arguments.${Reset}"
    Write-Host "${BackgroundDarkRed}${TextRed}Usage: .\dotnet-build.ps1 -ProjectFilePath <ProjectFilePath> -BuildConfiguration <BuildConfiguration>${Reset}"
    exit 1
}

Write-Host "${BackgroundDarkBlue}${TextBlue}Building project: $ProjectFilePath with configuration: $BuildConfiguration${Reset}"
dotnet build $ProjectFilePath -c $BuildConfiguration

if ($LASTEXITCODE -ne 0) {
    Write-Host "${BackgroundDarkRed}${TextRed}Build failed.${Reset}"
    exit 1
}

Write-Host "${BackgroundDarkGreen}${TextGreen}Build Test Completed Successfully.${Reset}"
