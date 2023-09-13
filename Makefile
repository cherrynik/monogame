ENTITIES_DIR = ./src/Libs/Entities
COMPONENTS_DIR = ./src/Libs/Components
SYSTEMS_DIR = ./src/Libs/Systems

JENNY = $(realpath ./external/Jenny/Jenny.Generator.Cli.dll)

all: c-gen

e-gen:
	cd $(ENTITIES_DIR) && dotnet $(JENNY) gen

c-gen:
	cd $(COMPONENTS_DIR) && dotnet $(JENNY) gen

s-gen:
	cd $(SYSTEMS_DIR) && dotnet $(JENNY) gen