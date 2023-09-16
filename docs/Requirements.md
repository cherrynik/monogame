# Requirements

## MonoGame

Resource - [Link](https://docs.monogame.net/articles/platforms.html#desktopgl)

| Supported Systems     | NuGet Package                | Template ID   |
|-----------------------|------------------------------|---------------|
| Windows, macOS, Linux | MonoGame.Framework.DesktopGL | `mgdesktopgl` |

DesktopGL uses SDL for windowing, OpenGL for graphics, and OpenAL-Soft for audio.

DesktopGL supports Windows (8.1 and up), macOS (Catalina 10.15 and up) and Linux (64bit-only).

DesktopGL requires at least OpenGL 2.0 with the ARB_framebuffer_object extension (or alternatively at least OpenGL 3.0).

DesktopGL is a convenient way to publish builds for Windows, macOS, and Linux from a single project and source code. It
also allows to cross-compile any build from any of these operating systems (e.g. you can build a Linux game from
Windows).

You can target Windows 8.1 (and up), macOS Catalina 10.15 (and up), and Linux with this platform.

DesktopGL currently does not have a `VideoPlayer` implementation.