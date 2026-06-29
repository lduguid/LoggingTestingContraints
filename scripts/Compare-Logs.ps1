# Compares two Serilog compact JSON log files from logs/.
# Default mode strips @t timestamps and compares behavioral content only.

param(
    [Parameter(Position = 0)]
    [string] $Log1,

    [Parameter(Position = 1)]
    [string] $Log2,

    [switch] $Raw,
    [switch] $Latest,
    [string] $LogsDirectory = "logs"
)

$ErrorActionPreference = "Stop"

function Get-NormalizedLogLines {
    param([string] $Path)

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Log file not found: $Path"
    }

    Get-Content -LiteralPath $Path | ForEach-Object {
        if ($Raw) {
            $_
        }
        else {
            $_ -replace '"@t":"[^"]+"', '"@t":"*"'
        }
    }
}

function Resolve-LatestLogPair {
    param([string] $Directory)

    if (-not (Test-Path -LiteralPath $Directory)) {
        throw "Logs directory not found: $Directory"
    }

    $files = Get-ChildItem -LiteralPath $Directory -Filter "log-*.json" |
        Sort-Object LastWriteTime -Descending

    if ($files.Count -lt 2) {
        throw "Need at least two log files in '$Directory' to use -Latest. Found: $($files.Count)"
    }

    return @($files[1].FullName, $files[0].FullName)
}

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptRoot

if (-not [System.IO.Path]::IsPathRooted($LogsDirectory)) {
    $LogsDirectory = Join-Path $projectRoot $LogsDirectory
}

if ($Latest) {
    $pair = Resolve-LatestLogPair -Directory $LogsDirectory
    $Log1 = $pair[0]
    $Log2 = $pair[1]
    Write-Host "Comparing latest two runs:"
    Write-Host "  Baseline: $($pair[0])"
    Write-Host "  Current:  $($pair[1])"
}
elseif (-not $Log1 -or -not $Log2) {
    throw "Usage: Compare-Logs.ps1 <baseline-log> <current-log> [-Raw]  OR  Compare-Logs.ps1 -Latest [-Raw]"
}

if (-not [System.IO.Path]::IsPathRooted($Log1)) {
    $Log1 = Join-Path $projectRoot $Log1
}
if (-not [System.IO.Path]::IsPathRooted($Log2)) {
    $Log2 = Join-Path $projectRoot $Log2
}

$mode = if ($Raw) { "raw (including timestamps)" } else { "behavioral (timestamps normalized)" }
Write-Host "Mode: $mode"
Write-Host ""

$left = Get-NormalizedLogLines -Path $Log1
$right = Get-NormalizedLogLines -Path $Log2

$diff = Compare-Object -ReferenceObject $left -DifferenceObject $right

if (-not $diff) {
    Write-Host "No differences - logs match ($mode)."
    exit 0
}

Write-Host ('Differences found ({0} lines):' -f $diff.Count)
$diff | Format-Table -AutoSize SideIndicator, InputObject

exit 1
