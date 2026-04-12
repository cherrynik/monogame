# Architecture, tooling, and roadmap

**AI agents:** a condensed, always-on summary of this file plus `docs/Logic.md`, `docs/Requirements.md`, and curated notes from `docs/Workflow.md` lives in **`.cursor/rules/monogame-project.mdc`**. When you change Docker, SDK, CI, or high-level architecture, update that rule alongside this document.

This document captures the **current state** of the repository: stack, solution layout, Docker/.NET decisions, CI, and limitations. The roadmap at the end is a guide, not a fixed contract.

---

## 1. Project goal

A desktop game on **MonoGame (DesktopGL)** with ECS (**Scellecs.Morpeh**), DI (**LightInject**), UI (**Myra**), a debug layer (**ImGui** via **MonoGame.ImGuiNet**), logging (**Serilog**), and monitoring (**Sentry**). Configuration uses **Microsoft.Extensions.Configuration** (JSON + environment variables).

---

## 2. Technology stack

| Area | Choice | Notes |
|------|--------|--------|
| Platform | **.NET 10** (`net10.0`) | SDK version pinned in `global.json` |
| Game / rendering | **MonoGame.Framework.DesktopGL** 3.8.1.x | One project targets Windows / macOS / Linux |
| Content | **MonoGame.Content.Builder.Task**, **MGCB** (dotnet tool) | `.mgcb`; tools in `.config/dotnet-tools.json` |
| ECS | **Scellecs.Morpeh** | Components, systems, features |
| DI | **LightInject** | Composition roots under `GameDesktop/CompositionRoots` |
| UI | **Myra** | Wired to `GraphicsDevice` in `Game` |
| Debug UI | **ImGui.NET** + **MonoGame.ImGuiNet** (submodule) | **DEBUG**: `ImGuiRenderer` is registered |
| Logging | **Serilog** (+ sinks, **Serilog.Settings.Configuration**) | Levels from config files |
| Errors | **Sentry** | Token in CI via secrets |

---

## 3. Solution layout (`MonoGame.sln`)

```
src/Apps/GameDesktop/     — entry point, Game, composition, content, resources
src/Libs/
  Components/             — ECS components
  Entities/               — entity factories
  Systems/                — game systems
  Systems.Debugging/      — debug systems (ImGui, etc.)
  Features/               — Morpeh features (game logic in chunks)
  Features.Debugging/
  Services/               — services (movement, input, math, factories)
  External/
    MonoGame.ImGuiNet/    — git submodule
    Scellecs.Morpeh.Extended/
src/UnitTests/            — NUnit tests
```

**Dependencies in practice:** `GameDesktop` references Features, Services, Components, ImGuiNet; libraries avoid tight cycles upward without explicit comments in code (`GameCompositionRoot` documents a deliberate hack with `GraphicsDeviceManager` and the Game ↔ container cycle).

---

## 4. Application architecture

### 4.1 Entry

`Program.cs` (top-level statements):

1. Load configuration (`ConfigurationFactory`).
2. Set `AppBaseDirectory` for resources/paths.
3. Create **Serilog** logger (`LogFactory`, optional Sentry).
4. **LightInject** `ServiceContainer`; register `IConfiguration`, `ILogger`, `IServiceContainer`.
5. `RegisterFrom<GameCompositionRoot>()` → resolve `Game` → `game.Run()`.

### 4.2 Game loop

`GameDesktop.Game` (MonoGame `Game`):

- **Initialize:** register `SpriteBatch`, Myra; in **DEBUG** — ImGui renderer; then root feature and UI.
- **LoadContent:** `RootFeature.OnAwake()`.
- **Update / Draw:** fixed/variable feature updates; render via `SpriteBatch`, then Myra, ImGui frame when present.

### 4.3 Environment configuration

- `Properties/launchSettings.json` — profile with `DOTNET_ENVIRONMENT=Development`.
- `appsettings.json`, `appsettings.Development.json`, `appsettings.Production.json` are copied to the output.

### 4.4 Graphics mode for CI

A **Release** build with **`IS_CI`** (see GitHub Actions) may exit with code 0 when graphics device creation fails, so headless agents do not fail without a display. This is not used for real debugging on a developer machine.

---

## 5. External code: **MonoGame.ImGuiNet** submodule

- Path: `src/Libs/External/MonoGame.ImGuiNet`
- Declared in `.gitmodules` (GitHub URL; CI often needs access via **PAT** and `submodules: recursive`).

**Decision:** the MSBuild target **`RestoreDotnetTools` with `BeforeTargets="Restore"`** was **removed** from this project and from `GameDesktop`. Otherwise every `dotnet restore` repeatedly invoked `dotnet tool restore` (noise and time). Tools are restored **explicitly**: once after clone locally, in Docker commands, and in CI.

---

## 6. Docker

### 6.1 Why

- Build with **.NET 10 SDK** without installing the SDK on the host.
- Reproducible Linux environment with native dependencies for MGCB/SDL.

### 6.2 Image (`Dockerfile`)

- Base: **`mcr.microsoft.com/dotnet/sdk:10.0`** (Ubuntu 24.04 “noble” in Microsoft’s image chain).
- Extra packages via `apt`: `git`, font/Freetype, ICU dev, **OpenAL**, **SDL2**, **xvfb** (virtual display for the `run` service).

After changing the base image or `global.json`, rebuild the image:

```bash
docker compose build --pull build
```

### 6.3 Docker Compose

Two services, **one Dockerfile**, shared named volumes:

| Volume | Purpose |
|--------|---------|
| `nuget-packages` → `/root/.nuget/packages` | NuGet package cache across runs |
| `nuget-work` → `/root/.local/share/NuGet` | NuGet HTTP/metadata cache; fewer network round-trips |

Environment: `DOTNET_CLI_TELEMETRY_OPTOUT`, `DOTNET_NOLOGO`, `DOTNET_SKIP_FIRST_TIME_EXPERIENCE`, optional **`BUILD_CONFIGURATION`** (default `Debug`).

**`build` service**

- `git config safe.directory` for the mounted repo.
- `dotnet tool restore` → `dotnet restore MonoGame.sln` → `dotnet build` with **`-m:1`** and **`BuildInParallel=false`** (lower memory spikes in the container).

**`run` service**

- Same restore/build, then **`xvfb-run -a dotnet run`** for `GameDesktop`.
- **No window on the host** (headless inside the container); on macOS use local `dotnet run` for a real window.

**ImGui / native `cimgui`**

- **ImGui.NET** on NuGet targets **`linux-x64`** for Linux, not **`linux-arm64`**.
- Docker on **Apple Silicon** defaults to **linux-arm64** → `DllNotFoundException` without mitigation.

**Decision for `run`:**

- **`platform: linux/amd64`** — x86_64 container (on M‑series Macs, emulation; slower).
- Build and run with **`-r linux-x64`** so the correct native assets are used.

The **`build`** service keeps **no** fixed RID: portable build for compile checks on the container’s native architecture.

### 6.4 `.dockerignore`

Excludes `bin/`, `obj/`, `artifacts/`, `.git` from the **build context** (faster context upload; with `.:/src` mount the full repo is still visible on the host).

---

## 7. Continuous integration (GitHub Actions)

`.github/workflows/dotnet.yml`:

- Matrix: **ubuntu-latest**, **windows-latest**.
- Checkout with **`submodules: recursive`** and **`PAT_TOKEN`** (for a private submodule if needed).
- **.NET SDK 10.0.x** (`actions/setup-dotnet`).
- Explicit **`dotnet tool restore`**, then restore / build / test.
- **Run** step: `dotnet run` with **`net10.0`**, **Release**, **`DefineConstants=IS_CI`**.

---

## 8. Local development (recommended)

| Task | Best option |
|------|-------------|
| Game window, debugging | Host: **.NET 10 SDK**, `dotnet run` / IDE |
| Quick “does it compile?” | `docker compose run --rm build` |
| Headless smoke run | `docker compose up run` or `docker compose run --rm run` |

After clone: `git submodule update --init --recursive`, then **`dotnet tool restore`**.

---

## 9. Known limitations and caveats

1. **MGCB warning** about missing `MonoGameContentReference` on **Monogame.ImGuiNet** is expected if the submodule has no `.mgcb` with the right build action; it does not affect **GameDesktop** content if wired there.
2. A **submodule with a changed `TargetFramework`** (e.g. aligned to `net10.0`) stays **dirty** until you commit or fork — plan for that in your git workflow.
3. **Docker `run` on ARM Mac** uses amd64 emulation; for day-to-day speed, prefer a local run.
4. **`sdk:10.0-alpine`** is smaller, but moving MonoGame/MGCB and **ImGui** glibc compatibility to Alpine is non-trivial; the repo intentionally uses the **full Ubuntu-based SDK** image.

---

## 10. Roadmap

Below mixes explicit **TODOs from code** with sensible next steps; priorities are flexible.

### 10.1 Gameplay and engineering (from `Game.cs` and context)

- Refine **timestep** (references in code to fixed timestep / Gaffer on Games).
- Debug logging flags (**LOG_MOVEMENT**, etc.).
- Expand debug UI (player position, input, other entities).
- Evaluate third-party libs (e.g. Nez) only when there is a clear need.

### 10.2 Tooling and repository

- Reintroduce a **`publish` compose service** (self-contained macOS / other RIDs) if needed — it was dropped in favor of `build` only; recover from git history if required.
- Consider **bumping ImGui.NET / MonoGame** if official **`linux-arm64`** natives appear — then relax `platform: linux/amd64` for `run` on Apple Silicon.
- Optionally: a single **`image:`** in Compose for `build` and `run` to avoid two image tags for the same Dockerfile.

### 10.3 Documentation

- Keep in sync with `docs/Logic.md`, `docs/Workflow.md`, `docs/Requirements.md` (this file is **infrastructure and decisions**; game logic lives there).

### 10.4 CI

- Periodically bump **`actions/checkout`**, **`setup-dotnet`**.
- If the submodule is public, simplify secrets; if private, document **PAT** in the team README.

---

## 11. Command cheat sheet

```bash
# Clone + submodule
git submodule update --init --recursive

# Local
dotnet tool restore
dotnet restore MonoGame.sln
dotnet build MonoGame.sln
dotnet run --project src/Apps/GameDesktop/GameDesktop.csproj

# Docker: build only
docker compose run --rm build

# Docker: headless run (see amd64 / linux-x64 section)
docker compose run --rm run

# Release build in Docker
BUILD_CONFIGURATION=Release docker compose run --rm build
```

---

*Last updated for the repository state with .NET 10, Docker Compose (`build` + `run`), and the decisions described above.*
