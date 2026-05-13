# Spice86.DataGrid

`Spice86.DataGrid` is an Avalonia DataGrid package used by Spice86.

It combines:

- a vendored DataGrid control implementation based on the final FLOSS v11 lineage, and
- the Semi DataGrid theme resources,

inside one assembly/package for simpler consumption.

## Package summary

This package provides:

- `DataGrid` and related control types (`Avalonia.Controls`, `Avalonia.Controls.Primitives`, and related namespaces),
- DataGrid style resources in:
  - `Themes/Fluent.xaml`
  - `Themes/Simple.xaml`
- Semi DataGrid resources:
  - `DataGridSemiTheme.axaml`
  - `DataGrid.axaml`
  - `Shared.axaml`
  - `Light.axaml`
  - `Dark.axaml`

## Requirements

Current project configuration targets:

- `net10.0`
- Avalonia `12.0.2`

## Install

```bash
dotnet add package Spice86.DataGrid
```

## Setup

### Register a base DataGrid style dictionary

```xml
<Application.Styles>
  <StyleInclude Source="avares://Spice86.DataGrid/Themes/Fluent.xaml" />
  <!-- or -->
  <!-- <StyleInclude Source="avares://Spice86.DataGrid/Themes/Simple.xaml" /> -->
</Application.Styles>
```

### Optional: register the Semi DataGrid theme class

```xml
<Application.Styles>
  <spice86:DataGridSemiTheme xmlns:spice86="clr-namespace:Spice86.DataGrid;assembly=Spice86.DataGrid" />
</Application.Styles>
```

## Usage example

```xml
<DataGrid ItemsSource="{Binding Rows}" AutoGenerateColumns="False">
  <DataGrid.Columns>
    <DataGridTextColumn Header="Name" Binding="{Binding Name}" />
    <DataGridTextColumn Header="Value" Binding="{Binding Value}" />
  </DataGrid.Columns>
</DataGrid>
```

## Development / source

Source repository:

- https://github.com/OpenRakis/Spice86.DataGrid

Main library project path:

- `src/Spice86.DataGrid/Avalonia.Controls.DataGrid/Avalonia.Controls.DataGrid.csproj`

Build command:

```bash
dotnet build Spice86.DataGrid.slnx --configuration Release
```

## License

MIT. See repository `LICENSE`.
