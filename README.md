# JSDelivrCLI

Install and consume 3rd-party client-side libraries from jsdelivr.

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
