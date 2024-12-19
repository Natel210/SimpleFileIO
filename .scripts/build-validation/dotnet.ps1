param (
    [string]$ProjectFilePath,
    [string]$BuildConfiguration,
    [string]$ResultFile = $null
)

. ./.scripts/colors.ps1

if (-not $ProjectFilePath -or -not $BuildConfiguration) {
    Write-Host "${BackgroundDarkRed}${TextRed}No arguments.${Reset}"
    Write-Host "${BackgroundDarkRed}${TextRed}Usage: .\dotnet-build.ps1 -ProjectFilePath <ProjectFilePath> -BuildConfiguration <BuildConfiguration>${Reset}"
    exit 1
}

$isError = $false
$output = "${BackgroundLightGray}${TextWhite}Building project: $ProjectFilePath with configuration: $BuildConfiguration${Reset}`n"

# Capture dotnet build output
$buildOutput = & dotnet build $ProjectFilePath -c $BuildConfiguration 2>&1
$buildExitCode = $LASTEXITCODE

if ($buildExitCode -ne 0) {
    $output += "${BackgroundDarkRed}${TextRed}Build failed.${Reset}`n"
    $output += "${TextRed}Error Output:${Reset}`n"

    # Process each line in the build output
    foreach ($line in $buildOutput -split "`n") {
        $output += "${TextLightGray} - $line${Reset}`n"
    }

    $summary = "${BackgroundDarkRed}${TextRed}Build failed.${Reset}"
    $isError = $true
} else {
    $summary = "${BackgroundDarkGreen}${TextGreen}Build Test Completed Successfully.${Reset}"

    # Process each line in the build output
    foreach ($line in $buildOutput -split "`n") {
        $output += "${TextLightGray} - $line${Reset}`n"
    }
}

# Combine summary and output
$result = "$summary`n$output"

# Print or save result
if ([string]::IsNullOrEmpty($ResultFile)) {
    Write-Host -NoNewline $result
} else {
    $result | Set-Content -Path $ResultFile -Encoding UTF8
}

# Exit with error if build failed
if ($isError) {
    exit 1
}
