# Activity 3 Decisions

Design decisions from the Activity 3 (responsive UI/UX) pass, recorded here
as durable project documentation rather than left implicit in commit
messages — following the same convention as
[`storefront-decisions.md`](storefront-decisions.md).

## Why `site.css` Exists Alongside `app.css`

The capstone brief's Activity 3 Step 1 names `wwwroot/css/site.css`
explicitly, but this repository's storefront styling had all grown inside
`wwwroot/css/app.css` since the storefront-bridge run. Rather than either
ignoring the brief's exact filename or dumping everything into one more
file, `site.css` was created and this project's own storefront, landing,
product-card, and cart-summary rules were **moved** into it — verbatim,
not rewritten — leaving the Blazor template's own base styles (`html,body`,
`.btn-primary`, the loading spinner, the error boundary, form-floating
placeholders, etc.) behind in `app.css`.

The split is meaningful, not cosmetic: `app.css` is template-owned scaffold
CSS this project didn't write and wouldn't extend; `site.css` is entirely
this project's own design system. The move commit was verified to be
byte-for-byte lossless — concatenating the resulting `app.css` and
`site.css` reproduces the pre-split `app.css` exactly — before any styling
change was layered on top of it in later commits.

## Breakpoints: 641px and 1025px

Three bands were required: mobile, tablet, and desktop. Rather than invent
new numbers, **641px** reuses the sidebar/nav cutover this app already had
in `Layout/MainLayout.razor.css` and `Layout/NavMenu.razor.css` (both
scoped, both predating this run) — below it the sidebar collapses into a
mobile hamburger menu; at or above it, the sidebar becomes a fixed 250px
column. Using the same number for the storefront grid means the whole page
changes character at one consistent width instead of the sidebar and the
product grid disagreeing about where "mobile" ends.

**1025px** is the new line this run adds, splitting what used to be a single
"641px and up" band into a distinct tablet tier (641px–1024px) and desktop
tier (1025px and up).

Concretely, styling is mobile-first: base rules in `site.css` target the
narrowest viewport, and two `min-width` media queries layer on progressively
larger changes — matching the mobile-first pattern already used in
`MainLayout.razor.css`/`NavMenu.razor.css`, rather than introducing a
different (e.g. max-width-first) convention alongside it:

- **`@media (min-width: 641px)`** — applies from tablet upward: the
  storefront toolbar switches from stacked to a single row, the landing
  hero's heading returns to full size, the header cart summary regains its
  slightly roomier padding, and the product grid moves from 1 to 2 columns.
- **`@media (min-width: 1025px)`** — applies at desktop only: the product
  grid moves from 2 to 3 columns.

The grid is the one element that genuinely needs three distinct values
(1 / 2 / 3 columns); the toolbar, hero, and header only need two (mobile vs.
everything else), so they don't get a redundant third tier for properties
that wouldn't visibly differ between tablet and desktop. Product card
internals (image aspect ratio, type scale, spacing) were left fluid at every
width — the arithmetic at the tightest real case (a 768px tablet, 250px
sidebar, existing 2rem/1.5rem content padding, 2-column grid) still leaves
each card comfortably above this app's original ~220px minimum card-width
assumption, so no card-internal override was needed.

## "Show More" Over Pagination

Kept from the run's resolved decisions, restated here for the record: the
storefront uses progressive "Show more" disclosure (6 products initially,
+6 per press, a live "Showing X of Y products" count) instead of
page-number pagination or an added page-size filter. Progressive disclosure
preserves the active category/sort filter across reveals — page-number
navigation would either drop that context or need to encode it into the
URL, which was more machinery than a 12-product catalog justifies — and it
avoids small numeric tap targets on mobile. It applies at every breakpoint,
including desktop, so there's one code path to reason about and to make
accessible, rather than a mobile-only affordance sitting beside a
desktop-only one.

**Known limitation, not a silent assumption:** this pattern degrades the
same way endless scrolling does once a catalog gets large — a shopper
wanting item #140 of 200 has to press "Show more" repeatedly. At this
catalog's current size (12 products) that's a non-issue. If the catalog
ever grows past roughly 50 products, page-number pagination should be
revisited.

## `/cart-test` Is Out of Scope for This Pass

`Pages/CartTest.razor` (Activity 1's required test program, reachable at
`/cart-test` but unlinked from navigation since the storefront-bridge run)
inherits whatever `app.css` base styling and `MainLayout` chrome it gets for
free, and it isn't visually broken by anything in this run. It received no
dedicated responsive or accessibility work, however — it was never part of
the graded storefront/landing surface this Activity 3 pass targets, and
treating it as in-scope would mean styling a page whose entire purpose is
to stay a plain, unstyled proof of Activity 1's `Cart` logic.
