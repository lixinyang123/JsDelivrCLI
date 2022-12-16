# JsDelivr command line Interface

Install and consume 3rd-party client-side libraries from jsdelivr.

### Install

Download from Release page.

### Usage

```pwsh
delivr
  JsDelivr CLI

Usage:
  delivr [options] [command]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  init               Initialize a package configuration file
  install <library>  install a package from jsdelivr
  remove <library>   remove a package from local
  search <library>   search package from npm
  info <library>     get library version info
  restore            restore client side package
```

Install a library

```pwsh
install
  install a package from jsdelivr

Usage:
  delivr [options] install <library>

Arguments:
  <library>  library name and version

Options:
  --version <version>  library version
  --dir <dir>          library install directory
  -?, -h, --help       Show help and usage information
```

Example:

```pwsh
# install jquery
delivr install jquery

# install jquery v3.6.0
delivr install jquery --version 3.6.0

# install jquery v3.6.0 in directory lib
delivr install jquery --version 3.6.0 --dir lib
```

Config library configuration file

```json
{
  "Libraries": [
    {
      "Name": "jquery",
      "Version": "3.6.0",
      "Destination": "lib"
    },
    {
      "Name": "bootstrap",
      "Version": "5.0.2",
      "Destination": "lib"
    }
  ]
}
```

Restore dependences

```pwsh
delivr restore
```


### How to Contribute?

Install pre-requisites

- Windows
  - Visual Studio 2022, including `.NET workload` and `Desktop development with C++ workload`.

- Linux
  - .NET 7 SDK
  - libicu-dev
  - cmake

##### On Windows

- Visit [official website](https://visualstudio.microsoft.com/) to download Visual Studio 2022.

##### On Linux

- Install .NET SDK
    - Ubuntu
    ```bash
    apt install dotnet-sdk-7.0 -y
    apt install libicu-dev cmake -y
    ```

    - CentOS
    ```bash
    dnf install dotnet-sdk-7.0 -y
    dnf install libicu-dev cmake -y
    ```

### Restore dependencies

```bash
dotnet restore
```

### Build & Run & Publish

```bash
dotnet build
dotnet run
```

### Publish with NativeAOT

```bash
dotnet publish -r win-x64 -c Release
dotnet publish -r linux-x64 -c Release
dotnet publish -r osx-x64 -c Release
```
