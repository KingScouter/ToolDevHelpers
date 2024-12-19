$powerToysPluginFolder = "$($env:LOCALAPPDATA)\Microsoft\PowerToys\PowerToys Run\Plugins\JSLHelpers"

Write-Output "Stop PowerToys"
# Stop PowerToys
Stop-Process -Name PowerToys

Start-Sleep 2

Write-Output "Remove old plugin"
# Delete old plugin folder
if (Test-Path -Path $powerToysPluginFolder) {
  Remove-Item -Path $powerToysPluginFolder -Recurse
}

Write-Output "Copy folder"
# Copy new plugin folder
Copy-Item -Path . -Destination $powerToysPluginFolder -Recurse -Exclude "*.ps1"

Start-Sleep 2

Write-Output "Start PowerToys"
# Restart PowerToys
Start-Process -FilePath "$($env:LOCALAPPDATA)\PowerToys\PowerToys.exe"