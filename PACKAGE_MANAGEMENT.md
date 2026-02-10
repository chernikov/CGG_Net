# Centralized Package Management

This solution uses `Directory.Build.props` for centralized version management of NuGet packages across all projects.

## How It Works

The [src/Directory.Build.props](src/Directory.Build.props) file defines:
- Common project properties (TargetFramework, Nullable, etc.)
- Package versions as MSBuild properties

## Adding New Packages

1. Add version to `Directory.Build.props`:
```xml
<PropertyGroup Label="Package Versions">
  <MyPackageVersion>1.0.0</MyPackageVersion>
</PropertyGroup>
```

2. Reference in project file without version:
```xml
<ItemGroup>
  <PackageReference Include="MyPackage" Version="$(MyPackageVersion)" />
</ItemGroup>
```

## Updating Package Versions

Simply update the version in `Directory.Build.props` and run `dotnet restore`:
```powershell
# Edit src/Directory.Build.props
<NewtonsonJsonVersion>13.0.3</NewtonsonJsonVersion>

# Restore all projects
dotnet restore
```

## Current Package Versions

### Microsoft Packages
- ASP.NET Core & Extensions: `10.0.2`
- Entity Framework Core: `10.0.2`
- Identity Stores: `9.0.1`
- OpenApi: `2.5.0`

### Third Party
- AutoMapper: `12.0.1`
- FluentValidation: `12.1.1`
- Swashbuckle: `10.1.1`
- MailKit: `4.14.1`
- MimeKit: `4.14.0`

## Benefits

✅ Single source of truth for package versions  
✅ Prevents version conflicts between projects  
✅ Easier dependency updates  
✅ Consistent configuration across solution  
✅ Cleaner project files  

## Documentation

- [MSBuild Directory.Build.props](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory)
- [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
