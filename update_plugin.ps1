$pluginFolder = ".\Community.PowerToys.Run.Plugin.JSLHelpers\bin\x64\Release\net9.0-windows10.0.22621.0\"
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
Copy-Item -Path $pluginFolder -Destination $powerToysPluginFolder -Recurse

Start-Sleep 2

Write-Output "Start PowerToys"
# Restart PowerToys
Start-Process -FilePath "$($env:LOCALAPPDATA)\PowerToys\PowerToys.exe"