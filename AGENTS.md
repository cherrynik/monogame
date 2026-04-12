# Guidance for AI agents

## Primary references

- **Claude Code:** [`CLAUDE.md`](CLAUDE.md) — stack, commands, architecture, code style, testing, all in one place
- **Cursor rules (always on):** [`.cursor/rules/monogame-project.mdc`](.cursor/rules/monogame-project.mdc) — same rules in Russian, detailed
- **Full write-up:** [`docs/development-architecture.md`](docs/development-architecture.md)

## Quick docs

- **Gameplay / workflow:** [`docs/Logic.md`](docs/Logic.md), [`docs/Requirements.md`](docs/Requirements.md), [`docs/Workflow.md`](docs/Workflow.md)
- `Workflow.md` still mentions Entitas/Jenny — the codebase uses **Scellecs.Morpeh** (see CLAUDE.md)

## Key rules (short)

1. Use **Morpeh**, not Entitas
2. Keep changes small and local
3. Tests always (TDD preferred)
4. `Game.cs` = orchestration only, no feature logic
5. UI dependency: `App → Pages → Widgets/Features → Entities → Shared`
6. Event-driven where it reduces coupling; frame-driven for rendering/physics
7. Validate: `dotnet build && dotnet test && dotnet run`

When changing Docker, .NET version, CI, or architecture conventions — update **both** `CLAUDE.md` and `.cursor/rules/monogame-project.mdc`.
