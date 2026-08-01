# Setup Guide

## Prerequisites

- **A .NET SDK that can target `net10.0`.** Check with `dotnet --version`; if
  it doesn't report a `10.x` SDK, install one from
  [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) (or
  see the Codespaces install-script note in
  [`how-to-run.md`](how-to-run.md#2-github-codespaces)).
- **Git**, for cloning the repository.
- **VS Code** (optional, for local development) — the C# Dev Kit extension is
  recommended but not required; `dotnet run` works from any terminal.
- **A GitHub account** (optional) — only needed if you want to open the
  project in a Codespace instead of cloning it locally.

No database, no additional services, and no environment variables are needed.
The project has no dependency beyond what `dotnet new blazorwasm` itself
installs — see [`../README.md`](../README.md#about-the-simulated-database) for
why: the brief's database requirement is met with an in-memory simulation
instead of a real MySQL server.

## First-Time Setup

```bash
git clone https://github.com/jdsaire/frontend_c6_ecommerce.git
cd frontend_c6_ecommerce
dotnet build src/ShopEase
```

A clean build with `0 Warning(s)` and `0 Error(s)` confirms everything
restored and compiled correctly. From here, see
[`how-to-run.md`](how-to-run.md) for how to actually launch it and see it in a
browser.

## Project Structure

See [`../src/ShopEase/README.md`](../src/ShopEase/README.md) for a tour of the
project's folders, or [`../learning-mode/`](../learning-mode/README.md) for a
plain-language explanation of how the code fits together.
