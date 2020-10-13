# CSProjOrganizer

A console application for sorting `.csproj` files.
Has some configuration. See [Sort Options](#sort-options).

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

```
CSProjOrganizer.exe --input=project.csproj
```

_Todo: Add more commandline flags for controlling SortOptions_.

# Development

For tidyness, [dotnet format](https://github.com/dotnet/format) is performed before all builds.
1. Install [dotnet format](https://github.com/dotnet/format#how-to-install): `dotnet tool install -g dotnet-format
`
2. `dotnet build`

### Run Tests
Run `dotnet test`.

## TODO:
- Separate Configuration from AppSettings
- More Unit Tests
- Action Suggestionss

## Useful Links
- https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2019