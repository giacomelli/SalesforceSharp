#tool "nuget:?package=NUnit.ConsoleRunner&version=3.10.0"

var target = Argument("target", "Default");
var version = "1.0.1";

Task("Build")
    .Does(() =>
{
    var solution = "./src/SalesforceSharp.sln";

    NuGetRestore(solution);
    
    MSBuild(solution, 
        new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
            Configuration = "Release"
        }
    );

    MSBuild(solution, 
        new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
            Configuration = "Debug"
        }
    );
});

Task("Test")
    .Does(() =>
{
    NUnit3("./src/SalesforceSharp.UnitTests/bin/Debug/SalesforceSharp.UnitTests.dll");
});

Task("Package")
    .Does(() =>{
        var nuspecFile = "./src/SalesforceSharp/bin/Release/SalesforceSharp.nuspec";
        
        CopyFile("./src/SalesforceSharp.nuspec", nuspecFile);

        NuGetPack(nuspecFile, new NuGetPackSettings{
            Version = version, 
            Verbosity = NuGetVerbosity.Detailed,
        }
    );
});

Task("Default")    
    .IsDependentOn("Build")
    .IsDependentOn("Test")    
    .IsDependentOn("Package")
	.Does(()=> { 
});

RunTarget(target);