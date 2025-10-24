# AutoSaver

A Unity Editor plugin that automatically saves your scenes at regular intervals, helping prevent data loss during development.

## Features

- **Automatic Scene Saving**: Saves your current scene every 5 minutes (configurable)
- **Manual Save**: One-click manual save with timestamp
- **Customizable Settings**: 
  - Adjustable save interval
  - Custom save path
  - Maximum number of saved files
- **Automatic Cleanup**: Removes old save files to prevent clutter
- **Multi-language Support**: English and Chinese interface
- **Editor-Only**: No GameObjects required, runs entirely in the background

## Installation

1. Add this package to your Unity project via VPM
2. The AutoSaver will automatically start working

## Usage

1. Open the AutoSaver control panel: `Tools > adez360 > AutoSaver`
2. Configure your settings:
   - **Save Interval**: How often to auto-save (in minutes)
   - **Save Path**: Where to save the files (default: `Assets/AutoSave/`)
   - **Max Save Files**: Maximum number of auto-saved files to keep
3. Toggle auto-save on/off as needed
4. Use "Manual Save" for important milestones

## How It Works

- AutoSaver runs in the background while you work in Unity Editor
- It automatically saves copies of your current scene to the specified directory
- Files are named with timestamps: `SceneName_Auto_20251024_034025.unity`
- Manual saves use: `SceneName_Manual_20251024_034025.unity`
- Old files are automatically deleted when the maximum count is reached

## Requirements

- Unity 2022.3 or later
- VRChat Package Manager (VPM)

## Author

Created by adez360

## License

This package is provided as-is for educational and development purposes.
