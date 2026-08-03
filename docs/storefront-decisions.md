# Storefront Decisions

Design decisions from the storefront-bridge run, recorded here as durable
project documentation rather than left implicit in commit messages.

## Stock Is Displayed, Not Enforced

Every product carries a `Stock` count (`Product.cs`), and the storefront
card shows it. Nothing in this app currently checks that count against cart
quantity — a shopper can add more units of a product than `Stock` says are
on hand, and a card never shows "out of stock" or disables its controls for
a zero-stock item.

This is a deliberate deferral, not an oversight. Enforcing it correctly
needs more than a bounds check: what happens when two things read stale
stock at once, what the UI should say when a card the shopper is looking at
just sold out, whether `Stock` decrements as items enter the cart or only at
a (nonexistent, in this app) checkout step. Those are real product
decisions, not a one-line fix, so they're left for a later run rather than
implemented half-considered here.

## Quantity Floor of 1, Ceiling of 10, Explicit Removal Only

Each cart line has a stepper bounded to 1–10 units
(`Cart.MinQuantity`/`Cart.MaxQuantity`). Decrementing at quantity 1 does
**not** remove the item — the minus control is disabled at the floor
instead. Removing a line from the cart is only possible through its own
explicit control (the trash icon on the card), which reuses
`Cart.RemoveProduct(int)` exactly as Activity 1 defined it.

This resolves an ambiguity the visual-inspection annotations raised
directly: whether a quantity of zero should double as the removal
mechanism. It deliberately does not. A stepper that silently deletes a line
on its last decrement is a well-known source of accidental cart loss — a
shopper meaning to go from 2 to 1 who taps twice, or double-taps by
accident, would otherwise lose the item entirely with no separate
confirmation. Requiring a distinct, clearly-labeled control for removal
means "reduce quantity" and "remove this item" stay two different actions
with two different affordances, matching how most real storefronts behave.
