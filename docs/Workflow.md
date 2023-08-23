# Project Workflow

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