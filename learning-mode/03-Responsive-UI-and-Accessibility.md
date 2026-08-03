# 03 — Responsive UI and Accessibility

## Picking Up From 02

`02-Building-the-Product-Card.md` covered how `ProductCard` and `Products.razor`
work together to render and interact with the catalog. Everything up to that
point looked the same regardless of window size, and nothing checked whether a
keyboard-only visitor could actually use any of it. This file covers Activity 3:
the pass that made the storefront look intentional at every screen size and
usable without a mouse.

## Two Stylesheets Instead of One: `app.css` and `site.css`

Before this activity, every rule this project had written — the storefront
grid, the product card, the landing hero, the header cart summary — lived
inside `wwwroot/css/app.css`, alongside the Blazor project template's own
default styles (the loading spinner, the error banner, and so on). The
capstone brief for this activity names a file called `site.css` specifically,
so this run created [`wwwroot/css/site.css`](../src/ShopEase/wwwroot/css/site.css)
and **moved** this project's own rules into it, leaving the template's rules
behind in `app.css`. Nothing about the move changed what anything looked like
— it was verified by literally concatenating the two new files back together
and checking the result matched the old single file byte for byte. See
[`docs/activity-3-decisions.md`](../docs/activity-3-decisions.md) for the full
reasoning.

## What a Media Query Is

A **media query** is a block of CSS that only applies when some condition
about the browser window is true — most commonly its width. This app uses
`min-width` media queries, meaning "apply this once the window is at least
this wide":

```css
.storefront-grid {
    grid-template-columns: 1fr;   /* the default: one column */
}

@media (min-width: 641px) {
    .storefront-grid {
        grid-template-columns: repeat(2, 1fr);   /* two columns, tablet and up */
    }
}

@media (min-width: 1025px) {
    .storefront-grid {
        grid-template-columns: repeat(3, 1fr);   /* three columns, desktop only */
    }
}
```

This is called **mobile-first**: the plain rule with no media query around it
is written for the narrowest screen, and each media query layers on a change
for wider ones. It's the same pattern [`Layout/MainLayout.razor.css`](../src/ShopEase/Layout/MainLayout.razor.css)
already used for the sidebar (a single column on a phone, a fixed 250px
sidebar from 641px up) — this activity reused that same 641px line for the
product grid instead of picking a new number, so the whole page changes
character at one consistent width. 1025px is the one new line this activity
adds, splitting "641px and up" into a tablet tier and a wider desktop tier.

## Building a Clearer Visual Hierarchy

**Visual hierarchy** just means: the most important thing on screen should be
the easiest to notice, and less important things should visibly recede. On a
product card, the price is what a shopper scans for first, so it was made
larger and bolder than everything around it; the category label and stock
count are useful but secondary, so they stay small and a muted gray instead of
competing with the price for attention. The same idea shows up as more
deliberate whitespace — more breathing room inside each card, and a slightly
larger gap between cards in the grid — since cramped spacing makes everything
feel equally important, which is the opposite of a hierarchy.

## Accessibility: Contrast, Keyboard Use, and Focus

**Color contrast** is a measured ratio between a foreground color and its
background — not a guess about whether something "looks readable." The Web
Content Accessibility Guidelines (WCAG) set a minimum of 4.5:1 for normal body
text and 3:1 for larger text and meaningful UI boundaries like borders. This
activity actually computed those ratios from this project's real CSS colors
rather than eyeballing them, and found one genuine failure: the light gray
border color used around cards and the filter toolbar measured about 1.26:1
against a white background — badly short of the 3:1 a border needs. It was
replaced with a darker gray that measures about 3.62:1. Every text color
already in use (the price's blue, the muted gray, the header cart summary's
white-on-blue) was checked the same way and already passed.

**Keyboard operability** means every interactive control — the category and
sort dropdowns, the quantity stepper's +/− buttons, the remove control, the
"Show more" button, the nav links — can be reached with the Tab key and
activated with Enter or Space, without a mouse. Because this app uses real
`<button>` and `<select>` elements everywhere (rather than, say, a clickable
`<div>` pretending to be a button) and never sets a custom `tabindex`
anywhere, this was already true by construction; this activity's job was
mostly to verify it and make sure the visual sign that something is focused —
a **focus indicator** — was actually visible. A few controls (the stepper
buttons, the remove control, the toolbar's dropdowns) weren't covered by the
project template's existing focus-ring style, so this activity extended that
same style to them instead of inventing a new one.

## What's Next

Activities 4 and 5 (secure coding practices with authentication, and
persisted state management) are still separate, later deliveries against this
same repository. When they land, this folder gains `04-...md` and `05-...md`
continuing exactly where this file leaves off.
