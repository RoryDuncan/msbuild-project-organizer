<style>
    table {
        width:100%;
    }
</style>

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
