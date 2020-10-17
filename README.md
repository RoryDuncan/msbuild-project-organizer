# CSProjOrganizer

A console application for sorting `.csproj` files.
Has some configuration. See [Sort Options](#sort-options).

## Install

[You can install as a `dotnet tool` from NuGet](https://www.nuget.org/packages/organize-csproj)


<h2 id="sort-options">Sort Options</h2>

- `GroupByNodeType`
    -  Groups all `Item`s of the same type into their own `<ItemGroup>`

        (e.g., `<Content>`, `<Compile>`, `<TypeScriptCompile>`, etc)
-  `GroupByFileType`
    - Creates `<ItemGroup>`s for each distinct filetype
    - Subdivides existing `<ItemGroup>`s into even more granular `<ItemGroup>`s
- `RemoveEmptyItemGroups`
    - Removes `<ItemGroup>`s without any children. This application doesn't create empty `<ItemGroup>`s―this option is for tidying input.
- `SortItemsWithinItemGroups`
    - Whether to alphanum sort all `<ItemGroup>`s. Performed last.


## Usage

[See Usage](USAGE.md)

# Development

`
2. `dotnet build`

### Run Tests
Run `dotnet test`.

## Useful Links
- https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2019


# Example Changes

### Before
_Some messy XML_
``` xml
<Project>
  <ItemGroup>
    <Content Include="jquery.js" />
  </ItemGroup>
  <ItemGroup>
    <Content Include="Views/Index.cshtml" />
    <Content Include="croppie.js" />
    <Compile Include="DashboardController.cs" />
    <Content Include="Views/Dashboard/Index.cshtml" />
    <Compile Include="Bundling.cs" />
    <Content Include="Views/Home.cshtml" />
    <Content Include="jquery.validator.js" />
    <Compile Include="HomeController.cs" />
  </ItemGroup>
</Project>
```

### After

``` xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <!-- Compiled C# Files -->
  <ItemGroup Label=".cs files">
    <Compile Include="Bundling.cs" />
    <Compile Include="DashboardController.cs" />
    <Compile Include="HomeController.cs" />
  </ItemGroup>
  <!-- Published Files -->
  <ItemGroup Label=".cshtml files">
    <Content Include="Views/Dashboard/Index.cshtml" />
    <Content Include="Views/Home.cshtml" />
    <Content Include="Views/Index.cshtml" />
  </ItemGroup>
  <ItemGroup Label=".js files">
    <Content Include="croppie.js" />
    <Content Include="jquery.js" />
    <Content Include="jquery.validator.js" />
  </ItemGroup>
</Project>
```
