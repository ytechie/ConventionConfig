cd ConventionConfiguration
nuget pack ConventionConfiguration.csproj -Symbols
move *.ConventionConfiguration.*.nupkg ..
pause