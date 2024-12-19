# PowerToys Run Tool-Dev-Helpers Plugin

> A simple plugin to ease developing (web) tools

The plugin is developed and tested with `PowerToys` `v0.87.0`.

## Installation

0. [Install PowerToys](https://docs.microsoft.com/en-us/windows/powertoys/install)
1. Exit PowerToys
2. Download the `.zip` file applicable for your platform from the lastest run of the Github-Action "Create Plugin Build":
   - [tool-dev-helpers-plugin.zip](tool-dev-helpers-plugin.zip)
3. Extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\ToolDevHelpers`
4. Start PowerToys

## Usage

### Branch Helpers

1. Open PowerToys Run with `alt + space`
2. Type `tdh b`
3. Type the name of a branch
4. All branches with matching names will be listed below
5. Use ⬆️ and ⬇️ keys to select a result
6. Select the desired operation from the context-menu buttons
6.1 Press `Enter` to open the branch on Jenkins
6.2 Press `Shift+Enter` to trigger the download of the build-artifacts for that tool

### Tool Helpers

1. Open PowerToys Run with `alt + space`
2. Type `tdh t`
3. Type the name of a tool
4. All tools with matching short-names will be listed below
5. Use ⬆️ and ⬇️ keys to select a result
6. Select the desired operation from the context-menu buttons
6.1 Press `Enter` to start the tool locally
6.2 Press `Shift+Enter` to open the locally started tool in the browser
6.3 Press `Ctr+Enter` to open the tool on the remote-server in the browser

## Settings

Open settings:

1. Open PowerToys Settings
2. Select PowerToys Run
3. Scroll down to Plugins
4. Expand `Tool Dev Helpers`

### Settings available:

- Git Repository URL: URL of the GIT repository to use for the branch-helpers
- Jenkins Multibranch-Pipeline URL: URL to the base Jenkins-Multibranch-Pipeline to open the branches for
- Remoteserver URL: URL of the remote-server where the tools are deployed and available
- Tool folder: Folder where the tools get downloaded to
- Download Script: Powershell-Script to download the tools with (accepts the branch-name as parameter)
- Tool config file: Configuration of all tools available for the Tool-Helpers. If the file doesn't exist yet, a sample project will be created at that location that can be adjusted.
