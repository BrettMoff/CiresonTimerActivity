 #Set-Location $PSScriptRoot
 # cd Z:\Source\Code\Cireson.Timer.Activity\Cireson.Timer.Activity\Cireson.Timer.Activity.General\ManagementPack

$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot


if (Test-Path -Path ".\Cireson.Timer.Activity.Library.mp") {
    Write-Host "Deleting MP..."
    DEL ".\Cireson.Timer.Activity.Library.mp"
}

if (Test-Path -Path ".\Cireson.Timer.Activity.Library.mpb") {
    Write-Host "Deleting MPB..."
    DEL ".\Cireson.Timer.Activity.Library.mpb"
}

if (Test-Path -Path ".\Cireson.Timer.Activity.WPF.dll") {
    Write-Host "Deleting Cireson.Timer.Activity.WPF DLL..."
    DEL ".\Cireson.Timer.Activity.WPF.dll"
}

if (Test-Path -Path ".\Xceed.Wpf.Toolkit.dll") {
    Write-Host "Deleting Xceed.Wpf.Toolkit DLL..."
    DEL ".\Xceed.Wpf.Toolkit.dll"
}

Write-Host "Copying DLLs..."
if (Test-Path -Path "..\..\CiresonTimerActivity.WPF\bin\Debug\Xceed.Wpf.Toolkit.dll") {
    Write-Host "Copying Xceed.Wpf.Toolkit.DLL..."
    xcopy /y "..\..\CiresonTimerActivity.WPF\bin\Debug\Xceed.Wpf.Toolkit.dll" .
}
else {
    Write-Host "Can not find Xceed.Wpf.Toolkit DLL."
}

if (Test-Path -Path "..\..\CiresonTimerActivity.WPF\bin\Debug\Cireson.Timer.Activity.WPF.dll") {
    Write-Host "Copying Cireson.Timer.Activity.WPF.dll..."
    xcopy /y "..\..\CiresonTimerActivity.WPF\bin\Debug\Cireson.Timer.Activity.WPF.dll" .
}
else {
    Write-Host "Can not find Cireson.Timer.Activity.WPF.dll DLL."
}


$ErrorActionPreference = "Continue"
#Seal the Incident MPB
Write-Host "Sealing Incident MP with fastseal..."
& .\fastseal.exe Cireson.Timer.Activity.Library.xml /keyfile CiresonDevOps.snk /company Cireson

if (Test-Path -Path ".\Cireson.Timer.Activity.Library.mp") {
    Write-Host "Sealing MPB with new-mpbfile..."
    & .\new-mpbfile.ps1 -mpFile  Cireson.Timer.Activity.Library.mp -mpbname Cireson.Timer.Activity.Library -computername localhost
}

if (Test-Path -Path ".\Cireson.Timer.Activity.Library.mpb") {
    Write-Host "Complete."

    #import the MP.
    Write-Host "Importing MP..."
    Import-SCManagementPack -FullName  ".\Cireson.Timer.Activity.Library.mpb"
    Write-Host "MP imported..."
}
else{
    Write-Host "Creating the MP failed."
}




$ErrorActionPreference = "Stop"





