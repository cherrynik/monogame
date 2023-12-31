﻿# Workflow

## Table Of Content

- [GitHub Actions](#github-actions)
- [MonoGame Pipeline](#monogame-pipeline)
- [Makefile](#makefile)
- [Architecture](#architecture)
- [Setup Jenny](#setup-jenny)
- [Importing sprites](#importing-sprites)
- [Importing special files](#importing-special-files)
- [Aseprite importing](#aseprite-importing)
- [Creating global `const`](#creating-global-const)
- [State Machine](#state-machine)
- [Logging](#logging)

## GitHub Actions

As GitHub runners don't have graphics devices, it's impossible to create the `Game` instance, as it'll throw
the `Failed to create graphics device!` error on the runners, what is not but successful way to exit the program **on the CI
machine**.
As all the dependencies are resolved before the game is ran, in the pipeline, we test DI Container to
resolve all the dependencies correctly, and it has to exit only with the error, which on the CI machine will be interpreted as a successful exit code.

We have windows and linux runners 'cuz of OS-related issues, like different paths, etc. (means I don't know others yet but it's a best practice), and need to ensure that (almost) everything is handled correctly.

## MonoGame Pipeline

Don't forget to rebuild `.mgcb` file after changing files in it.

## Makefile

If something happens like commands don't run, variables not evaluated, but the file is correct 99%,
ensure you didn't set BOM in you file encoding.

## Architecture

The project is based on **ECS** (Entity-Component-System) architecture.
And for its easy code-management, the ECS-framework is used, Entitas.

Entitas - [Link](https://github.com/sschmid/Entitas)

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

### Generate ECS Files

When you update your ECS files run the Jenny's generate target for all the projects, using `make` from the project root.

For parallel execution, add `-j` flag:

```shell
make -j
```

> **Warning!**
> `Vector2` code-generation doesn't work when `using System.Numerics;`
>
> But works if you specify explicitly `System.Numerics.Vector2` or set `using Vector2 = System.Numerics.Vector2;`

## Importing sprites

Buy Aseprite - [Link](https://www.aseprite.org/)

Manual (MonoGame Aseprite) - [Link](https://monogameaseprite.net/)

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

## Logging

### General

Serilog - [Link](https://serilog.net/)

### Errors Reporting

Sentry - [Link](https://docs.sentry.io/platforms/dotnet)

_It doesn't work with banned IP addresses, so use VPN if you're located in a banned area._
