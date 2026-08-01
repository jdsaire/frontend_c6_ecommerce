# DEPLOY-C6-ShopEaseBuild-v1_0 — Execution Plan

## Context

This is the first build of `jdsaire/frontend_c6_ecommerce`, executed from a deploy prompt (`DEPLOY-C6-ShopEaseBuild-v1_0.xml`) that itself mandates Plan Mode: present the full plan and stop before writing any code. The run covers only Activity 1 (Product/Cart business logic) and Activity 2 (ProductCard component + Add-to-Cart events) of the five-activity Microsoft Frontend Developer Specialization capstone, as two gated phases with one commit per assignment step. Activities 3–5 are out of scope and ship in later runs against this same repo.

Three live instructions from the user layer on top of the frozen XML prompt and take precedence where they add detail or explicitly override a doctrine line:

1. **Illustrative comments in `.cs` files** — Product.cs, ShopDatabase.cs, Cart.cs, MockProductData.cs get comments explaining what each piece does, so a grader unfamiliar with the code can follow it. This adds detail to tasks that already exist; it doesn't conflict with anything in the XML.
2. **Codespaces / VS Code run instructions for Part 1 (Activity 1)** — the relevant READMEs and `docs/how-to-run.md` must explain how to run the project and see the CartTest output in GitHub Codespaces or VS Code.
3. **Per-gate learning-mode files, including a Glossary** — this is an explicit, live override of the XML's own guardrail ("Doctrine item 10 ... is deliberately omitted from this run ... They ship in the final run"). The user's current message supersedes that line for this run. I'm treating it as an authorized deviation, recorded here and later in the Completion Report, not a silent scope change.

## Preflight — confirmed

- **GitHub access**: `gh auth status` → logged in as `jdsaire`, scopes include `repo` and `workflow`. ✅
- **Target repo**: `gh repo view jdsaire/frontend_c6_ecommerce` → `isEmpty: true`, no default branch yet. Confirmed empty; safe to create `main` with the first push. ✅
- **Attachments**: all three required attachments (KEY capstone doc, MasterTranscript JSON, and the non-blocking syllabus) were read in full this session. ✅
- **.NET SDK**: `10.0.201` installed, targets `net10.0`. `dotnet new list` shows the `blazorwasm` template available. ✅
- **Local working directory**: `/Users/jd-mac/Downloads/C6 – Security x Capstone/C6 – Ecommerce App` exists but is empty and not yet a git repo — this becomes the repo root.
- **Git identity**: no global git config existed. Per user confirmation, commits will use `user.name = jdsaire` (the hard rule's literal required name) and `user.email = 88201583+jdsaire@users.noreply.github.com` (GitHub's noreply format, confirmed via `gh api user --jq .id`), configured **locally in this repo only** — not touching global git config.

## Scaffold reality check — confirmed via dry run in scratch space

Ran `dotnet new blazorwasm -o ShopEase` in the scratchpad to compare against the XML's assumed tree. Differences to adopt (per the guardrail: adopt the template's actual output, never force a rename):

- The template emits a **`Layout/`** folder (`MainLayout.razor`, `MainLayout.razor.css`, `NavMenu.razor`, `NavMenu.razor.css`) not listed in the XML's assumed tree. It gets its own README like every other folder.
- Default `Pages/` contains `Home.razor`, `Counter.razor`, `NotFound.razor`, `Weather.razor`. Per task 2, `Counter.razor` and `Weather.razor` are removed along with their `NavMenu.razor` links and `wwwroot/sample-data/weather.json`; `Home.razor` and `NotFound.razor` are kept as-is.
- `ShopEase.csproj` already sets `TargetFramework=net10.0`, `Nullable=enable`, `ImplicitUsings=enable`, `OverrideHtmlAssetPlaceholders=true` — matches the assumed conventions exactly, no changes needed.
- `wwwroot/index.html` ships with `<base href="/" />` and `<title>ShopEase</title>` already — matches the assumption that this stays untouched in tracked source (only CI rewrites it for Pages).
- Everything else (`Program.cs`, `_Imports.razor`, `App.razor`, `Properties/launchSettings.json`, `wwwroot/lib/bootstrap/*`) matches the standard template with no surprises.

## Confirmed file tree (this run's output)

```
.github/workflows/deploy-pages.yml
.gitignore
README.md
docs/
  README.md
  how-to-run.md
  setup-guide.md
  grading-criteria.md
learning-mode/                          [NEW — user-requested this run, see deviation above]
  README.md
  01-Business-Logic-Foundations.md      — Activity 1 walkthrough (gate 1)
  02-Building-the-Product-Card.md       — Activity 2 walkthrough (gate 2)
  Glossary.md                            — cumulative; extended at gate 2
src/ShopEase/
  ShopEase.csproj
  Program.cs
  App.razor
  _Imports.razor
  README.md                              — includes Codespaces/VS Code run instructions
  Layout/
    README.md
    MainLayout.razor, MainLayout.razor.css
    NavMenu.razor, NavMenu.razor.css
  Models/
    README.md
    Product.cs
    MockProductData.cs
  Services/
    README.md
    ShopDatabase.cs                      — simulated Shop.Products store (honest-simulation invariant)
    Cart.cs                              — registered in DI as singleton
  Pages/
    README.md                            — explains why ProductCard.razor lives here, includes CartTest run steps
    Home.razor, NotFound.razor            (kept from template)
    CartTest.razor
    ProductCard.razor
    Products.razor
  Properties/
    README.md
    launchSettings.json
  wwwroot/
    README.md
    index.html, css/app.css, favicon.png, icon-192.png, lib/bootstrap/*
handoff/
  README.md
  v1/
    README.md
    plan.md                              — this plan, archived
    completion-report.md
```

## Simulated-database design

`Services/ShopDatabase.cs` carries this exact XML doc comment at its point of definition (honest-simulation invariant):

```csharp
/// <summary>
/// Simulated in-memory stand-in for the MySQL "Shop" database's "Products" table.
/// This app is a Blazor WebAssembly client deployed to static GitHub Pages hosting,
/// which has no server process and therefore cannot open a real MySQL connection.
/// This class mirrors the shape of that database — insert, delete, and read of
/// Products rows — entirely in browser memory, for the lifetime of the page.
/// It is not a real database connection; no ADO.NET or Entity Framework
/// integration exists here.
/// </summary>
```

Method shape (naming mirrors SQL vocabulary; behavior does not claim to be one):

```csharp
public class ShopDatabase
{
    private readonly List<Product> _products = new();
    public void InsertProduct(Product product) => _products.Add(product);
    public void DeleteProduct(int productId) => _products.RemoveAll(p => p.ProductID == productId);
    public IReadOnlyList<Product> SelectAllProducts() => _products.AsReadOnly();
}
```

`Cart.cs` takes `ShopDatabase` via constructor injection; `AddProduct`/`RemoveProduct` write through to it, matching the brief's "database component in your Cart methods" requirement. Both `ShopDatabase` and `Cart` are registered as DI singletons in `Program.cs` so cart state is shared across pages.

`Cart.DisplayCartItems()` returns the formatted detail lines (via `Product.GetDetails()`) rather than writing to a real console — CartTest.razor and Products.razor render those lines as visible output, which is how "printing" is honestly satisfied in a browser-only app.

## Ordered commit sequence

One commit per line, pushed directly to `main` (v1 push policy — no branch, no PR):

| # | Commit message | Contents |
|---|---|---|
| 1 | `chore: scaffold Blazor WebAssembly project` | `dotnet new blazorwasm` at `src/ShopEase`; remove Counter/Weather pages, their nav entries, and `sample-data/weather.json`; add `.gitignore` (`bin/`, `obj/`); `git init` + local identity + remote; zero-warning build. First push creates `main`. |
| 2 | `ci: add GitHub Pages deployment workflow` | `.github/workflows/deploy-pages.yml`, three CI-only fixups (base-href → `/frontend_c6_ecommerce/`, SPA `404.html`, `.nojekyll`), reproduced from the sibling repo pattern. |
| 3 | `feat(activity1): add Product class with details formatter` | `Models/Product.cs` — ProductID/Name/Price/Category + `GetDetails()` in the fixed format, with illustrative comments. |
| 4 | `feat(activity1): add simulated Shop database store` | `Services/ShopDatabase.cs` with the honest-simulation doc comment above. |
| 5 | `feat(activity1): add Cart class with add, remove, display, and total methods` | `Services/Cart.cs`; DI registration of `ShopDatabase` + `Cart` as singletons in `Program.cs`. |
| 6 | `feat(activity1): add cart logic test page with seed product data` | `Pages/CartTest.razor` + `Models/MockProductData.cs` — adds 2+ products, removes one, displays cart + total. |
| 7 | `docs(activity1): add learning-mode walkthrough and glossary for Activity 1` | `learning-mode/README.md`, `01-Business-Logic-Foundations.md`, first pass of `Glossary.md` — plain-language, styled after the referenced sibling-repo `learning-mode/` convention, drawing on MasterTranscript part 4 for OOP/C#/database framing. |
| **GATE 1** | — | Build check (zero errors/warnings), manual confirmation of CartTest's add/remove/display/total sequence, summarize commits 1–7, report Pages URL. **Stop for approval.** |
| 8 | `feat(activity2): add ProductCard component with product parameter` | `Pages/ProductCard.razor` — displays product details, "Add to Cart" button, `[Parameter] Product`. |
| 9 | `feat(activity2): add Add to Cart event callback` | `EventCallback<Product>` parameter on ProductCard, invoked on click. |
| 10 | `feat(activity2): add product listing page wired to the cart` | `Pages/Products.razor` — renders cards from seed catalog, handles callback via injected `Cart`, displays cart + total; nav entries added for `/products` and `/cart-test`. |
| 11 | `docs(activity2): add learning-mode walkthrough and update glossary for Activity 2` | `learning-mode/02-Building-the-Product-Card.md`; `Glossary.md` extended with component/parameter/event-callback/DI terms; `README.md` index updated — drawing on MasterTranscript part 2 for Blazor component framing. |
| **GATE 2** | — | Build check, manual confirmation multiple cards render and "Add to Cart" updates cart + total, summarize commits 8–11, report Pages URL. **Stop for approval.** |
| 12 | `docs: add root README and folder READMEs` | Root README (what/how-to-run/tech stack/doc index/out-of-scope index/attribution) + one README per folder, including Codespaces/VS Code run instructions in `src/ShopEase/README.md` and `Pages/README.md`, and the ProductCard-in-Pages placement rationale. |
| 13 | `docs: add setup guide, run instructions, and grading criteria` | `docs/how-to-run.md` (Codespaces + VS Code steps to see CartTest and Products render), `docs/setup-guide.md`, `docs/grading-criteria.md` (18-point breakdown + 6 submission questions, reference only — not answered). |
| — | *(verify, no commit)* | Final build; count + resolve every internal markdown link (report N/N); confirm `git log` shows only `jdsaire`; confirm tracked `index.html` still has `<base href="/" />`; grep for any real-database/real-auth claims (must find none). |
| 14 | `docs: archive build plan and completion report` | `handoff/v1/plan.md` (this plan), `handoff/v1/completion-report.md` (records the learning-mode deviation explicitly), `handoff/v1/README.md`, `handoff/README.md`. |

14 commits total, all direct to `main`.

## Codespaces / VS Code instructions (for docs)

Since Activity 1's "test program" is realized as `CartTest.razor` (a rendered Blazor page, per the honest-simulation architecture already fixed) rather than a literal console app, "operating the cs console" is interpreted as: running the project and viewing that page's rendered output. `docs/how-to-run.md` and `src/ShopEase/README.md` will cover both paths:

- **GitHub Codespaces**: open the repo in a Codespace → integrated terminal → `dotnet run --project src/ShopEase` → open the forwarded port → navigate to `/cart-test`. A note covers checking `dotnet --version` and using the official install script if .NET 10 isn't already present — no `.devcontainer` file is added, since that would be a new file outside the scaffold's own output (scope-ceiling guardrail).
- **VS Code (local)**: clone → open folder → integrated terminal → same `dotnet run` command → browser at the printed localhost URL.

## Verification steps

1. `dotnet build` after every commit — zero errors, zero warnings.
2. Manual render check at each gate (described above).
3. Markdown link resolution count, reported N/N.
4. `git log --format='%an %cn'` shows only `jdsaire` / `jdsaire` on every commit; grep commit messages and all tracked files for AI/agent attribution — must find none.
5. `grep` across tracked files for phrases claiming a real MySQL connection, ADO.NET/EF integration, or real authentication — must find none outside the explicit "this is simulated" disclaimers.
6. Confirm Pages workflow runs and the live URL (`https://jdsaire.github.io/frontend_c6_ecommerce/`) loads after each gate's push, with `/products` and `/cart-test` resolving via the SPA fallback.

## Authorized deviations from the XML (to record in the Completion Report)

- **Learning-mode content is delivered in this run**, per gate, instead of being deferred wholesale — overriding the XML's own guardrail line, per the user's explicit live instruction. Scoped to Activities 1 and 2 only; Activities 3–5 learning-mode content is a natural continuation in later runs, not a re-deferral of the same doctrine item.
- **`learning-mode/` folder added at repo root**, mirroring the sibling repo's convention (not `docs/learning-mode/`), since that's where the referenced style guide keeps it and it cross-links to `docs/how-to-run.md` the same way.
- Everything else in the XML (scope ceiling, honest-simulation invariant, one-gate-per-activity, author identity, v1 push policy, no subagents) is followed unchanged.

Still carried forward (unchanged from the XML): the pre-implementation business-logic flowchart, deferred to a later run so it documents what was actually built rather than what was projected.
