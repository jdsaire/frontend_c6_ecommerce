# Learning Mode

A plain-language walkthrough of how this app was built and why, written for a
reader with some general programming background but no prior experience with
Blazor, front-end frameworks, or web development specifically.

This folder grows alongside the app itself: a new file is added once its
corresponding activity is built, rather than all at once at the end. It currently
covers Activities 1 through 5, plus a storefront-bridge run between Activities 2
and 3 that gave the app a real retail shell (catalog, imagery, quantity controls,
filters, a persistent cart summary) without changing anything either activity's
underlying code was graded on — that run's changes are folded into `01` and `02`
inline rather than given a numbered file of their own, since it extended what
those files describe rather than adding a new activity. Activity 4's file also
covers the checkout screen a later patch added to that same run, for the same
reason — one screen's story, not a separate numbered file. Activity 5 closes out
the project's graded code — there is no Activity 6.

## What's here

| File | Covers |
|---|---|
| [`01-Business-Logic-Foundations.md`](01-Business-Logic-Foundations.md) | Classes, properties, and methods in plain terms; the `Product` and `Cart` classes; the honest reason the "database" is simulated instead of real; and the `CartTest` page that proves `Cart`'s four methods work. |
| [`02-Building-the-Product-Card.md`](02-Building-the-Product-Card.md) | What a Blazor component and a component parameter are; the `ProductCard` component and its deliberate `Pages/` placement; the "Add to Cart" event callback; and the `Products` listing page that ties cards back to the same shared `Cart`. |
| [`03-Responsive-UI-and-Accessibility.md`](03-Responsive-UI-and-Accessibility.md) | What a CSS media query is and why breakpoints matter; the `site.css`/`app.css` split; the mobile/tablet/desktop breakpoints on the storefront grid; and the accessibility pass (contrast, keyboard navigation, focus indicators). |
| [`04-Input-Validation-and-Authentication.md`](04-Input-Validation-and-Authentication.md) | Input validation and sanitization in plain terms; why "prevent SQL injection" and "prevent XSS" mean something different when there's no SQL and Blazor already encodes by default; the search box and login form built with `EditForm`/`DataAnnotations`, later rebuilt as an accessible combobox with catalog autocomplete; simulated authentication with Blazor's real `AuthenticationStateProvider`/`AuthorizeView`; gating the cart on sign-in from the calling layer, plus the base-relative-href fix that navigation depends on; and the checkout screen and its gating. |
| [`05-State-Management-and-Persistence.md`](05-State-Management-and-Persistence.md) | Local storage vs. session storage in plain terms; JS interop and how C# reaches `localStorage`; `CartStorageService` and why it stores `ProductID`s, not full products; and why `Cart.cs` needed zero changes to gain persistence. |
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
