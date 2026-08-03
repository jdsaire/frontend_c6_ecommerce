# wwwroot/images/

Placeholder product imagery for the storefront card grid.

- [`electronics.svg`](electronics.svg) — Electronics category
- [`home-goods.svg`](home-goods.svg) — Home Goods category
- [`computer-accessories.svg`](computer-accessories.svg) — Computer Accessories category
- [`audio.svg`](audio.svg) — Audio category

## Source and License

All four are locally authored for this project — simple flat-color SVG
glyphs, drawn from scratch in this repo, one per catalog category rather
than one per product. There is no external source and therefore no license
to track: no stock-photo library, no hotlinked host, nothing pulled from
bestbuy.com or any other retailer.

This was a deliberate choice over sourcing real photography. A stock-image
source's redistribution license would need to be positively confirmed before
committing an image to a public repository, and that couldn't be done
reliably in the environment this run was built in — so authored placeholders
were used instead of guessing at a license. `Product.ImageUrl` in
[`../../Models/Product.cs`](../../Models/Product.cs) points at whichever of
these four files matches that product's category.
