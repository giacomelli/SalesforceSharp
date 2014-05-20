param([string]$task = "local")

$ApplicationName = "GG.SalesforceSharp"

$NugetPackages = @(
	"GG.SalesforceSharp"
)

$webClient = new-object net.webclient
$webClient.Headers.Add("Accept", "application/vnd.github.3.raw")
$webClient.Headers.Add("User-Agent", "JustGivingBuildScript")
$webClient.DownloadFile('https://api.github.com/repos/justgiving/GG.BuildScript/contents/bootstrap.ps1?access_token=0f3f0348eb63d8ef44d5ff3f5091a2525e9a00a6','bootstrap.ps1')

. .\bootstrap.ps1

Invoke-Build -Task $task -ApplicationName $ApplicationName -NugetPackages $NugetPackages