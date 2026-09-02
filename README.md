# UMVVM

A small, opinionated reference architecture for building **WPF-inspired MVVM applications in Unity 6.6** with **UI Toolkit**, **R3**, and **VContainer**.

The goal is not to force desktop WPF APIs into Unity. The goal is to preserve the useful architectural properties of MVVM: plain ViewModels, explicit dependency boundaries, observable state, disposable bindings, testable application logic, and a single composition root.

## Stack

- Unity 6.6
- UI Toolkit + `PanelRenderer` for runtime UI
- UI Toolkit native runtime data binding for ViewModel → View state
- R3 1.3.1 for reactive application flows and event composition
- VContainer 1.18.0 for dependency injection and lifetime management
- `Task` + `CancellationToken` for asynchronous application boundaries

## Architecture

```text
Presentation
  Shell / Navigation
  Screen ViewModels
  Screen Binders
  UI Toolkit UXML + USS
        |
        v
Application
  Services                 <- application-facing functionality
        |
        v
Infrastructure
  Adapters                 <- external SDK / package / protocol boundaries
        |
        v
External systems
  HTTP / gRPC / Firebase / platform SDK / persistence / etc.
```

### Service vs Adapter

**Service = application-facing functionality.** It expresses what the application can do.

**Adapter = adaptation of an external library, SDK, platform API, or protocol into our architecture.** It expresses how an external dependency is accessed.

```csharp
public sealed class NotificationService : INotificationService
{
    private readonly IHttpAdapter _http;
    private readonly IPushAdapter _push;

    public NotificationService(IHttpAdapter http, IPushAdapter push)
    {
        _http = http;
        _push = push;
    }
}
```

This replaces ambiguous names such as `HttpCoreService`, `GrpcCoreService`, or `FirebaseCoreService`.

## MVVM roles

- **View** — UXML/USS and UI Toolkit elements. No business logic.
- **ViewModel** — plain C# state and user intents. Exposes bindable properties through Unity Properties and `INotifyBindablePropertyChanged`; uses application services and R3 where reactive composition adds value. No `VisualElement`, `MonoBehaviour`, or scene lookup.
- **Binder** — thin disposable bridge between a View and a ViewModel. Assigns the ViewModel as the UI Toolkit `dataSource`, wires commands/events, and owns visual behavior that is not a simple property binding.
- **Service** — application use cases/capabilities. May coordinate several adapters.
- **Adapter** — integration boundary around HTTP, gRPC, Firebase, platform SDKs, storage, analytics SDKs, etc.
- **Composition Root** — VContainer registrations in one place.

## Global navigation

UMVVM uses one persistent application shell. `INavigationService` exposes the current `AppRoute` as R3 state. `AppShellController` swaps screen UXML and disposes the previous Binder/ViewModel. Navigation therefore does not require scenes or static singletons.

For larger applications the enum can evolve into typed route objects carrying parameters, while the navigation service contract can remain stable.

## Folder layout

```text
Assets/
  Application/
    Notifications/
    Users/
  Composition/
  Infrastructure/Adapters/
  Presentation/
    Common/
    Navigation/
    Screens/
    Shell/
Assets/Resources/
  Screens/
  Shell/
```

## Dependency restore and first launch

R3 has **two Unity installation parts**: the core `R3` NuGet package and the `R3.Unity` UPM extension. UMVVM pins both to `1.3.1`; the core package is restored with NuGetForUnity and the Unity extension is declared in `Packages/manifest.json`.

On a fresh clone, Unity can try to compile `R3.Unity` before NuGetForUnity has restored the core `R3` assembly. If Unity shows missing `R3.Observable`, `R3.FrameProvider`, or `R3.Collections` errors on the first import, choose **Ignore** rather than Safe Mode, then run **NuGet -> Restore Packages**. NuGetForUnity also performs an automatic restore once its editor plugin has loaded. After the restore finishes, Unity recompiles the project normally.

For CI, restore NuGet dependencies before opening/building the project with the NuGetForUnity CLI.

## Running the demo

Open the project in Unity 6.6 and enter Play Mode from `Assets/Scenes/Main.unity`. `Bootstrap` creates a runtime `PanelRenderer` and the VContainer root automatically, so the architecture does not depend on hand-wired scene objects. The renderer uses the committed `Resources/UI/MainPanelSettings.asset`, while its visual tree comes from `Resources/Shell/AppShell.uxml`.

The three screens demonstrate:

1. **Home** — declarative UI Toolkit runtime binding plus ViewModel-driven navigation.
2. **Profile** — `ProfileViewModel -> IUserService -> IHttpAdapter`.
3. **Settings** — `SettingsViewModel -> INotificationService -> IHttpAdapter + IPushAdapter`.

The adapters are intentionally fake. Replace them with real package integrations while keeping the application and presentation layers unchanged.

## Design rules

1. ViewModels are plain C# and never touch Unity UI objects; presentation ViewModels may implement Unity's lightweight runtime-binding contract.
2. Prefer declarative UXML `DataBinding` for simple ViewModel → View state. Binders own `dataSource` assignment, commands/events, and non-trivial visual behavior, but no business decisions.
3. Application Services do not reference concrete SDKs.
4. External dependencies are wrapped by Adapters.
5. No service locator outside infrastructure/composition code.
6. Every reactive subscription has an explicit lifetime.
7. Screen lifetime ends when navigation replaces the screen.
8. Keep Unity scenes thin; composition and application state should not be hidden in scene hierarchies.

## Native data binding + R3

UMVVM intentionally uses both systems rather than making them compete. UI Toolkit runtime `DataBinding` handles straightforward ViewModel → View projection declaratively in UXML. Each Binder assigns its ViewModel to `VisualElement.dataSource`, and bindable ViewModel properties use `[CreateProperty]` plus `INotifyBindablePropertyChanged` so UI Toolkit can update only affected bindings.

R3 remains the tool for reactive application flows: navigation state, event-stream composition, debouncing/throttling, combining asynchronous sources, and similar logic where an observable pipeline is clearer than property binding. Binders remain for commands and UI-specific behavior such as toggling USS state classes.

This is deliberately close to WPF's `DataContext` + XAML binding model without trying to reproduce WPF APIs inside Unity.

## License

MIT. See [`LICENSE`](LICENSE).

### Input handling

The project keeps Unity's **Input System** package (`com.unity.inputsystem` 1.20.0) because Player Settings use **Active Input Handling = Input System Package (New)**. UI Toolkit runtime input relies on this configuration. The starter `.inputactions` asset is intentionally omitted because UMVVM does not define gameplay input.
