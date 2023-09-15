GEN_ENTRY_POINT = ./src/Libs/Components
RUN_ENTRY_POINT = ./src/Apps/GameDesktop/GameDesktop.csproj

gen:
	+$(MAKE) -C $(GEN_ENTRY_POINT)
	
run: 
	dotnet run --project $(RUN_ENTRY_POINT)

graph:
	dotnet dependensee . -t html -o ./dependensee.html -p && open-cli ./dependensee.html
	
editor:
	dotnet mgcb-editor