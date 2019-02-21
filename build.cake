#tool "nuget:?package=NUnit.Runners&version=2.6.4"

var target = Argument("target", "Default");
var solutionDir = "src";

Task("Build")
    .Does(() =>
{
    MSBuild("./src/SalesforceSharp.sln", 
        new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
            Configuration = "Release"
        }
    );

    MSBuild("./src/SalesforceSharp.sln", 
        new MSBuildSettings {
            Verbosity = Verbosity.Minimal,
            Configuration = "Debug"
        }
    );
});

Task("Test")
    .Does(() =>
{
    NUnit("./src/SalesforceSharp.UnitTests/bin/Debug/SalesforceSharp.UnitTests.dll");
    NUnit("./src/SalesforceSharp.FunctionalTests/bin/Debug/SalesforceSharp.FunctionalTests.dll");
});

Task("Package")
    .Does(() =>{
        NuGetPack("./src/SalesforceSharp.nuspec", new NuGetPackSettings{
            Version = "0.7.5", 
            Verbosity = NuGetVerbosity.Detailed,
        }
    );
});

Task("Default")
    .IsDependentOn("Package")
    .IsDependentOn("Test")
    .IsDependentOn("Build")
	.Does(()=> { 
});

RunTarget(target);