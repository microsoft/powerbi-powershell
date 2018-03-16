# Contribute Code to PowerShell Cmdlets for PowerBI

## Developer Environment Requirements

* [Visual Studio 2017](https://www.visualstudio.com/thank-you-downloading-visual-studio/)
* [.NET Core SDK](https://www.microsoft.com/net/learn/get-started/windows)
    * Version driven by [global.json](src/Common/Commands.Common/global.json)

### Optional requirements (for testing)
* [PowerShell Core (6.0.0)](https://github.com/powershell/powershell)
* Windows PowerShell v3 and up with .NET 4.6.1 or above

## Coding Style

Driven and enforced by [.editorconfig](src/.editorconfig). You can learn more about this config file by reading Visual Studio [documentation](https://docs.microsoft.com/en-us/visualstudio/ide/create-portable-custom-editor-options).

Optionally you can install the [EditorConfig](https://marketplace.visualstudio.com/items?itemName=EditorConfigTeam.EditorConfig) Visual Studio extension to give you intellisense editing the file.

## Common Developer Scenarios

### Build and Packaging

#### Command line

1. Open PowerShell prompt (replace your path with your VS edition and install directory):
```powershell
cd .\src
& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" .\PowerBIPowerShell.sln

# Packaging
& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" .\PowerBIPowerShell.sln /t:Build,Pack
```

> Note: dotnet build or dotnet msbuild does not work due to this [issue](https://github.com/Microsoft/msbuild/issues/2111), using this [workaround](https://github.com/RazorGenerator/RazorGenerator/issues/159). 
> The only impacted build task is from GitVersion, you will see errors like `Could not load file or assembly Microsoft.Build.Utilities.v4.0`.

#### Visual Studio 2017

1. Open [src/PowerBIPowerShell.sln](src/PowerBIPowerShell.sln).
2. Press `F6` or click `Build` menu and select `Build Solution`.

Binary output will be under each project's `bin\$(Configuration)\$(TargetFramework)` directory (TargetFramework is typically `netstandard2.0` unless its a test project then it's `netcoreapp2.0`)

For Packaging, right-click on a module project and select `Pack`. Output is in `bin\$(Configuration)\*.nupkg`.
To test module, rename extension from NuGet package `.nupkg` to Zip `.zip` and extract archive. Load the PSD1 file in the extracted directory by using `Import-Module -Path`.

To execute tests, just open `Test Explorer` and run tests from the explorer. Solution supports Live Unit Testing, there are interactive tests in the suite that are auto excluded from running with this feature. 

### Creating a new Cmdlet and Unit Test

1. Create a new class in a Module project and extend `PowerBICmdlet` from `Microsoft.PowerBI.Commands.Common` project and implement class.
2. Add `[Cmdlet(CmdletVerb, CmdletName)]` attribute to class and provide `public const string` for `CmdletVerb` and `CmdletName`.
    * For `CmdletVerb`, pick from the System.Management.Automation.Verbs* classes.
3. Optionally add `[OutputType(typeof(type))]` if your Cmdlet class if it writes an object to the output stream.
4. Optionally add `[Alias("Get-Alias1", "Get-Alias2")]` to your Cmdlet class. 
5. Add a test class to verify your cmdlet by adding (replacing with your new class name):
```csharp
using (var ps = System.Management.Automation.PowerShell.Create())
{
    ps.AddCommand(new CmdletInfo($"{<class name>.CmdletVerb}-{<class name>.CmdletName}", typeof(<class name>))); // Optionally .AddParameter(), use intellisense to see options
    var result = ps.Invoke();
    // Add asserts to verify
}
```
6. If your test is interactive, add method attributes `[TestCategory("Interactive")]` and `[TestCategory("SkipWhenLiveUnitTesting")]` (last tells Live Unit Testing to skip it).

### Creating a new PowerShell module

1. Open PowerShell and set your current directory to the root of this repo.
2. In PowerShell, execute the following by supplying your module name:
```powershell
$moduleName = '<fill out with module name>'
$root = "$pwd"
cd .\src\Modules
mkdir $moduleName
cd ".\$moduleName"
dotnet new classlib --name "Commands.$moduleName" --framework netstandard2.0
dotnet new mstest --name "Commands.$moduleName.Test"

cd ".\Commands.$moduleName"
dotnet add package PowerShellStandard.Library --version 3.0.0-preview-01
dotnet add package Microsoft.PowerBI.Api --version 2.0.10
dotnet add reference "$root\src\Common\Commands.Common\Commands.Common.csproj"
dotnet add reference "$root\src\Common\Common.Abstractions\Common.Abstractions.csproj"
dotnet add reference "$root\src\Modules\Profile\Commands.Profile\Commands.Profile.csproj"
Remove-Item .\Class1.cs
New-ModuleManifest -Path ".\MicrosoftPowerBIMgmt.$moduleName.psd1" `
    -Author 'Microsoft Corporation' `
    -CompanyName 'Microsoft Corporation' `
    -Copyright 'Microsoft Corporation. All rights reserved.' `
    -RootModule "Microsoft.PowerBI.Commands.$moduleName.dll" `
    -ModuleVersion '1.0.0' `
    -Description "Microsoft PowerBI PowerShell - $moduleName cmdlets for Microsoft PowerBI" `
    -PowerShellVersion '3.0' `
    -PrivateData @{
         PSData=@{
             Prerelease='-beta1'
             Tags=@('PowerBI', $moduleName)
             ProjectUri='https://github.com/Microsoft/powerbi-powershell'
         }
     }

```
3. Edit CSProj file in the current directory and add (update with your module name):
```xml
<PropertyGroup>
    <AssemblyName>Microsoft.PowerBI.Commands.<fill out with module name></AssemblyName>
    <RootNamespace>Microsoft.PowerBI.Commands.<fill out with module name></RootNamespace>
    <ReferenceWindowsAuthenticator>true</ReferenceWindowsAuthenticator>
</PropertyGroup>

<!-- NuGet Package Properties -->
<PropertyGroup>
    <IsPackable>true</IsPackable>
    <PackageId>MicrosoftPowerBIMgmt.<fill out with module name></PackageId>
    <Description>Microsoft PowerBI PowerShell - <fill out with module name> cmdlets for Microsoft PowerBI</Description>
    <PackageTags>PowerBI;<fill out with module name></PackageTags>
</PropertyGroup>
```
4. In the same CSProj file, for any packages add `<PrivateAssets>All</PrivateAssets>` inside `<PackageReference></PackageReference>` as this will prevent those packages becoming dependencies for the module.
5. In the same CSProj file, for any project references outside of the Modules folder (such as Commands.Common and Commands.Abstractions, but don't do it for Commands.Profile), add    `<PrivateAssets>All</PrivateAssets>` inside `<ProjectReference></ProjectReference>`.
6. Save the CSProj file and close.
7. Execute in PowerShell (same console window as before):
```powershell
cd "..\Commands.$moduleName.Test"
dotnet add package PowerShellStandard.Library --version 3.0.0-preview-01
dotnet add package Microsoft.PowerBI.Api --version 2.0.10
dotnet add package Microsoft.PowerShell.SDK --version 6.0.1
dotnet add package Microsoft.PowerShell.Commands.Diagnostics --version 6.0.1
dotnet add package Microsoft.WSMan.Management --version 6.0.1
dotnet add reference "$root\src\Common\Commands.Common\Commands.Common.csproj"
dotnet add reference "$root\src\Common\Common.Abstractions\Common.Abstractions.csproj"
dotnet add reference "$root\src\Common\Common.Authentication\Common.Authentication.csproj"
dotnet add reference "$root\src\Modules\Profile\Commands.Profile\Commands.Profile.csproj"
dotnet add reference "$root\src\Modules\$moduleName\Commands.$moduleName\Commands.$moduleName.csproj"
```
8. Edit CSProj file in the current directory and add (update with your module name):
```xml
<PropertyGroup>
    <AssemblyName>Microsoft.PowerBI.Commands.<fill out with module name>.Test</AssemblyName>
    <RootNamespace>Microsoft.PowerBI.Commands.<fill out with module name>.Test</RootNamespace>
    <ReferenceWindowsAuthenticator>true</ReferenceWindowsAuthenticator>
</PropertyGroup>
```
9. Execute in PowerShell (same console window as before):
```powershell
cd $root
cd .\src
dotnet sln add "$root\src\Modules\$moduleName\Commands.$moduleName\Commands.$moduleName.csproj"
dotnet sln add "$root\src\Modules\$moduleName\Commands.$moduleName.Test\Commands.$moduleName.Test.csproj"

ii .\PowerBIPowerShell.sln
```
10. Visual Studio should open for you, add a new class to your module project.
11. Make the class public and extend PowerBICmdlet (`Ctrl+.` to add using statement and implement)
12. Decorate class `[Cmdlet(CmdletVerb, CmdletName)]` and define `CmdletVerb` and `CmdletName` as `public const string` (for verb use the System.Management.Automation.Verbs* classes to pick an approved verb, you can see in the list in PowerShell by doing Get-Verb).
13. Optionally add `[OutputType(typeof(type))]` to your class if you cmdlet returns a certain output.
14. Click on psd1 file in the module project, change Build Action to `Content`, edit csproj and add the following to the psd1 file `<Content>` element:
```xml
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
```

If the content extension is not `.psd1` or `.types.ps1xml` then add the following to the `Content` element too:
```xml
    <Pack>true</Pack>
    <PackagePath></PackagePath>
```
15. Save the project file and build the solution.
16. A `help` folder should appear under your project after you build, the files contain the documenation for your module and cmdlets which you can fill out.

## Develolper Resources

### MSBuild and NuGet

* https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets
* https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files