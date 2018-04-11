[CmdletBinding()]
param
(
    [ValidateNotNullOrEmpty()]
    [string] $Path,

    [ValidateNotNullOrEmpty()]
    [string] $Destination,

    [ValidateNotnull()]
    [string[]] $ItemsToDelete = @('_rels', 'package', '[Content_Types].xml')
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"
if($Path -eq $Destination) {
    throw "-Path and -Destination have to be different"
}

Add-Type -AssemblyName System.IO.Compression.FileSystem

[System.IO.FileInfo[]]$nupkgFiles = Get-ChildItem -Path $Path -File -Filter '*.nupkg'
if(!$nupkgFiles) {
    throw "Found no NUPKG files in: $Path"
}

Write-Output "Nuget Packages:`n$($nupkgFiles | ForEach-Object { $_.FullName } | Out-String)"

[void](Remove-item -Path $Destination -Force -Recurse -ErrorAction SilentlyContinue)
[void](New-Item -Path $Destination -ItemType Directory -Force -ErrorAction SilentlyContinue)
foreach($nupkgFile in $nupkgFiles) {
    Write-Output "Extracting $($nupkgFile.FullName)..."
    $nupkgDestination = Join-Path -Path $Destination -ChildPath $nupkgFile.BaseName
    [System.IO.Compression.ZipFile]::ExtractToDirectory($nupkgFile.FullName, $nupkgDestination)
}

Write-Output "Looking for nuspec files under $Destination"
[System.IO.DirectoryInfo[]]$nuspecDirectories = Get-ChildItem -Recurse -Path $Destination -Filter '*.nuspec' | ForEach-Object { $_.Directory }
foreach($directory in $nuspecDirectories) {
    Get-ChildItem -Path $directory.FullName | Where-Object { $_.Name -in $ItemsToDelete } | ForEach-Object {
        Write-Output "Deleting $($_.FullName)"
        $_ | Remove-Item -Force -ErrorAction Continue -Recurse
    }

    $libFolder = Join-Path -Path $directory.FullName -ChildPath 'lib'
    if(Test-Path -Path $libFolder) {
        Write-Output "Lib founder found: $libFolder"
        Get-ChildItem -Path $libFolder -Directory | Where-Object { $_.BaseName -like '*%2B*'} | ForEach-Object {
            $newName = $_.BaseName -replace '%2B', '+'
            Write-Output "Renaming '$($_.BaseName)' to '$newName'"
            Rename-Item -Path $_.FullName -NewName $newName
        }
    }
}

Write-Output "Finished running $($MyInvocation.MyCommand.Name)"