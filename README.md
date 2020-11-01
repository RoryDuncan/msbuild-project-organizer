# MSBuild Project Organizer
[![Unit Tests](https://github.com/RoryDuncan/msbuild-project-organizer/workflows/.NET%20Core/badge.svg)](https://github.com/RoryDuncan/msbuild-project-organizer/actions?query=workflow%3A%22.NET+Core%22)

A console application for organizing messy `.csproj` files.


## Install

[You can install as a `dotnet tool` from NuGet](https://www.nuget.org/packages/organize-csproj)

## Usage

[See Usage](USAGE.md)

# Development

Run `dotnet build src`

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

You can also see the dogfooding examples:
- [msbuild-project-organizer.csproj](src/msbuild-project-organizer/msbuild-project-organizer.csproj)
- [msbuild-project-organizer.tests.csproj](src/msbuild-project-organizer/msbuild-project-organizer.tests.csproj)
