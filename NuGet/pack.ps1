#by Lo Sauer, 2016
#invoke the script as pack.ps1 projectname dllname nugetid

$project = $args[0] # e.g. "Singleton"
$dllname = $args[1] # e.g. "Core.Singleton"
$nugetid = $args[2] # e.g. "CSharp.Portable-Singleton"

$rootself = (split-path -parent $MyInvocation.MyCommand.Definition)
$root = "$rootself\.."
$releaseNotesPath = "$rootself\releaseNotes.txt"

Write-Host "Root is: $root; Inovcation Path: $rootself"
$versionStr = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$root\$project\bin\Release\$dllname.dll").FileVersion.ToString()
Write-Host "Set $nugetid.nuspec version tag: $versionStr"
Set-Content ..\VERSION $versionStr

$env:EnableNuGetPackageRestore = 'true'

$content = (Get-Content $root\NuGet\$nugetid.nuspec) 
$content = $content -replace '{version}',$versionStr

#set release notes
if([System.IO.File]::Exists("$releaseNotesPath")){
	$releaseNotes = [IO.File]::ReadAllText("$releaseNotesPath")
	$content = $content -replace '{releaseNotes}', $releaseNotes
}

$content | Out-File $root\NuGet\$nugetid.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\NuGet\$nugetid.compiled.nuspec

$env:NuGetPackageName = "$nugetid.$versionStr.nupkg"