# Usage

Install as a global dotnet tool.
```
dotnet tool install -g organize-csproj
```

Then, use it via:
```
dotnet organize-csproj --input=your-project.csproj
```

## Parameters


| Parameter | Description | Default | Required |
| --------- | ----------- | ------- | -------- |
| `--input` | The path to a `.csproj` file | None | Yes |
| `--output`| The filename to save the input`.csproj` after it has been sorted. | `--input`'s value | No |
| `--config` | The path to a configuration json file | None | No |


## Configuration

The default configuration can be seen here: [csproj.config.defaults.json](CSProjOrganizer/csproj.config.defaults.json)
It should work as a solid reference to get yourself started.
The configuration needs to map to [SortConfiguration.cs](CSProjOrganizer/Models/SortConfiguration.cs).

If this isn't sufficient documentation, open an issue and I'll write out more.