`
# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.
``

## Commands

- Restore dependencies

```sh
 dotnet restore Lab678.sln
```

- Build (Debug/Release)

```sh
 dotnet build Lab678.sln -c Debug
 dotnet build Lab678.sln -c Release
```

- Run the Windows Forms app

```sh
 dotnet run --project Lab678.csproj -c Debug
```

- Clean

```sh
 dotnet clean Lab678.sln
```

- Format/lint (uses .NET analyzers and code style)

```sh
 dotnet format
```

- Tests

There are no test projects in this repo. If you add them (e.g., `*.Tests.csproj`), use:

```sh
 # run all tests
 dotnet test

 # run a single test by name
 dotnet test --filter "FullyQualifiedName~Namespace.ClassName.TestMethod"
```

## Architecture and structure (high level)

- Platform and target
  - .NET 9 Windows Forms desktop application (`TargetFramework: net9.0-windows`, `OutputType: WinExe`). Runs on Windows.

- Solution/project
  - Single Visual Studio solution `Lab678.sln` with one project `Lab678.csproj` (Windows Forms enabled via `<UseWindowsForms>true</UseWindowsForms>`). Configurations: Debug/Release (Any CPU).

- Layers and responsibilities (from README)
  - Models: domain entities (Client, PledgeItem, Loan, Payment) grouped via `PawnShopDatabase`.
  - Services: `DataService` handles persistence to a JSON file, loading/saving data and initializing seed data on first run.
  - UI: `Form1` is the main Windows Form hosting tabbed views with DataGridView tables for Clients, Pledge Items, Loans, and Payments.

- Data persistence
  - JSON file `pawnshop_data.json` is created on first launch and stores all application data in collections: Clients, PledgeItems, Loans, Payments.

- UX behaviors (from README)
  - Loans table uses color-coding: green (active), red (overdue), gray (repaid). Tables auto-size columns, alternate row colors, styled headers, full-row selection.

- Entry point
  - `Program.cs` starts the Windows Forms app and opens `Form1`.

- Notable implications
  - Because it is a WinForms app, execution launches a GUI; typical console output is minimal.
  - Ensure the working directory is writable so `pawnshop_data.json` can be created/updated.
