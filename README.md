# Azure API Management Policy C# Script Extractor

This project extracts C# scripts from Azure API Management policies to enable them to be coded in a more familiar environment, and to enable them to be tested and debugged.

Scripts are parsed from the policy XML and written to an output directory with the same name as the policy. The script is wrapped in a C# Script file which also provides an instance of the `context` object, togther with a related JSON file to populate the context for testing.

## Prerequisites

The extractor is a Node.js project, and as such requires Node.js to be installed.

This project also requires the following to be installed:
- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)
- [dotnet-script CLI tool](https://github.com/dotnet-script/dotnet-script)

## Usage

After cloning this repository and installing the above prerequisites, the following command should be run to install the required Node.js packages:

```bash
npm install
```

The extrator tool can then be built by running the following command:

```bash
npm run build
```

You are then ready to extract scripts from policy files. All API Management policy files should be extracted manually from the Azure portal, and placed in the `policies` directory as XML files.

The extractor can then be run from the command line with the following command:

```bash
npm run extract
```

This will extract all scripts from the policy files in the `policies` directory, and write them to the `scripts` directory.

Each policy file will be written to a subdirectory of the `scripts` directory, with the same name as the policy file. The script file will be named either `block-xxx.csx` or `inline-xxx.csx` depending upon whether it was a code block or an inline script.

The `context` object is defined in the file `context.csx` and is also copied into each policy script directory. This file is referenced from each script file and should not be removed. A context settings file, named `context.json`, is also copied into each policy script directory. This file can be edited to provide the context settings required for testing.

## Testing

With the scripts extracted, they can be tested and debugged using the `dotnet-script` CLI tool. This is configured in the `launch.json` file, and can be run from Visual Studio Code.

Each `*.csx` script file can be debugged individually, and the `context.json` file can be edited to provide common context settings required for testing.

To debug a script file, open the `*.csx` file in Visual Studio Code, and press `F5` to start debugging. The script will be run, and the output will be displayed in the Debug Console. Breakpoints can be set in the script file to debug the script.