# Personal Roadmap

This document is a living plan. Add new wishes to the **Wishlist Inbox** first, then move them into a category.

## Wishlist Inbox

- Main menu screen: beautiful background, "New Game", "Continue" buttons, same pixel-art stone style.
- Loading screen: transition between main menu and gameplay with visual feedback.
- Game settings menu: keybindings (rebind + persist), language, persisted options (load user config at startup, save on apply/exit; migrate defaults when options evolve), later graphics settings.
- Save game / load game: save slots (or checkpoints), main-menu Continue from last save, optional auto-save.
- Mod support (content/gameplay extension points).
- Online/multiplayer support.
- Texture packs support.

---

## 1) UI Platform (Production-grade, customizable)

Goal: make UI architecture maintainable, themeable, and safe for long-term production use.

### Backlog

- Define UI architecture split: player-facing UI (Myra) vs debug/editor UI (ImGui).
- Introduce a shared UI style system (colors, spacing, typography tokens).
- Add reusable widget library for common controls (panel, button variants, list items, form rows).
- Define UI state boundaries (game state vs UI-only state) to avoid hidden coupling.
- Add screenshot-based visual smoke checks for critical screens.

### Success Criteria

- New screens are assembled from reusable blocks, not one-off layouts.
- Visual changes are centralized in style/theme configuration.
- UI regressions are detectable with fast smoke checks.

### React-like UI Plan (saved)

- Use a unidirectional flow: `Input -> UiAction -> UiReducer -> UiState -> View`.
- Keep reducer pure and side effects outside of reducer.
- Put UI state and reducer in shared services layer for fast unit testing.
- Keep Myra-specific code in view layer only (`GameDesktop/UI`).
- Centralize style and texts in `UiTheme`.
- Map gameplay effects (pause, exit, dialogs) in one adapter point inside `Game`.

---

## 2) Component-driven ImGui Tools

Goal: attach configurable ImGui panels to ECS components for debugging and fast iteration.

### Backlog

- Define a component-inspector registry (component type -> renderer/editor panel).
- Add opt-in component metadata for display/edit intent.
- Support runtime registration so new component panels can be added without touching core inspector logic.
- Add safe editing rules (clamps, validation, rollback on invalid input).
- Group component panels by gameplay domain (movement, rendering, inventory, quests).

### Success Criteria

- Adding an inspector for a new component requires only registration + panel implementation.
- Runtime editing is stable and does not corrupt ECS state.

---

## 3) Quest System (Convenient and extensible)

Goal: build a vertical slice that scales from simple quests to chained objective graphs.

### Backlog

- Create minimal quest ECS slice: `QuestComponent`, progress tracking, completion condition.
- Add quest lifecycle systems (activate, update progress, complete, reward/trigger follow-up).
- Define quest data format (JSON/assets) and loading pipeline.
- Add debug quest inspector panel (active/completed objectives).
- Prepare extension points for branching, dependencies, and scripted conditions.

### Success Criteria

- One end-to-end quest works from data load to completion.
- New quest types are added via extension points, not core rewrites.

---

## 4) Inventory UI

Goal: deliver a clear, responsive inventory flow integrated with ECS data.

### Backlog

- Define inventory UX flows (open/close, select, inspect, move/use/drop).
- Add item-slot view model mapping from ECS inventory data.
- Implement drag/drop or quick actions with input parity (mouse + keyboard).
- Add item tooltip/details panel and empty/error states.
- Ensure inventory interactions emit explicit gameplay events.

### Success Criteria

- Inventory operations are deterministic and easy to test.
- UI and gameplay stay synchronized under rapid actions.

---

## 5) Modular DI

Goal: split composition roots into explicit modules with clear ownership and testability.

### Backlog

- Break current registration hotspot into modules: Core, Rendering, UI, Debug, Gameplay.
- Enforce registration boundaries (module cannot silently depend on sibling internals).
- Add container validation/smoke resolution test for all modules.
- Define conventions for lifetime and factory usage.
- Document onboarding flow: where to register new systems/components/services.

### Success Criteria

- New features are wired by adding/modifying one module, not a monolithic root.
- DI graph is observable and fails fast on broken registrations.

---

## 6) Performance, Load, Optimization

Goal: establish measurable performance budgets and continuous optimization loop.

### Backlog

- Add benchmark project for hot paths (movement, filtering, sorting, render preparation).
- Add load/stress scenarios (entity count scaling, inventory stress, quest update spikes).
- Define baseline metrics (frame time, update time, allocations, GC pressure).
- Set regression thresholds and reporting in CI/local scripts.
- Prioritize optimization by measured impact only.

### Success Criteria

- Performance changes are accepted/rejected by measurable thresholds.
- Optimization work tracks user-visible gains and not micro-optimizations without value.

---

## 7) Engineering Quality Rules for Agents

Goal: enforce consistent clean-code discipline after every implementation iteration.

### Mandatory Policy

- After each code iteration, perform a cleanup pass before finishing the task.
- Apply clean code principles focused on readability, naming, and small responsibilities.
- Prefer simple, explicit flow over clever abstractions.
- Keep modules cohesive and reduce accidental coupling.
- If a feature introduces debt, capture it immediately in this roadmap.

### Definition of Done Extension

- Feature works.
- Build/tests pass for touched scope.
- Readability and structure are improved or preserved.
- No avoidable complexity is left behind.

---

## 8) Game Settings

Goal: provide a player-facing settings experience with progressive depth.

### Backlog

- Add settings screen in pause/main menu.
- Implement settings persistence: load user config at game start, save when the player applies or exits; merge defaults when new options appear.
- Add keybindings rebinding UI and persistence (same user config or dedicated bindings file).
- Add language switch (localization selection + persistence).
- Add graphics settings later (quality, fullscreen/windowed, vsync, resolution where applicable).
- Add save/load game flow: slots or checkpoints, Continue entry point, optional auto-save (separate from settings persistence).

### Success Criteria

- Settings are discoverable, editable, and persist between runs.
- Keybindings and language updates are reflected in runtime behavior.

---

## 9) Extensibility & Live Content

Goal: make the game open for community and future expansion.

### Backlog

- Define mod API boundaries (safe extension points, content contracts).
- Add texture pack loading/override pipeline.
- Add online foundation plan (network architecture, session model, authority model).
- Separate offline core logic from online adapters to keep maintainability.

### Success Criteria

- Mods and texture packs can be added without code changes in core gameplay.
- Online support has a clear phased roadmap and does not block single-player progress.
