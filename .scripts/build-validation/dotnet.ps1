param (
    [string]$ProjectFilePath,
    [string]$BuildConfiguration,
    [string]$ResultFile = $null
)

# Import colors if necessary
. ./.scripts/tools/colors.ps1

if (-not $ProjectFilePath -or -not $BuildConfiguration)
{
    Write-Host "${BackgroundDarkRed}${TextRed}No arguments.${Reset}"
    Write-Host "${BackgroundDarkRed}${TextRed}Usage: .\dotnet-build.ps1 -ProjectFilePath <ProjectFilePath> -BuildConfiguration <BuildConfiguration> -ResultFile <ResultFile>${Reset}"
    exit 1
}

# Initialize variables
$isError = $false
$output = "${BackgroundLightGray}${TextWhite}Building project: $ProjectFilePath with configuration: $BuildConfiguration${Reset} os:Windows`n"



# Execute build command and capture output
$buildOutput = & dotnet build $ProjectFilePath -c $BuildConfiguration 2>&1
$buildExitCode = $LASTEXITCODE

if ($buildExitCode -ne 0)
{
    # Handle build failure
    $output += "${BackgroundDarkRed}${TextRed}Build failed.${Reset}`n"
    $output += "${TextRed}Error Output:${Reset}`n"

    # Process each line in build output
    foreach ($line in $buildOutput -split "`n")
    {
        $output += "${TextLightGray} - $line${Reset}`n"
    }

    $summary = "${BackgroundDarkRed}${TextRed}Build failed.${Reset}"
    $isError = $true
}
else
{
    # Handle build success
    $summary = "${BackgroundDarkGreen}${TextGreen}Build Test Completed Successfully.${Reset}"

    # Process each line in build output
    foreach ($line in $buildOutput -split "`n")
    {
        $output += "${TextLightGray} - $line${Reset}`n"
    }
}

# Combine summary and output
$result = "$output`n$summary"

# Output or save result
if ([string]::IsNullOrEmpty($ResultFile))
{
    Write-Host $result
}
else
{
    # Ensure directory for result file exists
    $resultDirectory = Split-Path -Path $ResultFile
    if (-not (Test-Path -Path $resultDirectory))
    {
        New-Item -ItemType Directory -Path $resultDirectory -Force | Out-Null
    }
    $result | Set-Content -Path $ResultFile -Encoding UTF8
}

# Exit with error if build failed
if ($isError)
{
    exit 1
}
