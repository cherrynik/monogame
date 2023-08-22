# Description

A MonoGame-based game.

- MonoGame repository - [Link](https://github.com/MonoGame/MonoGame).
- Project docs - [Link](https://monogame.youtrack.cloud/articles/MG)

# Getting Started

## Install Dependencies

1. Using Node.js v20.5.1:
    ```shell
    npm i
    ```

2. Install .NET 6 - [Link](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

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

[//]: # (TODO: Use make later)

You can also use `npm` to see the project tree.

```shell
npm run graph
```

# Project Workflow

## Importing sprites

1. Use Aseprite for exporting sprite sheets, export 'em with a JSON file.
    - **How?** - [Link](https://gamebanana.com/tuts/13811)
    - Buy Aseprite - [Link]()
2. Use QuickType (for generating types from your JSON) - [Link](https://quicktype.io/)
3. Add the new type in `Models`.
4. **Use it!**

## Importing text files: `JSON`, `txt`...

In the MonoGame Content Pipeline file `Content.mgcb`, just click on your file and choose `Copy` instead of `Build`.

Or write the command below in the file:

```
#begin <path-within-content-folder>
/copy:<path-within-content-folder>
```

Then, just read it in your code as in following:

```csharp
File.ReadAllText(@"Content\<path-to-your-file>.<extension>");
```

Perfectly, add a constant value in resources.

Here is how - [Link](#creating-global-const);

## Creating global `const`

In the `Resources/` folder in the app project, you can add const values.

> **Warning!** Don't store any sensitive data there.

Microsoft Docs - [Link](https://learn.microsoft.com/en-us/dotnet/core/extensions/resources)

**Case**: Some path values start with `'Content/...'`, some don't.

**Explanation**: It's made so as some of them are handled by `Content.Load` of MonoGame, and some
by `File`, `FileStream`, and the kind of tools, which don't read files starting from the `Content/` folder but from the
root of the project.

### TODO: Run tests on CI when did git push