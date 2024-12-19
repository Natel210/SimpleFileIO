param (
    [string]$ProjectFilePath,
    [string]$BuildConfiguration
)

$TextWhite = "`e[38;5;15m"
$TextRed = "`e[38;5;196m"
$TextBlue = "`e[38;5;27m"
$TextGreen = "`e[38;5;46m"
$TextLightGray = "`e[38;5;245m"
$TextDarkRed = "`e[38;5;124m"
$TextDarkBlue = "`e[38;5;20m"
$TextDarkGreen = "`e[38;5;28m"

$BackgroundLightGray = "`e[48;5;245m"
$BackgroundDarkRed = "`e[48;5;52m"
$BackgroundDarkBlue = "`e[48;5;19m"
$BackgroundDarkGreen = "`e[48;5;22m"

$Reset = "`e[0m"

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
