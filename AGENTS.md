# AGENTS.md

This file provides guidance to coding agents — Codex, Claude Code (claude.ai/code), and others — when working with code in this repository.

> **This file is the single source of project instructions for every agent.** Codex and other AGENTS.md-aware tools read it directly; Claude Code reads it through a one-line `@AGENTS.md` import in `CLAUDE.md`. Put project instructions here. Anything added to `CLAUDE.md` is visible to Claude Code only.

## Licensing: Clean-Room Reference Policy

Aquavit VR ships under MIT. The full policy, the allowlist, and the reasons live in `docs/ReferencePolicy.md`. Read it before you consult any implementation other than this repository. The rule is binary: a reference is either open or closed. There is no "read but do not port" tier, because nobody can verify that an agent did not reproduce what it read.

**Never open these** — source code, tests, issues, pull requests, in-repository docs, forks, package contents, local clones, or code snippets quoted in articles and chat:

- WebF (`openwebf/webf`) — GPL-3.0.
- Kraken (`openkraken/kraken`) — Apache-2.0, but it is WebF's predecessor and shares its lineage.
- Chromium's Blink (`third_party/blink`) — the repository says BSD-3-Clause, but DOM and Layout files carry LGPL headers.
- WebKit, Servo, Gecko — LGPL / MPL-2.0.

What you do instead:

- Decide behavior from web standards: WHATWG (DOM, HTML), W3C CSS specifications, and MDN.
- Take test cases from web-platform-tests (BSD-3-Clause).
- Read an implementation from the allowlist in `docs/ReferencePolicy.md` (Ladybird, litehtml, Yoga, Taffy, AngleSharp, FloatSoda, Flutter). Check the license header of each file you open, and state the project and its license in your report.
- For the closed engines, read design documents and official blogs only. For WebF, read product information on its official website only.
- **When a reference is on neither list, ask the owner before opening it.**

If you touch a closed reference by accident, report it to the owner right away: what you opened and which code you wrote afterwards. The contact log is in `docs/ReferencePolicy.md`.

## Output Language

Write output to the repository owner in Japanese: reports, review findings, commit messages, and PR titles/bodies. `docs/` is written in Japanese. Identifiers, type names, file paths, and code stay verbatim in English.

## Project Overview

Aquavit VR is a web UI framework for VR. The UI model is DOM / CSS / Layout / Paint, and the paint result is converted into the FloatSoda LayerTree. It is an independent framework, separate from FloatSoda's Widget / Element / RenderObject model.

The roadmap is tracked as GitHub milestones (Phase 0 – Phase 4) and issues. `docs/Home.md` maps each Phase to its epic issue. The current phase is **Phase 0 - Engine Boundary** (POC, issue #1).

## Engine Boundary Rules (see `docs/Architecture.md`)

- `src/Aquavit` references exactly one FloatSoda package: `FloatSoda.Rendering`, through a NuGet `PackageReference`.
- The boundary shared with FloatSoda is the LayerTree and below. Build UI with Aquavit's own DOM model; `Widget`, `Element`, `RenderObject`, `BuildContext`, and `Key` belong to FloatSoda's UI model and stay outside this repository.
- When FloatSoda itself needs a change, make it in the FloatSoda repository and reference the released version here.
- These rules are currently enforced only by `src/Aquavit/Aquavit.csproj`. Automated checking is tracked in issue #6.

## Repository Conventions

Each convention has one home. Read that document before acting on a convention, and keep the rule text there only; link to it from here.

| What you need | Where it lives |
|---|---|
| Issue labels, branch naming, PR scope, commit messages, test naming, namespace/directory layout, language of agent-facing files | `CONTRIBUTING.md` |
| Code review criteria and test perspectives: priorities, the bar for a finding, specification precedence, licensing and engine-boundary checks, what a test must cover | `REVIEW.md` |
| What may be referenced or ported (licensing) | `docs/ReferencePolicy.md` |
| API design: web-standard APIs vs Aquavit's own APIs, immutability, `double`, keeping Skia types out of the DOM / Style / Layout API | `docs/APIDesign.md` |
| XML documentation comments | `docs/DocumentationComments.md` |

Three that catch agents out most often:

- **Branch names carry the issue and the area, never the agent.** Use `<issue-number>-<primary-area>-<slug>`; no `codex/`, `claude/`, or `agent/` prefixes. Full rules in `CONTRIBUTING.md`.
- **Test method names are `Member_条件_期待結果`, with the condition and the expectation in Japanese.** Full rules in `CONTRIBUTING.md`.
- **Keep a PR to its issue.** Unrelated refactoring, renames, cleanup, and dependency changes go to a separate issue. Full rules in `CONTRIBUTING.md`.

## Build & Run Commands

```bash
# Build the whole solution
dotnet build Aquavit.slnx

# Run all tests
dotnet test Aquavit.slnx

# Run the LayerTree sample and write a PNG
dotnet run --project samples/Aquavit.Samples.LayerTree -- layer-tree.png
```

## Verification

Until Phase 0 ends, verify rendering by running a sample under `samples/` and inspecting the PNG it writes. When you report a rendering change, run the sample and look at the PNG yourself. PNG-based regression tests arrive in Phase 1 (issue #15).

To add a verification, add a console project `samples/Aquavit.Samples.<Name>/` that references `src/Aquavit/Aquavit.csproj`, and register it under `/samples/` in `Aquavit.slnx`.

## Project Structure

| Path | Role |
|---|---|
| `src/Aquavit` | Framework library (NuGet package ID `Aquavit`) |
| `samples/` | Verification samples; they run without SteamVR |
| `tests/Aquavit.Test` | xunit tests |
| `docs/` | Documentation, entry point `docs/Home.md` |

Build settings shared by all projects live in `Directory.Build.props`. `samples/` and `tests/` each have a `Directory.Build.props` that turns packing and XML documentation off.

## Conventions

This repository follows FloatSoda's conventions for solution layout (`.slnx`, `src/` / `samples/` / `tests/`), `.editorconfig`, and `docs/` page format (a `← [Home](Home.md)` back link, half-width parentheses, `> **実装状況** — ` notes). Match the surrounding code and documents when you add to them.

Code style is checked at build time by StyleCop.Analyzers, Roslynator, and the SDK code-style rules. The enabled rules and the reason for each exclusion are in `.editorconfig`; `stylecop.json` holds StyleCop settings. Analyzers are opt-in: enable a rule only with a stated purpose, and write that purpose as a comment next to it. Keep `dotnet build Aquavit.slnx` at zero warnings.
