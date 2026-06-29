# Lists recent Serilog JSON log files in logs/ (newest first).

param(
    [int] $Count = 10,
    [string] $LogsDirectory = "logs"
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptRoot

if (-not [System.IO.Path]::IsPathRooted($LogsDirectory)) {
    $LogsDirectory = Join-Path $projectRoot $LogsDirectory
}

if (-not (Test-Path -LiteralPath $LogsDirectory)) {
    Write-Host "No logs directory yet: $LogsDirectory"
    exit 0
}

Get-ChildItem -LiteralPath $LogsDirectory -Filter "log-*.json" |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First $Count |
    Format-Table Name, Length, LastWriteTime -AutoSize
