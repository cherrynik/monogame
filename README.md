## Getting Started

- [Wiki](./docs/TableOfContent.md)
- [MonoGame repository](https://github.com/MonoGame/MonoGame)
- [Project docs](https://monogame.youtrack.cloud/articles/MG)
- [Task-tracking board](https://monogame.youtrack.cloud/agiles/147-2)

## Install Dependencies

> **Warning!** It's only required if you're going to maintain the project.
> As npm packages installed, you gotta use conventional commits from now on whenever you're using `git`.

1. Using Node.js v20.5.1:

```shell
npm i
```

2. Install .NET 7 - [Link](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

3. Run

```shell
npm start
```

## Code Style

Microsoft Docs - [Link](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/code-style-rule-options)

Use CSharpier - [Link](https://csharpier.com/docs/Editors)

## Dependency Injection

LightInject Docs - [Link](https://github.com/seesharper/LightInject)

## Dependency Graph

Install `Dependensee` - [Link]()

Run the command:

```shell
dependensee . -t html -o ./dependensee.html
```

And open the output file in the root of the project.

Or check the docs - [Link](https://github.com/madushans/DependenSee)

[//]: # (TODO: Write about make)

[//]: # (TODO: Make graphs in GH pages for dependencies of the projects and maybe for IoC containers)

[//]: # (TODO: Generate ECS files when saved sorta cs file?)

You can also use `npm` to see the project tree.

```shell
npm run graph
```
