Import-Module ..\tools\psake\psake.psm1 -ErrorAction SilentlyContinue
Invoke-psake .\default.ps1 PublishMinor
Remove-Module psake -ErrorAction SilentlyContinue