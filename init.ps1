function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { Write-Error $_ -ErrorAction Continue; exit 1 }

# Install .Net
ExecSafe { & $PSScriptRoot\tools\Install-DotNet.ps1 -RootFolder $PSScriptRoot }

# Restore NuGet solution dependencies
Write-Host "Restoring all dependencies"
Get-ChildItem $PSScriptRoot -rec |? { $_.FullName.EndsWith('.sln') } |% {
    $SolutionPath = $_.FullName;
    Write-Host "Restoring packages for $($SolutionPath)..."
    ExecSafe { & $env:DOTNET_EXE restore -v:quiet $SolutionPath  }
}
Write-Host "Done."
Write-Output "---------------------------------------"