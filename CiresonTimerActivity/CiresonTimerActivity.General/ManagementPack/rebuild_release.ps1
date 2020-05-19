 #Set-Location $PSScriptRoot
 # cd Z:\Source\Code\CiresonTimerActivity\CiresonTimerActivity\CiresonTimerActivity.General\ManagementPack

$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot


if (Test-Path -Path ".\CiresonTimerActivity.Library.mp") {
    Write-Host "Deleting MP..."
    DEL ".\CiresonTimerActivity.Library.mp"
}

if (Test-Path -Path ".\CiresonTimerActivity.Library.mpb") {
    Write-Host "Deleting MPB..."
    DEL ".\CiresonTimerActivity.Library.mpb"
}

if (Test-Path -Path ".\CiresonTimerActivity.WPF.dll") {
    Write-Host "Deleting CiresonTimerActivity.WPF DLL..."
    DEL ".\CiresonTimerActivity.WPF.dll"
}

if (Test-Path -Path ".\CiresonTimerActivity.WPF.dll") {
    Write-Host "Deleting Xceed.Wpf.Toolkit DLL..."
    DEL ".\Xceed.Wpf.Toolkit.dll"
}

Write-Host "Copying DLLs..."
xcopy /y "..\..\CiresonTimerActivity.WPF\bin\Debug\CiresonTimerActivity.WPF.dll" .
xcopy /y "..\..\CiresonTimerActivity.WPF\bin\Debug\Xceed.Wpf.Toolkit.dll" .

$ErrorActionPreference = "Continue"
#Seal the Incident MPB
Write-Host "Sealing Incident MP with fastseal..."
& .\fastseal.exe CiresonTimerActivity.Library.xml /keyfile CiresonDevOps.snk /company Cireson

if (Test-Path -Path ".\CiresonTimerActivity.Library.mp") {
    Write-Host "Sealing MPB with new-mpbfile..."
    & .\new-mpbfile.ps1 -mpFile  CiresonTimerActivity.Library.mp -mpbname CiresonTimerActivity.Library -computername localhost
}

if (Test-Path -Path ".\CiresonTimerActivity.Library.mpb") {
    Write-Host "Complete."

    #import the MP.
    Write-Host "Importing MP..."
    Import-SCManagementPack -FullName  ".\CiresonTimerActivity.Library.mpb"
    Write-Host "MP imported..."
}
else{
    Write-Host "Creating the MP failed."
}




$ErrorActionPreference = "Stop"





