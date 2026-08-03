# Security Testing

Activity 4, Step 3 asks for a security test pass. This app's test surface is the
[`/security-test`](../src/ShopEase/Pages/SecurityTest.razor) page — following the
`/cart-test` precedent from Activity 1: a visible, checkable evidence surface for a
graded step, not a hidden test suite.

## What It Runs

`/security-test` calls
[`InputValidationService`](../src/ShopEase/Services/InputValidationService.cs) — the same
class every text input in this app (product search, login, checkout) is validated
through — against five benign, well-known probe strings, and reports pass/fail per case.
A case passes when the service's actual accept/reject behavior matches what's documented
as expected below, not when the probe string itself "does" anything; none of these
strings are working exploits, because there is no SQL engine or script-executing sink on
the other end of this client-side app for them to exploit.

| # | Case | Input | Expected | Why |
|---|---|---|---|---|
| 1 | Script tag (XSS) | `<script>alert('test')</script>` | Rejected | `<` and `>` are outside the allow-list character policy. |
| 2 | Quote-and-OR tautology (SQL injection) | `' OR '1'='1` | Rejected | `'` is outside the allow-list, and the pattern is also caught by `ContainsInjectionPattern`. |
| 3 | Statement terminator + comment ("Bobby Tables", SQL injection) | `Robert'); DROP TABLE Products;--` | Rejected | `'`, `;`, and `--` are all outside the allow-list / caught by `ContainsInjectionPattern`. |
| 4 | Legitimate hyphenated catalog name | `27-Inch Monitor` | **Accepted** | The allow-list must not reject real product names — this case exists specifically to prove the policy isn't over-strict (see the guardrail in `handoff/v4/plan.md`). |
| 5 | Over-length input | 500 `A` characters | Rejected | Exceeds `InputValidationService.DefaultMaxLength` (60). |

## Encoding Demonstration

The same page also renders the script-tag probe string (case 1's input) back to the
screen through normal Razor interpolation — `@ScriptProbe`, never `MarkupString`. Blazor
HTML-encodes interpolated values by default, so the literal text renders instead of an
executed script. This is not something this run added; it's Blazor's existing behavior,
and the point of the demonstration is to prove it's still true after this run's changes
(no `MarkupString` usage exists anywhere in this app — see `docs/security-decisions.md`).

## What This Does and Does Not Prove

- **Confirmed by running the app's own logic against known-benign inputs**: the
  allow-list character policy and the injection-pattern detector behave as documented
  for these five cases, and Blazor's default encoding still applies.
- **Not proven**: that this app is safe against a determined attacker. Every check here
  runs in the browser. Anyone with developer tools can call `Cart.AddProduct` directly,
  bypass the validation layer entirely, or simply edit the DOM. There is no server here
  to enforce anything — see the client-side honesty invariant in
  `docs/security-decisions.md`, which governs how every security claim in this
  repository is worded.
- **Not exercised by this page**: authentication bypass, session handling, or anything
  beyond input validation — those are covered narratively in `docs/security-decisions.md`
  and by the gate-2 walkthrough in `handoff/v4/completion-report.md`.
