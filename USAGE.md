# Usage

[Install from NuGet as a global dotnet tool.](https://www.nuget.org/packages/organize-csproj)

Then, use it on a project file:
```
organize-csproj --input=your-project.csproj
```

Alternatively, you can use it on a solution file to organize all Projects associated with the solution. 
Simply run the below line in the directory where a `.sln` file exists.
```
organize-csproj --scan
```

## CLI Arguments

Use `--help` to see command line arguments for the version you're using.

### On a Solution File

You can use a solution file to organize all associated project files, you only need to use the `--scan` flag:
```
--scan
```

### Individual Project files
```
--input
```
**Description:** The path to a `.csproj` file.

**Default:** None

**Required:** Yes

```
--output
```
**Description:** The filename to save the input`.csproj` after it has been sorted.

**Default:** The value of `--input` (overwrites existing file)

**Required:** No


```
--config
```
**Description:** The path to a configuration json file

**Default:** None

**Required:** No
---

### Example
```
dotnet organize-csproj --input=project.csproj --output=project.sorted.csproj --config=my.config.json
```


## Configuration

The default configuration can be seen here: [csproj.config.defaults.json](MSBuildProjectOrganizer/csproj.config.defaults.json)
It should work as a solid reference to get yourself started.
The configuration needs to map to [SortConfiguration.cs](MSBuildProjectOrganizer/Models/SortConfiguration.cs).

If this isn't sufficient documentation, open an issue and I'll write out more.

## Sort Options

Below are the primary steps taken when cleaning up a `.csproj` file. 

They each can be disabled using your own configuration file, like so:
``` json
{
  "SortOptions": {
    "GroupByNodeType": true,
    "GroupByFileType": true,
    "RemoveEmptyItemGroups": true,
    "SortItemsWithinItemGroups": true
  }
}
```

They all default to `true`.

- `GroupByNodeType`
    -  Groups all `Item`s of the same type into their own `<ItemGroup>`

        (e.g., `<Content>`, `<Compile>`, `<TypeScriptCompile>`, etc)
-  `GroupByFileType`
    - Creates `<ItemGroup>`s for each distinct filetype
    - Subdivides existing `<ItemGroup>`s into even more granular `<ItemGroup>`s
- `RemoveEmptyItemGroups`
    - Removes `<ItemGroup>`s without any children. The tool doesn't create empty `<ItemGroup>`s―this option is for tidying up.
- `SortItemsWithinItemGroups`
    - Whether to alphanum sort all `<ItemGroup>`s. Performed last.


