function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { Write-Error $_ -ErrorAction Continue; exit 1 }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

# Install .Net
ExecSafe { & $PSScriptRoot\tools\Install-DotNet.ps1 -RootFolder $PSScriptRoot }

# Restore workloads
Write-Host "Restoring all workloads"
ExecSafe { & $env:DOTNET_EXE workload install macos -v:quiet  }
Get-ChildItem $PSScriptRoot\src\ -rec |? { $_.FullName.EndsWith('proj') -and ($_.FullName.Contains('Mac') -or $_.FullName.Contains('iOS') -or $_.FullName.Contains('Android') -or $_.FullName.Contains('Windows') -or $_.FullName.Contains('Linux')) } |% {
    $ProjectPath = $_.FullName;
    Write-Host "Restoring workload for $($ProjectPath)..."
    ExecSafe { & $env:DOTNET_EXE workload restore -v:quiet --project $ProjectPath  }
}
Write-Host "Done."
Write-Output "---------------------------------------"

# Restore NuGet solution dependencies
Write-Host "Restoring all dependencies"
Get-ChildItem $PSScriptRoot\src\ -rec |? { $_.FullName.EndsWith('.sln') } |% {
    $SolutionPath = $_.FullName;
    Write-Host "Restoring packages for $($SolutionPath)..."
    ExecSafe { & $env:DOTNET_EXE restore -v:quiet $SolutionPath  }
}
Write-Host "Done."
Write-Output "---------------------------------------"