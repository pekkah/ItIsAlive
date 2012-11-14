#
Properties {
	$build_dir = Split-Path $psake.build_script_file	
	$build_artifacts_dir = "$build_dir\build-artifacts\"
	$build_artifacts_bin_dir="$build_artifacts_dir\binaries\"
	$code_dir = "$build_dir\sources"
	$solution = "Sakura.sln"
	$config = "Release"
	$pack_dir= "$build_dir\nuget"
	$version = (git describe --tags --candidates 1).split('-')[0]
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends PackAll

Task BuildSolution -Depends Clean, Build

Task Build -Depends Clean {	
	Write-Host "Building $solution" -ForegroundColor Green
	Exec { msbuild "$code_dir\$solution" /t:Build /p:Configuration=$config /v:m /p:OutDir=$build_artifacts_bin_dir } 
}

Task PackAll -Depends BuildSolution {
	Pack "$pack_dir\Sakura\Sakura-Template.nuspec" "$build_artifacts_bin_dir\Sakura.dll" $build_artifacts_dir
	Pack "$pack_dir\Sakura.Extensions.NHibernate\Sakura.Extensions.NHibernate-Template.nuspec" "$build_artifacts_bin_dir\Sakura.Extensions.NHibernate.dll" $build_artifacts_dir
}

Task Clean {
	Write-Host "Creating build-artifacts directory" -ForegroundColor Green
	if (Test-Path $build_artifacts_dir) 
	{	
		rd $build_artifacts_dir -rec -force | out-null
	}
	
	mkdir $build_artifacts_dir | out-null
	mkdir $build_artifacts_bin_dir | out-null
	
	Write-Host "Cleaning $solution" -ForegroundColor Green
	Exec { msbuild "$code_dir\$solution" /t:Clean /p:Configuration=$config /v:quiet } 
}

Function Pack {
	param($spec_template, $assembly, $output_dir)
	$spec_file = "$spec_template".Replace("-Template", "")
	$spec_dir = Split-Path $spec_file
	
	Write-Host "Packaging $spec_file $version" -ForegroundColor Green
	Write-Host "Working directory : $spec_dir"
	get-content $spec_template | ForEach-Object { $_ -replace "{version}", $version } | Set-Content $spec_file
	
	$lib_dir = "$spec_dir\lib\"
	
	if (Test-Path $lib_dir)
	{
		rd $lib_dir -Force -Recurse
	}
	
	mkdir "$lib_dir\net40" | out-null
	
	cp $assembly "$lib_dir\net40\"
	
    exec { sources\.nuget\NuGet.exe pack $spec_file -OutputDirectory $output_dir }
	
	rd $lib_dir -Force -Recurse
	rd $spec_file
}