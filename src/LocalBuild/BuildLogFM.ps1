# Get all .pubxml files
$scriptDirectory = $PSScriptRoot
Write-Host "Script directory: $scriptDirectory"
# Navigate up one directory level
$srcFolder = Split-Path -Path $scriptDirectory -Parent

# Output the parent directory path
Write-Output "The parent directory is: $srcFolder"
$binaryDirectory = $scriptDirectory+"\Binaries\"
Write-Output "The binary directory is: $binaryDirectory"

$codeFolder = $srcFolder+"\LogFM\"
Write-Output "The code directory is: $codeFolder"

cd $codeFolder

$publishProfiles = Get-ChildItem -Path ".\Properties\PublishProfiles\" -Filter "*.pubxml"

# Iterate over each publish profile and run the dotnet publish command
foreach ($profile in $publishProfiles) {
    $profileName = $profile.Name
    Write-Host "Publishing with profile: $profileName"
    dotnet publish /p:PublishProfile=Properties\PublishProfiles\$profileName
}
# Define the parent directory containing the subfolders

# Get all subdirectories in the parent directory
$subdirectories = Get-ChildItem -Path $binaryDirectory -Directory

# Loop through each subdirectory and create a zip file
foreach ($subdir in $subdirectories) {
    $zipFilePath = Join-Path $binaryDirectory ($subdir.Name + ".zip")
    Compress-Archive -Path $subdir.FullName -DestinationPath $zipFilePath -Force
    Write-Host "Created zip: $zipFilePath"
}

cd $scriptDirectory