# handoff/v4/

Activity 4: input validation and sanitization, simulated authentication built on
Blazor's real `AuthenticationStateProvider`/`AuthorizeView` abstraction, cart gating on
sign-in, and a security test pass — combined with a patch adding a checkout screen,
retroactive Activity-3-equivalent styling/accessibility coverage for the login and
checkout screens, and a standing deploy-timing measurement. Built on the `main` produced
by merging PR #3 (`handoff/v3/`).

- [`plan.md`](plan.md) — the combined core-prompt-plus-patch plan: the structural
  problems in the capstone brief this run resolves, the design decisions behind the
  validation service, simulated authentication, and checkout screen, and the full
  ordered commit sequence as planned.
- [`completion-report.md`](completion-report.md) — what actually happened: the full
  commit list and PR, a PASS/FAIL table against all 21 success criteria, measured
  contrast ratios, authorized deviations, decisions resolved autonomously, every
  carried-forward open item, and the Deploy Timing log.
