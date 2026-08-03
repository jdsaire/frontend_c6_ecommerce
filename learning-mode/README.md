# Learning Mode

A plain-language walkthrough of how this app was built and why, written for a
reader with some general programming background but no prior experience with
Blazor, front-end frameworks, or web development specifically.

This folder grows alongside the app itself: a new file is added once its
corresponding activity is built, rather than all at once at the end. It currently
covers Activities 1 through 3, plus a storefront-bridge run between Activities 2
and 3 that gave the app a real retail shell (catalog, imagery, quantity controls,
filters, a persistent cart summary) without changing anything either activity's
underlying code was graded on — that run's changes are folded into `01` and `02`
inline rather than given a numbered file of their own, since it extended what
those files describe rather than adding a new activity. Activities 4-5 are still
separate, later deliveries against this same repository, and their walkthrough
files land when that code does.

## What's here

| File | Covers |
|---|---|
| [`01-Business-Logic-Foundations.md`](01-Business-Logic-Foundations.md) | Classes, properties, and methods in plain terms; the `Product` and `Cart` classes; the honest reason the "database" is simulated instead of real; and the `CartTest` page that proves `Cart`'s four methods work. |
| [`02-Building-the-Product-Card.md`](02-Building-the-Product-Card.md) | What a Blazor component and a component parameter are; the `ProductCard` component and its deliberate `Pages/` placement; the "Add to Cart" event callback; and the `Products` listing page that ties cards back to the same shared `Cart`. |
| [`03-Responsive-UI-and-Accessibility.md`](03-Responsive-UI-and-Accessibility.md) | What a CSS media query is and why breakpoints matter; the `site.css`/`app.css` split; the mobile/tablet/desktop breakpoints on the storefront grid; and the accessibility pass (contrast, keyboard navigation, focus indicators). |
| [`Glossary.md`](Glossary.md) | Every term used across the walkthrough files, defined in plain language, with a note on where it appears in the project. |

## How to read this

Read the numbered files in order — each one picks up exactly where the last left
off. `Glossary.md` isn't part of that sequence; it's a reference to dip into
whenever a word in one of the numbered files doesn't ring a bell.

## Want to see the app itself?

These files describe what the code does — they don't replace actually clicking
through it. [`docs/how-to-run.md`](../docs/how-to-run.md) covers every way to get
the app open, whether that's GitHub Codespaces, VS Code, or the live GitHub Pages
URL once it's deployed.
