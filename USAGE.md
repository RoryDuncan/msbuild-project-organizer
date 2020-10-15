# Usage

Install as a global dotnet tool.
```
dotnet tool install -g organize-csproj
```

Then, use it via:
```
dotnet organize-csproj --input=your-project.csproj
```

## CLI Arguments

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

The default configuration can be seen here: [csproj.config.defaults.json](CSProjOrganizer/csproj.config.defaults.json)
It should work as a solid reference to get yourself started.
The configuration needs to map to [SortConfiguration.cs](CSProjOrganizer/Models/SortConfiguration.cs).

If this isn't sufficient documentation, open an issue and I'll write out more.