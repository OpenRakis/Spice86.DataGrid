# Spice86.DataGrid

`Spice86.DataGrid` is an internal fork of the last FLOSS `Avalonia.Controls.DataGrid` v11 codebase, updated to work with Avalonia v12 and packaged as a single NuGet artifact.

It is intended for projects that need:

- the classic `DataGrid` control API (`Avalonia.Controls.*` namespaces), and
- the Semi-inspired DataGrid visuals used by Spice86,
- without splitting control code and styling resources across separate packages.

## Why this repository exists

Spice86 needs a stable DataGrid implementation that can evolve independently from upstream release timing. This repository provides that by:

- vendoring the DataGrid control implementation,
- adapting it for Avalonia 12,
- keeping package metadata and release automation in one place,
- shipping both behavior and theme resources together.

## What the NuGet package contains

The `Spice86.DataGrid` package includes:

- DataGrid controls and related types in `Avalonia.Controls`, `Avalonia.Controls.Primitives`, `Avalonia.Collections`, and related namespaces,
- DataGrid control templates and theme resources under:
  - `Themes/Fluent.xaml`
  - `Themes/Simple.xaml`
- Semi DataGrid resources:
  - `DataGridSemiTheme.axaml`
  - `DataGrid.axaml`
  - `Shared.axaml`
  - `Light.axaml`
  - `Dark.axaml`

The package readme bundled in NuGet is:

- `/home/runner/work/Spice86.DataGrid/Spice86.DataGrid/src/Spice86.DataGrid/Avalonia.Controls.DataGrid/README.md`

## Target framework and dependencies

Current project settings (from `Directory.Build.props` and `Directory.Packages.props`):

- Target framework: `net10.0`
- Avalonia dependency: `12.0.2`
- Nullable reference types: enabled

## Installation

Add the package to your Avalonia application:

```bash
dotnet add package Spice86.DataGrid
```

## Basic usage

### 1. Register DataGrid styles

Include one of the packaged DataGrid style dictionaries in your app styles.

```xml
<Application.Styles>
  <!-- Choose one base style set -->
  <StyleInclude Source="avares://Spice86.DataGrid/Themes/Fluent.xaml" />
  <!-- or -->
  <!-- <StyleInclude Source="avares://Spice86.DataGrid/Themes/Simple.xaml" /> -->
</Application.Styles>
```

### 2. (Optional) Add the Semi DataGrid theme

If you want the Semi-flavored resource setup, include `DataGridSemiTheme`:

```xml
<Application.Styles>
  <spice86:DataGridSemiTheme xmlns:spice86="clr-namespace:Spice86.DataGrid;assembly=Spice86.DataGrid" />
</Application.Styles>
```

### 3. Use `DataGrid` in views

```xml
<DataGrid ItemsSource="{Binding Rows}" AutoGenerateColumns="True" />
```

Or define columns explicitly:

```xml
<DataGrid ItemsSource="{Binding Rows}" AutoGenerateColumns="False">
  <DataGrid.Columns>
    <DataGridTextColumn Header="Name" Binding="{Binding Name}" />
    <DataGridTextColumn Header="Value" Binding="{Binding Value}" />
  </DataGrid.Columns>
</DataGrid>
```

## Repository layout

- `/home/runner/work/Spice86.DataGrid/Spice86.DataGrid/src/Spice86.DataGrid/Avalonia.Controls.DataGrid`  
  Main library project and all DataGrid sources/resources
- `/home/runner/work/Spice86.DataGrid/Spice86.DataGrid/.github/workflows`  
  CI, package publish, prerelease and CodeQL workflows

## Local development

From repository root:

```bash
dotnet build Spice86.DataGrid.slnx --configuration Release
```

This is the same baseline build command used by PR CI.

## CI and publishing overview

- PR workflow builds on macOS, Windows, and Ubuntu.
- `main` pushes trigger packaging/publishing automation.
- A prerelease workflow also publishes commit-hash-suffixed package versions to GitHub Packages.

See workflow files under `.github/workflows/` for current details.

## Compatibility notes

- This is a forked/internal package with Spice86-focused maintenance priorities.
- It tracks Avalonia compatibility as needed by Spice86, not necessarily upstream cadence.
- Public API shape is based on the vendored DataGrid sources in this repository.

## License

This repository is licensed under MIT. See `LICENSE`.
