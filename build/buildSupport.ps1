Set-Variable buildOnly -option Constant -value 2
Set-Variable minor -option Constant -value 3
Set-Variable major -option Constant -value 1

function Increment-VersionNumber([String] $assemblyInfoFile, [string] $nuspecFile, [int] $incrementType)  
{            
    $assemblyPattern = "[0-9]+(\.([0-9]+|\*)){1,3}"  
    $assemblyVersionPattern = 'AssemblyVersion\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'                          
              
    $rawVersionNumberGroup = get-content $assemblyInfoFile | select-string -pattern $assemblyVersionPattern | select -first 1 | % { $_.Matches }              
    $rawVersionNumber = $rawVersionNumberGroup.Groups[1].Value  
                    
    $versionParts = $rawVersionNumber.Split('.')

    if($incrementType -eq $buildOnly){
        $versionParts[3] = 0
        $versionParts[2] = ([int]$versionParts[2]) + 1
    } elseif($incrementType -eq $minor) {
        $versionParts[3] = 0
        $versionParts[2] = 0
        $versionParts[1] = ([int]$versionParts[1]) + 1
    } elseif($incrementType -eq $minor) {
        $versionParts[3] = 0
        $versionParts[2] = 0
        $versionParts[1] = 0
        $versionParts[0] = ([int]$versionParts[0]) + 1
    }
        
    $updatedAssemblyVersion = "{0}.{1}.{2}.{3}" -f $versionParts[0], $versionParts[1], $versionParts[2], $versionParts[3]  
 
    (Get-Content $assemblyInfoFile) | ForEach-Object {  
            % {$_ -replace $assemblyPattern, $updatedAssemblyVersion }               
        } | Set-Content $assemblyInfoFile
            
    $Spec = [xml](get-content $nuspecFile)
    $Spec.package.metadata.version = $updatedAssemblyVersion
    $Spec.Save($nuspecFile)                          
}