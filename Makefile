GEN_ENTRY_POINT = ./src/Libs/Components
RUN_ENTRY_POINT = ./src/Apps/GameDesktop/GameDesktop.csproj

gen:
	+$(MAKE) -C $(GEN_ENTRY_POINT)
	
dev: 
	dotnet run --project $(RUN_ENTRY_POINT) --configuration Debug --launch-profile GameDesktop.Development

dev-watch:
	dotnet watch --project $(RUN_ENTRY_POINT) run --configuration Debug --launch-profile GameDesktop.Development
	
prod:
	dotnet run --project $(RUN_ENTRY_POINT) --configuration Release --launch-profile GameDesktop.Production

graph:
	dotnet dependensee . -t html -o ./dependensee.html -p && open-cli ./dependensee.html
	
editor:
	dotnet mgcb-editor