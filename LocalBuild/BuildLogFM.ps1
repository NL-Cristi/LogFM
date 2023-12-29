# Get all .pubxml files
$publishProfiles = Get-ChildItem -Path ".\Properties\PublishProfiles\" -Filter "*.pubxml"

# Iterate over each publish profile and run the dotnet publish command
foreach ($profile in $publishProfiles) {
    $profileName = $profile.Name
    Write-Host "Publishing with profile: $profileName"
    dotnet publish /p:PublishProfile=Properties\PublishProfiles\$profileName
}
# Define the parent directory containing the subfolders
$parentDirectory = "..\Binaries\FormatLog4Net\"

# Get all subdirectories in the parent directory
$subdirectories = Get-ChildItem -Path $parentDirectory -Directory

# Loop through each subdirectory and create a zip file
foreach ($subdir in $subdirectories) {
    $zipFilePath = Join-Path $parentDirectory ($subdir.Name + ".zip")
    Compress-Archive -Path $subdir.FullName -DestinationPath $zipFilePath
    Write-Host "Created zip: $zipFilePath"
}
