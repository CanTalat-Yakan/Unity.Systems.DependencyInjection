# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# Dependency Injection

> Quick overview: Lightweight, attribute‑based runtime dependency injection for MonoBehaviours. Providers expose instances via annotated methods; injectables receive them into fields, methods, or properties after scene load.

Dependencies are discovered at startup from provider components and written into targets marked for injection. A simple registry keyed by type is populated from provider methods; injection then assigns resolved instances to fields, calls annotated methods with resolved parameters, or sets properties. Optional validation reports missing dependencies; a clear utility resets injected fields.

![screenshot](Documentation/Screenshot.png)

## Features
- Minimal, reflection‑based DI
  - Mark fields, methods, or properties with `[Inject]` to receive dependencies
  - Mark provider methods with `[Provide]` to supply instances to the registry
  - Implement `IDependencyProvider` on any MonoBehaviour that offers `[Provide]` methods
- Automatic startup
  - AfterSceneLoad scan: providers are registered and injectables are populated automatically
- Multiple injection styles
  - Field injection (null fields are assigned)
  - Method injection (parameters are resolved and method invoked)
  - Property injection (setter receives resolved instance)
- Utilities
  - `DependencyInjector.Register<T>(instance)` to add/replace a mapping at runtime
  - `DependencyInjector.ValidateDependencies()` to log missing/invalid dependencies
  - `DependencyInjector.ClearDependencies()` to set all `[Inject]` fields back to null
- Zero external dependencies
  - Small runtime .asmdef, no third‑party DI framework required

## Requirements
- Unity 6000.0+
- Add provider and consumer MonoBehaviours to your scene(s)
- Public or non‑public members can be injected; attributes are discovered via reflection

## Usage
1) Provide dependencies
   - Create a MonoBehaviour that implements `IDependencyProvider`
   - Add one or more parameterless methods marked `[Provide]` that return the dependency type to register
   - Place the component in the scene so it is discovered at startup
   
   Example:
   ```csharp
   public interface IClock { DateTime Now { get; } }
   public class SystemClock : IClock { public DateTime Now => DateTime.Now; }

   public class ClockProvider : MonoBehaviour, IDependencyProvider
   {
       [Provide]
       private IClock ProvideClock() => new SystemClock();
   }
   ```

2) Consume dependencies
   - In any MonoBehaviour, mark a field/property/method with `[Inject]` of the type you need
   - Leave injected fields null in the inspector; the injector assigns them after scene load
   
   Example (field and method injection):
   ```csharp
   public class ClockConsumer : MonoBehaviour
   {
       [Inject] private IClock _clock;

       [Inject]
       private void Initialize(IClock clock)
       {
           // Both _clock and the parameter are resolved from the registry
       }
   }
   ```

3) Play
   - On first scene load, providers are registered and injectables are populated automatically

4) Validate or adjust
   - Call `DependencyInjector.ValidateDependencies()` at runtime to print missing mappings
   - Call `DependencyInjector.Register<IMyType>(instance)` to add/override a mapping programmatically
   - Call `DependencyInjector.ClearDependencies()` to reset all `[Inject]` fields to null

## How It Works
- Scene scan
  - All `MonoBehaviour` instances are collected and filtered for providers (`IDependencyProvider`) and injectables (any member with `[Inject]`)
- Registry
  - For each provider, every `[Provide]` method is invoked; the returned instance is stored in a dictionary keyed by its return `Type`
  - `Register<T>(instance)` sets or replaces the mapping for the exact `typeof(T)` key
- Injection
  - Fields: if null, set to the resolved instance of the field’s type; warn if already set
  - Methods: resolve each parameter type and invoke the method; throw if any parameter cannot be resolved
  - Properties: resolve the property type and set via the property setter; throw if unresolved
- Validation and clearing
  - Validation scans `[Inject]` fields and reports types that are neither provided nor already assigned
  - Clearing sets all `[Inject]` fields back to null

## Notes and Limitations
- Exact type keys
  - Resolution matches the requested `Type` exactly; no hierarchy/interface fallback is performed unless you register using the interface as the key (e.g., `Register<IMyInterface>(instance)`)
- Duplicate providers
  - Provider registration uses a dictionary; duplicate `[Provide]` return types may collide. `Register<T>` overwrites; provider registration uses `Add` and will throw on duplicates
- Lifecycle
  - Automatic injection runs after scene load. If dependencies are added later, there is no public API to re‑scan/inject; prefer registering before injection or implement your own call site initialization
- Thread safety
  - The registry is a static `Dictionary<Type, object>` without locking; use from the main thread
- Member state
  - Field injection skips fields that are already non‑null and logs a warning

## Files in This Package
- `Runtime/DependencyInjector.cs` – Attributes (`Inject`, `Provide`), provider interface, registry, scan, inject, validate, clear
- `Runtime/Examples/` – Example provider/consumers (`DependencyInjectionProvider.cs`, `ClassA.cs`, `ClassB.cs`)
- `Runtime/UnityEssentials.DependencyInjection.asmdef` – Runtime assembly definition

## Tags
unity, dependency-injection, di, attributes, reflection, runtime, services, providers
