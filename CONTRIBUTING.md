# Contributing

Keep changes aligned with the project boundaries:

- application-facing capabilities are **Services**;
- third-party SDK/protocol/package boundaries are **Adapters**;
- ViewModels remain plain C#;
- UI Toolkit-specific code stays in Views/Binders;
- dependencies are wired through VContainer rather than static access;
- R3 subscriptions must have explicit disposal ownership.

For architecture-changing pull requests, explain the dependency direction before adding a new abstraction.
