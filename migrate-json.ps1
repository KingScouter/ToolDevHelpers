param (
    [string]$inputFile,
    [string]$outputFile
)

# Read the JSON file
$jsonContent = Get-Content -Path $inputFile -Raw | ConvertFrom-Json

# Convert toolConfigs object to an array
$jsonContent.toolConfigs = $jsonContent.toolConfigs.PSObject.Properties.Value

# Convert back to JSON and save to the output file
$jsonContent | ConvertTo-Json -Depth 10 | Set-Content -Path $outputFile

Write-Host "Migration completed. Output saved to $outputFile"
