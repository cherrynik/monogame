# Project Workflow

## Table Of Content

- [Architecture](#architecture)
- [Importing sprites](#importing-sprites)
- [Importing special files](#importing-special-files)
- [Aseprite importing](#aseprite-importing)
- [Creating global `const`](#creating-global-const)
- [State Machine](#state-machine)

## Makefile

If some commands don't run, but the file is correct 99%,
ensure you didn't set BOM in you file encoding.

## Setup Jenny

The step is required for the first time.

> If you see a `Jenny.properties` file and running generations through `make` succeeds, you should negotiate your
> actions with your team.

### Run initializer

```shell
# from project root directory
cd ./src/<Libs|Apps>/<YourProjectName>
```

```shell
# from the last entered directory
dotnet ../../../external/Jenny/Jenny.Generator.Cli.dll
```

Then copy an already existing `Jenny.properties` file in the new existing project directory and change it:

```shell
Jenny.Plugins.ProjectPath = <YourProjectName>.csproj

# Where Health derives from IComponent, the next method will be generated:
# if false -> Add<YourProjectName>Health 
# if true -> AddHealth
Entitas.CodeGeneration.Plugins.IgnoreNamespaces = true
```

Don't forget to reference in your new project:

```html
...
<ItemGroup>
    <PackageReference Include="Entitas" Version="1.14.2"/>
    <PackageReference Include="Entitas.CodeGeneration.Attributes" Version="1.14.1"/>
    <PackageReference Include="Entitas.CodeGeneration.Plugins" Version="1.14.2"/>
    <PackageReference Include="Entitas.Roslyn.CodeGeneration.Plugins" Version="1.14.2"/>
</ItemGroup>
...
```

When you update your ECS files run the Jenny generate target for all the projects, using `make` from the root of the project.

For parallel execution, add `-j` flag:

```shell
make -j
```

> **Warning!**
> `Vector2` code-generation doesn't work when `using System.Numerics;`
>
> But works if you specify explicitly `System.Numerics.Vector2` or set `using Vector2 = System.Numerics.Vector2;`

## Architecture

The project is based on **ECS** (Entity-Component-System) architecture.
And for its easy code-management, the ECS-framework is used, Entitas.

Entitas - [Link](https://github.com/sschmid/Entitas)

### Installing Jenny & Code Generation

[TODO]

## Importing sprites

1. Use Aseprite for exporting sprite sheets, export 'em with a JSON file.
    - **How?** - [Link](https://gamebanana.com/tuts/13811)
    - Buy Aseprite - [Link]()
2. Use QuickType (for generating types from your JSON) - [Link](https://quicktype.io/)
3. Add the new type in `Models`.
4. **Use it!**

## Importing special files

In the MonoGame Content Pipeline file `Content.mgcb`, just click on your file and choose `Copy` instead of `Build`.

Or write the command below in the file:

```
#begin <path-within-content-folder>
/copy:<path-within-content-folder>
```

Then, use your own processor/reader:

```csharp
File.ReadAllText(@"Content\<path-to-your-file>.<extension>");
```

Perfectly, add a constant value in `*.resx` of the path to your file.

And use it like that:

```csharp
File.ReadAllText(Some.Path);
```

Here is how - [Link](#creating-global-const);

### Currently supported (and used) formats

- Aseprite: `ase`, `aseprite`
- Text: `json`, `txt`

## Aseprite importing

Here is how - [Link](https://monogameaseprite.net/)

## Creating global `const`

In the `Resources/` folder in the app project, you can add const values.

> **Warning!** Don't store any sensitive data there.

Microsoft Docs - [Link](https://learn.microsoft.com/en-us/dotnet/core/extensions/resources)

**Case**: Some path values start with `'Content/...'`, some don't.

**Explanation**: It's made so as some of them are handled by `Content.Load` of MonoGame, and some
by `File`, `FileStream`, and the kind of tools, which don't read files starting from the `Content/` folder but from the
root of the project.

## State Machine

Stateless Docs - [Link](https://github.com/dotnet-state-machine/stateless)

For now, it's used in the `Player` entity.