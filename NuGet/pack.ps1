#by Lo Sauer, 2016
#invoke the script as pack.ps1 projectname dllname nugetid

$project = $args[0] # e.g. "Singleton"
$dllname = $args[1] # e.g. "Core.Singleton"
$nugetid = $args[2] # e.g. "CSharp.Portable-Singleton"

$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
Write-Host "Root is: $root"
$version = [System.Reflection.Assembly]::LoadFile("$root\$project\bin\Release\$dllname.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Set $nugetid.nuspec version tag: $versionStr"

$env:EnableNuGetPackageRestore = 'true'

$content = (Get-Content $root\NuGet\$nugetid.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\NuGet\$nugetid.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\NuGet\$nugetid.compiled.nuspec
