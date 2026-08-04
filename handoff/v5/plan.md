# DEPLOY-C6-StateManagement-v5_0 — Execution Plan

Approved 2026-08-03, executed the same session. Activity 5 of five: browser-persisted
cart state plus a final cross-app testing pass. No prompt patches this run — unlike
v4's core+patch checkout addition, Activity 5's full scope (cart persistence + final
testing pass) was delivered as a single self-contained prompt.

## Context

Activities 1-4 were done and merged into `main` (HEAD `2c72c22`) before this run
started. The governing constraint, carried since Activity 1, is the freeze:
`Services/Cart.cs`'s four original methods, `Models/Product.cs`'s four original
properties, and `ProductCard.razor`'s two original parameters had to end this run
byte-identical to `main`. Persistence was implemented as a new, separate service wired
from the calling layer (`MainLayout.razor`), the same additive pattern this repo used
for every prior extension, so `Cart.cs` needed zero changes.

Deploy timing: run start recorded at **2026-08-03 22:57:11 -05:00**.

## Preflight — confirmed

- GitHub access: `gh auth status` confirmed logged in as `jdsaire`, `repo` scope
  present.
- Both required attachments (the capstone brief and the master transcript) were read
  successfully.
- HEAD confirmed at `2c72c22` on `main`, matching the prompt's `verified_state`
  exactly. Local `main` == `origin/main`, no drift.
- Read directly from the live repo and confirmed every fact in `verified_state` and
  `resolved_decisions` held against actual file contents: `Services/Cart.cs`,
  `Models/Product.cs`, `Models/MockProductData.cs`, `Program.cs`,
  `Layout/MainLayout.razor`, `Layout/CartSummary.razor`, `wwwroot/index.html`. No JS
  interop existed anywhere in the app yet. No `deploy/v5-state-management` branch
  existed locally or on `origin`.

**Two drifts found beyond what the prompt's `verified_state` flagged:**

1. `wwwroot/js/` was a genuinely new folder, not an addition to an existing one --
   `wwwroot/` had only `css/`, `images/`, `lib/`, and `index.html`. Resolved by giving
   it a README, per the README-everywhere rule's own new-folder fallback clause.
2. Root `README.md`'s "Out of Scope (So Far)" section was stale in the same way
   `docs/grading-criteria.md` was -- it still listed Activity 4 as a not-yet-built
   later delivery, contradicting the file's own "What's Built So Far" section above
   it. Fixed alongside the already-planned intro-paragraph update.

## Target design

- `src/ShopEase/wwwroot/js/cartStorage.js` -- a thin, key-agnostic wrapper
  (`getItem`/`setItem`/`removeItem`) over `window.localStorage`, exposed as
  `window.cartStorage`. The namespaced key itself lives in C#, not JS.
- `src/ShopEase/Services/CartStorageService.cs` -- constructor-injected `IJSRuntime`,
  owns the storage key `shopease.cart.v1`, `SaveAsync`/`LoadAsync` using
  `System.Text.Json`, never throwing (missing/empty/corrupt value → empty list).
  Registered as a DI singleton in `Program.cs`, matching `Cart`'s own lifetime.
- `src/ShopEase/Layout/MainLayout.razor` -- new `@code` block: injects `Cart` and
  `CartStorageService`; `OnInitialized` subscribes a save handler to `Cart.OnChange`
  (`Dispose` unsubscribes); `OnAfterRenderAsync(firstRender)` loads persisted
  `ProductID`s, resolves each against `MockProductData.GetSeedProducts()` (skipping
  and console-logging any ID no longer in the catalog), replays each through the
  existing, unmodified `Cart.AddProduct`, then calls `Cart.NotifyChange()` and
  `StateHasChanged()` once for the whole batch.

## Ordered commit sequence (as planned)

1. `feat(state): add localStorage JS interop for cart persistence`
2. `feat(state): add CartStorageService for cart load/save`
3. `feat(state): persist and restore cart via localStorage on every page`
   -- GATE 1: stop for approval before the final testing pass.
4. Task 6 (final testing/regression pass) -- `perf:`/`fix:` commit only if a genuine
   issue is found.
5. `docs: add state-management decisions, learning-mode walkthrough, and sync grading
   criteria and READMEs`
6. `docs: archive v5 plan and completion report`
7. Open PR against `main`, unmerged.

## Verification plan (task 8)

`dotnet build` clean after every commit; `ShopEase.csproj` `PackageReference` count
unchanged (3); `git diff main -- Services/Cart.cs Models/Product.cs
Pages/ProductCard.razor` empty; every internal markdown link counted and confirmed
resolving, reported N/N; `git log` author/committer check for zero AI attribution; no
browser-automation tool available this session, so persistence verified by code-level
trace rather than a live click-through -- flagged explicitly at gate 1 and again in the
completion report's open items.

Note: no file named `completion-report-shape.md` exists in the repo -- the completion
report instead follows the shape `handoff/v4.1/completion-report.md` actually
established.
