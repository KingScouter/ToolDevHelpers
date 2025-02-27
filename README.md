# PowerToys Run Tool-Dev-Helpers Plugin

> A simple plugin to ease developing (web) tools

The plugin is developed and tested with `PowerToys` `v0.87.0`.

## Installation

0. [Install PowerToys](https://docs.microsoft.com/en-us/windows/powertoys/install)
1. Exit PowerToys
2. Download the `.zip` file applicable for your platform from the releases:
   - [ToolDevHelpers_x64.zip](ToolDevHelpers_x64.zip)
3. Extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\ToolDevHelpers`
4. Start PowerToys

Alternatively you can use the provided Powershell-Script to install the Plugin:

1. Download the `.zip` file applicable for your platform from the releases:
   - [ToolDevHelpers_x64.zip](ToolDevHelpers_x64.zip)
2. Extract it
3. Execute the install-script using the following command:
   - `powershell.exe -ExecutionPolicy Bypass .\install_plugin.ps1`

## Usage

### Available options

1. Open PowerToys Run with `alt + space`
2. Type `tdh`
3. All available commands will be listed

### Branch Helpers

1. Open PowerToys Run with `alt + space`
2. Type `tdh bl` (local branches) or `tdh br` (remote branches)
3. Type the name of a branch
4. All branches with matching names will be listed below
5. Use ⬆️ and ⬇️ keys to select a result
6. Select the desired operation from the context-menu buttons
   - Press `Enter` to open the branch on Jenkins
   - Press `Ctrl+Enter` to open the branch on Github
   - Press `Shift+Enter` to trigger the download of the build-artifacts for that tool

### Tool Helpers

1. Open PowerToys Run with `alt + space`
2. Type `tdh t`
3. Type the name of a tool
4. All tools with matching short-names will be listed below
5. Use ⬆️ and ⬇️ keys to select a result
6. Select the desired operation from the context-menu buttons
   - Press `Enter` to start the tool locally
   - Press `Ctrl+Enter` to open the locally started tool in the browser
   - Press `Shift+Enter` to open the tool on the remote-server in the browser

### Misc Options

#### Reload

Execute `tdh reload` to reload the tool configuration from the configured project file.

## Settings

Open settings:

1. Open PowerToys Settings
2. Select PowerToys Run
3. Scroll down to Plugins
4. Expand `Tool Dev Helpers`

### Settings available:

- <strong>Git Repository URL:</strong><br>
  URL of the GIT repository to use for the branch-helpers (remote branches)
- <strong>Local source folder:</strong><br>
  Path to a locally cloned source folder (for local branches)
- <strong>Jenkins Multibranch-Pipeline URL:</strong><br>
  URL to the base Jenkins-Multibranch-Pipeline to open the branches for
- <strong>Tool folder:</strong><br>
  Folder where the tools get downloaded to
- <strong>Download Script:</strong><br>
  Powershell-Script to download the tools with (accepts the branch-name as parameter)
- <strong>Tool config file:</strong><br>
  Configuration of all tools available for the Tool-Helpers. If the file doesn't exist yet, a sample project will be created at that location that can be adjusted.
- <strong>Powershell Version</strong><br>
  The version of Powershell to use when executing scripts (e.g. to download tools).<br>
  Available options are:
  - Legacy (powershell)
  - Powershell 7 (pwsh)

## Tool config

In the tool config file you can configure all tools you want to work with.

> [!TIP]
> To start of you can set the `Tool config file` setting to a non-existing file. This will create a sample project which you can then adapt.

For every tool you have the following settings available:

- `shortName`<br>
  A short abbreviation for the tool to quickly select it from the list
- `name`<br>
  The name of the tool (will be displayed in the list and in the shell window title)
- `useHttps`<br>
  True if the tool uses secure HTTPS (false or empty for HTTP) (optional)
- `removeServerUrl`<br>
  URL of the server where the tool is also deployed (optional)
- `port`<br>
  Port of the tool (optional)
- `exePath`<br>
  Relative path to the file that starts up the app (e.g. an executable)
- `additionalPages`<br>
  List of additional pages that will be opened alongside the tool itself (optional). This can be used to e.g. open a separate subpage for the API or an admin-panel. You can use one or more placeholders for various values:
  - `#BASE#`: Will be replaced with the fully formed tool URL (e.g. `https://production.local:8080/`) as a placeholder, which will be replaced with the URL of the tool.
  - `#BASE_HOST#`: Will be replaced with the hostname of the tool (depending on if opened locally or remotely)
  - `#BASE_PORT#`: Will be replaced with the configured tool port

### Example config

```json
{
  "toolConfigs": {
    "ta": {
      "shortName": "ta",
      "name": "Test app",
      "useHttps": true,
      "remoteServerUrl": "production.local",
      "port": 8080,
      "exePath": "testApp/start.exe",
      "additionalPages": ["#BASE#api", "#BASE_HOST#:#BASE_PORT#/test"]
    }
  }
}
```
