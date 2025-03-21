$powerToysPluginFolder = "$($env:LOCALAPPDATA)\Microsoft\PowerToys\PowerToys Run\Plugins\ToolDevHelpers"
$defaultInstallLocation = "C:\DevelopmentSandbox"

$installLocation = Read-Host -Prompt "Enter the install location (default: $defaultInstallLocation)"
if ($installLocation -eq "") {
  $installLocation = $defaultInstallLocation
}
$sandboxLocation = "$installLocation\ToolDevHelpers"

Write-Output "Stop PowerToys"
# Stop PowerToys
Stop-Process -Name PowerToys

Start-Sleep 2

Write-Output "Remove old plugin"
# Delete old plugin folder
if (Test-Path -Path $powerToysPluginFolder) {
  Remove-Item -Path $powerToysPluginFolder -Force
}
if (Test-Path -Path $sandboxLocation) {
  Remove-Item -Path $sandboxLocation -Recurse
}


Write-Output "Copy folder"
# # Copy new plugin folder
Copy-Item -Path . -Destination $sandboxLocation -Recurse -Exclude "*.ps1"

cmd /c mklink /J "$powerToysPluginFolder" "$sandboxLocation"

Start-Sleep 2

Write-Output "Start PowerToys"
# Restart PowerToys
Start-Process -FilePath "$($env:LOCALAPPDATA)\PowerToys\PowerToys.exe"