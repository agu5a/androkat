rmdir .\Tests\androkat.application.Tests\TestResults /s /q
rmdir .\Tests\androkat.web.Tests\TestResults /s /q
rmdir .\Tests\androkat.infrastructure.Tests\TestResults /s /q

dotnet clean androkat.web.sln
dotnet restore androkat.web.sln
dotnet sonarscanner begin /k:"androkat.web" /d:sonar.login="admin" /d:sonar.password="A-T4An$v-KPCxvQ" /d:sonar.host.url="http://localhost:9000" /d:sonar.coverage.exclusions="**/Program.cs, **/*.cshtml, **/*.js" /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml" -d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx"
dotnet build --no-restore --configuration Release androkat.web.sln
dotnet test androkat.web.sln --no-build --no-restore --configuration Release --verbosity normal --logger trx --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
dotnet sonarscanner end /d:sonar.login="admin" /d:sonar.password="A-T4An$v-KPCxvQ"