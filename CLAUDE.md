# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Stack

- **.NET 10** (`net10.0`), pinned in `global.json`
- **MonoGame.Framework.DesktopGL** — rendering, game loop
- **Scellecs.Morpeh** — ECS (not Entitas/Jenny; those are obsolete here)
- **LightInject** — dependency injection
- **Myra** — main UI
- **ImGui.NET + MonoGame.ImGuiNet** — debug UI (DEBUG builds only; submodule at `src/Libs/External/MonoGame.ImGuiNet/`)
- **Serilog** + **Sentry** — logging and error monitoring

## Commands

After cloning:
```bash
git submodule update --init --recursive
dotnet tool restore
dotnet restore MonoGame.sln
```

Run / build:
```bash
make dev           # dotnet run Debug + Development profile
make dev-watch     # dotnet watch run
make prod          # dotnet run Release + Production profile

dotnet build MonoGame.sln
dotnet test
```

Docker (headless, no window on host):
```bash
docker compose run --rm build
docker compose run --rm run
BUILD_CONFIGURATION=Release docker compose run --rm build
```

MGCB editor:
```bash
make editor        # dotnet mgcb-editor
```

Before finishing any change: `dotnet build && dotnet test && dotnet run` (at minimum a smoke check).

## Solution layout

```
src/Apps/GameDesktop/          — entry point: Program.cs, Game.cs, composition roots, content
src/Libs/
  Components/                  — ECS components
  Entities/                    — entity factories
  Systems/                     — game systems
  Systems.Debugging/           — debug/ImGui systems
  Features/                    — Morpeh features (game logic chunks)
  Features.Debugging/
  Services/                    — movement, input, math, factories
  UI/                          — UI library (Myra-based)
  External/
    MonoGame.ImGuiNet/         — git submodule
    Scellecs.Morpeh.Extended/
src/UnitTests/
  UnitTests.Entities/
  UnitTests.Services/
```

Tests go in `src/UnitTests/<Project>/Unit/`. Integration tests: `src/IntegrationTests/`. Do not mix test types.

## Application flow

1. `Program.cs` — loads config, creates Serilog logger, creates LightInject container.
2. `RegisterFrom<GameCompositionRoot>()` → resolves `Game` → `game.Run()`.
3. `Game.Initialize()` — registers SpriteBatch, Myra, ImGui (DEBUG only), then root feature and UI.
4. `Game.LoadContent()` — calls `RootFeature.OnAwake()`.
5. `Game.Update()` / `Game.Draw()` — fixed/variable feature updates; render via SpriteBatch → Myra → ImGui.

`GameCompositionRoot` has a deliberate `GraphicsDeviceManager` ↔ `Game` ↔ container cycle — do not "clean" it without understanding why.

## Architecture rules

**ECS:** Use Morpeh only. Ignore any Entitas/Jenny code or docs.

**`Game.cs`** is orchestration only — lifecycle coordination and high-level wiring. Input mapping, UI effects, and feature-specific logic belong in separate modules.

**DI:** Avoid `game.Services.GetService(...)` service locator; wire through LightInject instead.

**UI layers (FSD-like), dependency direction must flow top → bottom:**
`App → Pages → Widgets/Features → Entities → Shared`

**Event-driven** where it reduces coupling (movement events, UI intents, domain signals). Keep frame-driven loop for rendering, animation, physics-like updates.

**`RestoreDotnetTools` via MSBuild `BeforeTargets="Restore"` is intentionally removed.** Run `dotnet tool restore` explicitly after clone, in Docker, and in CI.

## Code style

- Guard clauses and early returns over nested `if`
- One top-level type per file; folder/namespace alignment
- No service locator, no god files, no flat file dumps
- Explicit naming; intermediate booleans where they improve readability
- After changing code: improve naming, reduce coupling, simplify flow in touched areas

## Testing

- TDD by default: tests first → implement → refactor
- Every feature must include tests (unit at minimum)
- Unit tests: `src/UnitTests/<Project>/Unit/`; integration: `src/IntegrationTests/`; e2e: `src/E2ETests/`
- If touching a legacy area without coverage, add tests for it first
- Before finishing: `dotnet build && dotnet test && dotnet run` (smoke check)

## Dependency and reuse

- Before writing new code, check for existing solutions in the repo
- Understand the full call chain end-to-end before changing it
- Make changes at the lowest appropriate architectural level
- Use existing libraries (`UI`, `Services`, `Systems`, `Features`) — do not create new top-level `src/Libs/*` for simple refactoring

## Gameplay notes

- Normalize movement vector for diagonals; MonoGame Y-axis is inverted
- Direction pipeline: input axes → vector → radians → 8-direction enum

## Content pipeline

After changing `.mgcb`, rebuild content. For files read directly (not via `Content.Load`), use `Copy` / `/copy:` in `Content.mgcb`.

## Docker / CI notes

- Docker `run` service uses `platform: linux/amd64` and `-r linux-x64` because `cimgui` native ships for `linux-x64` only (no `linux-arm64`). On Apple Silicon this means emulation — prefer local `dotnet run` for daily dev.
- CI (GitHub Actions) uses `IS_CI` define; Release + `IS_CI` allows a controlled exit (code 0) on graphics device failure in headless runners.
- Docker base image is full Ubuntu-based SDK (`mcr.microsoft.com/dotnet/sdk:10.0`), not Alpine, due to glibc requirements for MonoGame/ImGui natives.
