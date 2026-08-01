# Layout/

The app's shell: `MainLayout.razor` wraps every page with the sidebar and
content area, and `NavMenu.razor` is that sidebar's navigation links (Home,
Products, Cart Test). Both files, along with their `.razor.css` companions,
are the unmodified output of `dotnet new blazorwasm`, except for the nav links
themselves, which were changed to point at this app's own pages instead of the
template's sample Counter/Weather pages.

This folder isn't part of the file tree originally assumed for this run —
`dotnet new blazorwasm` emits it by default, and it was kept as-is rather than
restructured, since the scaffold's actual output takes priority over any prior
assumption about the tree.
