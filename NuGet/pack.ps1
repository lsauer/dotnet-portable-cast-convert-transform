#by Lo Sauer, 2016
#invoke the script as pack.ps1 projectname dllname nugetid

$project = $args[0] # e.g. "Singleton"
$dllname = $args[1] # e.g. "Core.Singleton"
$nugetid = $args[2] # e.g. "CSharp.Portable-Singleton"

$rootself = (split-path -parent $MyInvocation.MyCommand.Definition)
$root = "$rootself\.."
$releaseNotesPath = "$rootself\releaseNotes.txt"
$buildPath = "\$env:Configuration"
$addX64 = ""
if($env:Platform -eq "x64") { 
	$buildPath = "\$env:Platform\$env:Configuration" 
	$addX64 = "x64"
}
if($env:Platform -eq "x86") { $buildPath = "\$env:Platform\$env:Configuration" }
Write-Host "The build path is: $buildPath"
 
Write-Host "Root is: $root; Inovcation Path: $rootself"
$versionStr = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$root\$project\bin$buildPath\$dllname.dll").FileVersion.ToString()
Write-Host "Set $nugetid.nuspec version tag: $versionStr"
Set-Content ..\VERSION $versionStr

$env:EnableNuGetPackageRestore = 'true'

$content = (Get-Content $root\NuGet\$nugetid.nuspec) 
$content = $content -replace '{version}',$versionStr

#set release notes
if([System.IO.File]::Exists("$releaseNotesPath")){
	$releaseNotes = [IO.File]::ReadAllText("$releaseNotesPath")
	$content = $content -replace '{releaseNotes}', $releaseNotes

	# append the last view changleog entries
	cd $env:APPVEYOR_BUILD_FOLDER
	$changeLog = git log HEAD~20..HEAD --graph --all --date=relative --no-color --pretty='format:%x09 %ad %d %s (%aN)' | Out-String
	$content = $content -replace '{CHANGELOG_RLEASE}', $changeLog
}

$content | Out-File $root\NuGet\$nugetid$addX64.compiled.nuspec

& $root\NuGet\NuGet.exe pack $root\NuGet\$nugetid$addX64.compiled.nuspec

$env:NuGetPackageName = "$nugetid$addX64.$versionStr.nupkg"