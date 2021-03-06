# JSDelivr command line Interface

Install and consume 3rd-party client-side libraries from jsdelivr.

### Install

- Windows (PowerShell Admin)
  ```pwsh
  $InstallPath = 'C:\Program Files\JSDelivrCLI\'
  if ($false -eq $(Test-Path $InstallPath)) { mkdir $InstallPath }
  iwr https://github.com/lixinyang123/JSDelivrCLI/releases/download/v1.0.0-release.1/delivr-win-x64.exe -OutFile $($InstallPath + 'delivr.exe')
  $Target = 'User'
  $Path = [Environment]::GetEnvironmentVariable('Path', $Target)
  $NewPath = $Path + ';' + $InstallPath
  [Environment]::SetEnvironmentVariable('Path', $NewPath, $Target)
  ```

- Linux (Bash)
  ```bash
  sudo curl -L https://github.com/lixinyang123/JSDelivrCLI/releases/download/v1.0.0-release.1/delivr-linux-x64 -o /usr/bin/delivr
  sudo chmod +x /usr/bin/delivr
  ```

- OSX (Bash)
  ```bash
  curl -L https://github.com/lixinyang123/JSDelivrCLI/releases/download/v1.0.0-release.1/delivr-osx-x64 -o /usr/local/bin/delivr
  chmod +x /usr/local/bin/delivr
  ```

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
