# handoff/v4.1/

Three surgical fixes closing out loose ends found by using the app after Activity 4's
`v4` run merged: a critical navigation regression (every sign-in entry point 404'd on
the live deploy), a search field with no clear control or autocomplete, and a glued-
together header. Executed in Auto Mode against `deploy/v4-secure-coding`, then — after
that branch and `main` diverged (`v4`'s PR merged before this run's PR did) — merged
directly into `main`.

- [`plan.md`](plan.md) — a retrospective root-cause diagnosis, written after the fact
  since this run executed in Auto Mode rather than Plan Mode: the exact base-href
  regression that broke navigation, the combobox rebuild of the search field, and the
  header-spacing fix.
- [`completion-report.md`](completion-report.md) — what actually happened: the three
  commits and PR #5, the direct-into-`main` integration episode (including why PR #6 was
  closed rather than merged), a PASS/FAIL check against six success criteria, and the
  full Deploy Timing log.
