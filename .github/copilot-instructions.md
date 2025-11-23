## Repo orientation — quick facts

- Project type: WPF desktop app (C#, .NET). Solution file: `gentech_services.sln`. Project file: `gentech_services.csproj`.
- UI pattern: lightweight MVVM. Key MVVM helpers live in `MVVM/`:
  - `ViewModelBase.cs` — implements INotifyPropertyChanged with `OnPropertyChanged`.
  - `RelayCommand.cs` — simple ICommand wrapper using `Action<object>` and `Func<object,bool>`.

## Big-picture architecture

- Views (XAML) bind to ViewModels. ViewModels inherit from `MVVM/ViewModelBase` and expose properties that call `OnPropertyChanged`.
- Commands use `MVVM/RelayCommand` and rely on `CommandManager.RequerySuggested` for requery behavior.
- Domain objects live in `Models/` (e.g., `Product.cs`, `Sale.cs`, `ServiceOrder.cs`). Some model files have commented relations (look for commented `ICollection<T>` lines).
- Services and data access are expected under `Services/` (check folder for concrete implementations). `Resources/Assets` holds app resources.

## Project-specific conventions and gotchas

- ViewModel visibility: MVVM helpers are `internal` by default. When adding ViewModels consumed by XAML, make them `public` if needed to avoid XAML type visibility issues.
- RelayCommand signature: commands accept `object` parameters. When generating bindings, ensure parameter types match (use casting inside `Execute`).
- Property change pattern: always call `OnPropertyChanged` (no Fody/IL weaving in use).
- Namespace inconsistency: `Models/Sales.cs` currently uses `ProductServicesManagementSystem.Models` — prefer `gentech_services.Models` for new/changed files to keep namespaces consistent.

## Build / run / debug (how developers normally run this repo)

- Recommended (CLI):
  - Build: run `dotnet build` from the repository root (or open `gentech_services.sln` in VS). On Windows with Visual Studio, open the solution and run as usual (WPF debugging supported).
  - Run: `dotnet run --project c:\Users\asus\Desktop\gentech_services\gentech_services.csproj` (or run from Visual Studio).

## Common change patterns (examples)

- Adding a ViewModel:
  - Create `ViewModels/MyViewModel.cs` that inherits `MVVM.ViewModelBase`.
  - Expose properties that call `OnPropertyChanged()` in the setter.
  - Expose commands as `RelayCommand` instances (initialize in ctor).
  - Hook DataContext in XAML code-behind or via ViewModelLocator.

- Command example (from repo pattern):
  - `public RelayCommand SaveCommand { get; }`
  - `SaveCommand = new RelayCommand(o => Save(), o => CanSave());`

## Integration points & external dependencies

- NuGet dependencies are declared in `gentech_services.csproj`. Inspect the .csproj for any database or third-party SDK references.
- Data flow: Models -> ViewModels -> Views. Persist/IO likely implemented in `Services/` (check implementations). If adding a new persistence mechanism, add a service interface in `Services/Interfaces` and register/consume it from ViewModels.

## Files to inspect when troubleshooting

- `MVVM/ViewModelBase.cs` — property-change pattern and intended ViewModel shape.
- `MVVM/RelayCommand.cs` — how commands are created and invoked.
- `Models/` — domain structure and sample fields (e.g., `Product.cs`).
- `App.xaml`, `App.xaml.cs`, `MainWindow.xaml(.cs)` — app entry and initial window wiring.

## Concrete prompts/examples for an AI code agent working here

- "Add a new ViewModel `InventoryViewModel` that exposes an ObservableCollection<Product> and a `LoadProducts` method which loads from `Services/IProductService` (create the interface if missing). Use `MVVM/ViewModelBase` and `RelayCommand` for refresh." 
- "Refactor `Models/Sale.cs` namespace to `gentech_services.Models` and update any references." 

## Notes / follow-up items (ask the maintainer)

- Confirm preferred namespace root (suggest `gentech_services.Models`).
- Indicate if you want viewmodel injection (IoC) patterns (none are present by default).
- Point to any hidden DB connection strings or external endpoints if they exist outside repo.

---
If anything here looks incorrect or you'd like a different focus (tests, CI, or service wiring examples), tell me which area to expand. I'm ready to iterate on this file.
