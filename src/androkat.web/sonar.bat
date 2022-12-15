dotnet clean androkat.core.sln
dotnet restore androkat.core.sln
dotnet sonarscanner begin /k:"androkat.core" /d:sonar.login="admin" /d:sonar.password="A-T4An$v-KPCxvQ" /d:sonar.host.url="http://localhost:9000" /d:sonar.coverage.exclusions="**/Program.cs, **/*.js" /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml" -d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx"
dotnet build --no-restore --configuration Release androkat.core.sln
dotnet test androkat.core.sln --no-build --no-restore --configuration Release --verbosity normal --logger trx --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
dotnet sonarscanner end /d:sonar.login="admin" /d:sonar.password="A-T4An$v-KPCxvQ"