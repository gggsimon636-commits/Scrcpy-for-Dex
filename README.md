# Scrcpy Virtual Display

A simple Windows application for using [scrcpy](https://github.com/Genymobile/scrcpy) with Android devices.

The application provides a modern Windows Forms interface for configuring and launching scrcpy. It also supports a separate **External Display mode**, which can be useful for **Samsung DeX** and other use cases that require a virtual Android display.

## ✨ Features

* 🪞 **Mirror Mode**

  * Mirrors the normal Android screen in a scrcpy window.

* 🖥️ **External Display Mode**

  * Creates a separate Android display.
  * Especially useful for **Samsung DeX** and similar use cases.

* ⚙️ **Setup Wizard**

  * Easy first-time configuration.
  * Select your scrcpy installation.
  * Configure resolution and DPI.
  * Enable or disable audio.
  * Enable fullscreen mode.
  * Optionally specify a device ID.

* 🔄 **Choose Mode on Startup**

  * Optionally ask whether to use Mirror or External mode every time the application starts.

* 💾 **Automatic Settings**

  * Settings are stored locally and loaded automatically when the application starts.

* 🧹 **ResetApp**

  * A separate `ResetApp.exe`.
  * Resets the saved application settings.
  * The Setup Wizard will appear again on the next launch.

* 🌑 **Modern Dark UI**

  * Dark interface.
  * Windows 11-inspired design.
  * Rounded buttons and panels.

## 🗺️ Roadmap

The current version is available for **Windows**.

Planned platforms:

* 🐧 **Linux**
* 🍎 **macOS**

The goal is to make **Scrcpy Virtual Display** available across Windows, Linux, and macOS.

More features and platform support may be added in future releases.

## 📋 Requirements

* Windows 10 or Windows 11
* x64 system
* Android device with **USB debugging** enabled
* [scrcpy](https://github.com/Genymobile/scrcpy) **is included in the ZIP**
* USB connection or a properly configured ADB connection

## 🚀 Installation

1. Download the latest release.
2. Extract the files to a folder of your choice.
3. Make sure `scrcpy.exe` is available.
4. Start:

```text
ScrcpyLauncher.exe
```

5. The Setup Wizard will automatically open on the first launch.
6. Select the location of your `scrcpy.exe`.
7. Configure your preferred settings.
8. Done!

## 📱 Preparing Your Android Device

To allow scrcpy to connect to your device:

1. Open the Android settings.
2. Enable **Developer Options**.
3. Enable **USB Debugging**.
4. Connect your Android device to your PC.
5. Accept the USB debugging authorization prompt if it appears.

You can then start the device through `ScrcpyLauncher.exe`.

## 🪞 Mirror Mode

Mirror Mode displays the normal Android screen in a scrcpy window.

No additional display is created.

```text
Android Device
      │
      └──► scrcpy window on Windows
```

## 🖥️ External Display Mode

External Display Mode uses scrcpy's virtual display functionality:

```text
Android Device
      │
      └──► New virtual display
                 │
                 └──► scrcpy
```

For example, the following scrcpy option may be used:

```text
--new-display=1920x1080/160
```

The resolution and DPI can be configured through the Setup Wizard.

This mode is particularly useful for **Samsung DeX** and other applications that can make use of a separate Android display.

## ⚙️ Settings

Settings are stored locally at:

```text
%AppData%\ScrcpyVirtualDisplay\settings.json
```

The following settings are stored:

* scrcpy path
* display mode
* resolution
* DPI
* audio
* fullscreen mode
* device ID
* startup mode selection

## 🧹 Reset Settings

To completely reset the application configuration, run:

```text
ResetApp.exe
```

The application removes:

```text
%AppData%\ScrcpyVirtualDisplay\settings.json
```

The next time `ScrcpyLauncher.exe` is started, the Setup Wizard will appear again.

**Note:** `ResetApp.exe` does not remove your scrcpy installation or any personal data on your Android device.

## 📁 Project Structure

```text
ScrcpyVirtualDisplay/
│
├── Program.cs
├── Settings.cs
├── SetupForm.cs
├── ModeSelectionForm.cs
├── ScrcpyLauncher.csproj
│
└── ResetApp/
    ├── Program.cs
    └── ResetApp.csproj
```

## 🔨 Building

The project currently uses:

* C#
* .NET 8
* Windows Forms
* Windows x64

### Main Application

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### ResetApp

```powershell
dotnet publish .\ResetApp\ResetApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The applications are published as **self-contained executables**, so a separate .NET installation is not required on the target system.

## 📦 Release

A release package should contain at least:

```text
Scrcpy Virtual Display/
│
├── ScrcpyLauncher.exe
├── ResetApp.exe
└── README.md
```

The scrcpy installation itself may be included separately depending on the distribution.

## 🛠️ Troubleshooting

### `scrcpy.exe` was not found

Open the Setup Wizard and select the correct path to `scrcpy.exe`.

### Android device is not detected

Check:

* USB cable
* USB Debugging
* ADB drivers
* USB debugging authorization on the Android device

You can also check whether ADB detects your device:

```powershell
adb devices
```

### The Setup Wizard does not appear again

Run:

```text
ResetApp.exe
```

This removes the saved configuration. The Setup Wizard will then appear the next time you launch the application.

## 🔐 Privacy

The application does not require a cloud connection and stores its configuration locally on the Windows PC.

The application itself does not collect personal data.

Communication with the Android device is handled through **ADB/scrcpy**.

## 📄 License

This project is independent of scrcpy.

`scrcpy` is developed by **Genymobile**.

For more information and the scrcpy license, visit the official repository:

https://github.com/Genymobile/scrcpy

## ❤️ Credits

* **scrcpy** – Genymobile
* **Scrcpy Virtual Display** – Launcher and configuration application

---

**Scrcpy Virtual Display**

A simple way to use scrcpy on Windows with convenient Mirror and External Display modes, with **Linux and macOS support planned for future releases**.
