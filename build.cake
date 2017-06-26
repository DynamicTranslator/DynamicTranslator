#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=NuGet.CommandLine"

#addin "Cake.Json"
#addin "Cake.FileHelpers"
#addin "nuget:?package=NuGet.Core"
#addin "nuget:?package=Cake.ExtendedNuGet"

#l "common.cake"

using NuGet;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var projectName = "DynamicTranslator";
var solution = "./" + projectName + ".sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"tools");
var branch = Argument("branch", EnvironmentVariable("APPVEYOR_REPO_BRANCH"));

var testFileRegex = $"**/bin/{configuration}/*Tests*.dll";
var testProjectNames = new List<string>()
                    {
                        "DynamicTranslator.Application.Tests",
                        "DynamicTranslator.Tests"
                    };

var nugetPath = toolpath + "/NuGet.CommandLine/tools/nuget.exe";

 
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        Information("Current Branch is:" + branch);
        CleanDirectories("./src/**/bin");
        CleanDirectories("./src/**/obj");
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solution, new NuGetRestoreSettings
                  	{
                  		NoCache = true,
                  		Verbosity = NuGetVerbosity.Detailed,
                  		ToolPath = nugetPath
                  	});
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild(solution, c=> c.Configuration = configuration);
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach(var testProject in testProjectNames)
        {
           var testFile = GetFiles($"**/bin/{configuration}/{testProject}*.dll").First();
           Information(testFile);
           XUnit2(testFile.ToString(), new XUnit2Settings { });
        }
    });

Task("Coverage")
    .IsDependentOn("Run-Unit-Tests")
    .Does(()=>
    {
      Information("Coverage...");
      /*var coverSettings = new DotCoverCoverSettings()
                              .WithFilter("-:*Tests");
      var coverageResultSsV4 = new FilePath("./dotcover/dotcoverSsV4.data");
      DotCoverCover(ctx =>
                    ctx.XUnit2("./SsV4Test/NodaTime.Serialization.ServiceStackText.UnitTests.dll"),
                        coverageResultSsV4,
                        coverSettings
                   );
      var htmlReportFile = new FilePath("./dotcover/dotcover.html");
      var reportSettings = new DotCoverReportSettings { ReportType = DotCoverReportType.HTML};
      DotCoverReport(mergedData, htmlReportFile, reportSettings);
      StartProcess("powershell", "start file:///" + MakeAbsolute(htmlReportFile));*/
    });

Task("Analyse")
    .IsDependentOn("Coverage")
    .Does(()=>
    {
        Information("Sonar running!...");
        /*var settings = new SonarBeginSettings()
        {
    			Url = sonarQubeServerUrl,
    			Key = sonarQubeKey
  		  };
  		Sonar(
  			ctx => {
  				ctx.MSBuild(solution);
  			}, settings
  		);*/
    });
    

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);