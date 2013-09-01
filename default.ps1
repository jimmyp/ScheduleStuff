. .\buildSupport.ps1

Properties {
	$root_dir = Split-Path $psake.build_script_file
    $build_dir = "$root_dir\build"
    $pack_dir = "$build_dir\pack"
	$build_artifacts_dir = "$build_dir\artifacts"
	$code_dir = "$root_dir\src\ScheduleStuff"
    $output_dir = "$code_dir\bin\Release"

    $assemblyVersion_file = "$code_dir\Properties\AssemblyInfo.cs"
    $nuspec_file = "$root_dir\ScheduleStuff.nuspec"
}

Task Default -Depends Build

Task PublishMinor -Depends IncrementVersionMinor, Pack{
}

Task IncrementVersionMinor {
    Increment-VersionNumber $assemblyVersion_file $nuspec_file $minor
}

Task Pack -Depends DoBuild{
    mkdir "$pack_dir\lib\NET40"
    cp "$output_dir\ScheduleStuff.dll" "$pack_dir\lib\NET40"
    cp "$root_dir\ScheduleStuff.nuspec" $pack_dir
    & "$root_dir\tools\NuGet\Nuget.exe" pack "$pack_dir\ScheduleStuff.nuspec"

    $package_file = gci *.nupkg
    #$package_file_split = $package_file.Name.Split('.')
    #$package_id = $package_file_split[0]
    #$package_version = "{0}.{1}.{2}.{3}" -f $package_file_split[1], $package_file_split[2], $package_file_split[3], $package_file_split[4]
    
    mv $package_file  $build_artifacts_dir
           
    $package_file_name = $package_file.Name
    $package_for_publishing =  "$build_artifacts_dir\$package_file_name"

    exec {  & "$root_dir\tools\NuGet\Nuget.exe" push $package_for_publishing -NonInteractive }
    

    #git add .
    #git commit -m "Prepared new nuget package"
}

Task Build -Depends IncrementVersionBuild, DoBuild{
}

Task IncrementVersionBuild {
    Increment-VersionNumber $assemblyVersion_file $nuspec_file $buildOnly
}

Task DoBuild -Depends Clean {	
	Write-Host "Building ScheduleStuff.csproj" -ForegroundColor Green
	Exec { msbuild "$code_dir\ScheduleStuff.csproj" /t:Build /p:Configuration=Release /v:quiet } 
}


Task Clean {
	Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
	if (Test-Path $pack_dir) 
	{	
		rd $pack_dir -rec -force | out-null
	}
	
	mkdir $pack_dir | out-null

    if (Test-Path $output_dir) 
	{	
		rd $output_dir -rec -force | out-null
	}
	
	mkdir $output_dir | out-null
	
	Write-Host "Cleaning ScheduleStuff.csproj" -ForegroundColor Green
	Exec { msbuild "$code_dir\ScheduleStuff.csproj" /t:Clean /p:Configuration=Release /v:quiet } 
}