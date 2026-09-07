# Scrcpy Virtual Display

Eine einfache Windows-App zur komfortablen Nutzung von [scrcpy](https://github.com/Genymobile/scrcpy) mit Android-Geräten.

Die App bietet eine moderne Oberfläche, mit der sich scrcpy konfigurieren und direkt starten lässt. Zusätzlich kann ein separater **External-Modus** für die Verwendung eines virtuellen Android-Displays, z. B. mit Samsung DeX, verwendet werden.

## ✨ Funktionen

* 🪞 **Mirror-Modus**

  * Spiegelt den normalen Android-Bildschirm über scrcpy.

* 🖥️ **External-Modus**

  * Erstellt ein separates Android-Display.
  * Besonders praktisch für **Samsung DeX** und ähnliche Anwendungsfälle.

* ⚙️ **Setup-Assistent**

  * Einfache Ersteinrichtung.
  * Auswahl der scrcpy-Installation.
  * Auflösung und DPI konfigurieren.
  * Audio aktivieren/deaktivieren.
  * Vollbildmodus konfigurieren.
  * Optional eine Geräte-ID festlegen.

* 🔄 **Modus beim Start auswählen**

  * Optional kann bei jedem Start zwischen Mirror und External gewählt werden.

* 💾 **Automatische Speicherung**

  * Einstellungen werden lokal gespeichert und beim nächsten Start wieder geladen.

* 🧹 **ResetApp**

  * Separate `ResetApp.exe`.
  * Setzt die gespeicherten Einstellungen zurück.
  * Beim nächsten Start wird der Setup-Assistent erneut angezeigt.

* 🌑 **Modernes Design**

  * Dunkle Oberfläche.
  * Windows-11-inspirierte Optik.
  * Abgerundete Buttons und Panels.

## 🗺️ Roadmap

Aktuell liegt der Fokus auf der **Windows-Version**.

Weitere Plattformen sind geplant:

* [x] 🪟 Windows
* [ ] 🐧 Linux
* [ ] 🍎 macOS

> **Linux- und macOS-Versionen sind geplant und werden zukünftig hinzugefügt.**

Das Ziel ist, Scrcpy Virtual Display langfristig auf allen drei großen Desktop-Plattformen verfügbar zu machen.

## 📋 Voraussetzungen

### Windows

* Windows 10 oder Windows 11
* x64-System
* Android-Gerät mit aktiviertem **USB-Debugging**
* [scrcpy](https://github.com/Genymobile/scrcpy)
* USB-Verbindung oder entsprechend konfigurierte ADB-Verbindung

### Linux & macOS

Linux- und macOS-Unterstützung befindet sich derzeit noch in Planung.

## 🚀 Installation

1. Lade die aktuelle Release-Version herunter.
2. Entpacke die Dateien.
3. Stelle sicher, dass `scrcpy.exe` vorhanden ist.
4. Starte:

```text
ScrcpyLauncher.exe
```

5. Beim ersten Start öffnet sich automatisch der Setup-Assistent.
6. Wähle den Speicherort deiner `scrcpy.exe`.
7. Konfiguriere die gewünschten Einstellungen.
8. Fertig.

## 📱 Android vorbereiten

Damit scrcpy das Gerät erkennen kann:

1. Öffne die Android-Einstellungen.
2. Aktiviere die **Entwickleroptionen**.
3. Aktiviere **USB-Debugging**.
4. Verbinde das Smartphone mit dem PC.
5. Bestätige gegebenenfalls die USB-Debugging-Abfrage.

Danach kann `ScrcpyLauncher.exe` das Gerät über scrcpy starten.

## 🖥️ Mirror-Modus

Im Mirror-Modus wird der normale Android-Bildschirm angezeigt.

```text
Android
   │
   └──► scrcpy-Fenster auf Windows
```

## 🖥️ External-Modus

Der External-Modus verwendet scrcpys virtuelle Display-Funktion:

```text
Android
   │
   └──► Neues virtuelles Display
             │
             └──► scrcpy
```

Beispiel:

```text
--new-display=1920x1080/160
```

Die Auflösung und DPI können im Setup konfiguriert werden.

Der Modus eignet sich insbesondere für **Samsung DeX**.

## ⚙️ Einstellungen

Die Einstellungen werden unter folgendem Windows-Pfad gespeichert:

```text
%AppData%\ScrcpyVirtualDisplay\settings.json
```

Gespeichert werden unter anderem:

* scrcpy-Pfad
* Display-Modus
* Auflösung
* DPI
* Audio
* Vollbild
* Geräte-ID
* Auswahlabfrage beim Start

## 🧹 Einstellungen zurücksetzen

Wenn du die Konfiguration komplett zurücksetzen möchtest, starte:

```text
ResetApp.exe
```

Die App löscht:

```text
%AppData%\ScrcpyVirtualDisplay\settings.json
```

Beim nächsten Start von `ScrcpyLauncher.exe` wird der Setup-Assistent erneut angezeigt.

**Hinweis:** `ResetApp.exe` löscht nicht deine scrcpy-Installation und keine persönlichen Android-Daten.

## 📁 Projektstruktur

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

## 🔨 Build

Das Projekt verwendet:

* C#
* .NET 8
* Windows Forms
* Windows x64

### Hauptprogramm

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### ResetApp

```powershell
dotnet publish .\ResetApp\ResetApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 📦 Release

Eine fertige Windows-Version sollte mindestens enthalten:

```text
Scrcpy Virtual Display/
│
├── ScrcpyLauncher.exe
├── ResetApp.exe
└── README.md
```

## 🛠️ Fehlerbehebung

### scrcpy.exe wurde nicht gefunden

Öffne den Setup-Assistenten erneut und wähle den richtigen Pfad zu `scrcpy.exe`.

### Android-Gerät wird nicht erkannt

Überprüfe:

* USB-Kabel
* USB-Debugging
* ADB-Treiber
* USB-Debugging-Berechtigung auf dem Smartphone

Teste außerdem:

```powershell
adb devices
```

### Setup erscheint nicht erneut

Starte:

```text
ResetApp.exe
```

Danach wird die gespeicherte Konfiguration gelöscht.

## 🔐 Sicherheit & Datenschutz

Die App benötigt keine Cloud-Verbindung und speichert ihre Konfiguration lokal auf dem Windows-PC.

Die App selbst sammelt keine persönlichen Daten.

Die Kommunikation mit dem Android-Gerät erfolgt über **ADB/scrcpy**.

## 📄 Lizenz

Dieses Projekt ist unabhängig von scrcpy.

`scrcpy` wird von **Genymobile** entwickelt.

Weitere Informationen und die Lizenz von scrcpy findest du im offiziellen Repository:

https://github.com/Genymobile/scrcpy

## ❤️ Credits

* **Scrcpy** – Genymobile
* **Scrcpy Virtual Display** – eigenes Launcher-/Konfigurationsprojekt

---

**Scrcpy Virtual Display**
Eine einfache Möglichkeit, scrcpy komfortabler mit Mirror- und External-Display-Modi zu verwenden.

**Plattform-Support:** Windows verfügbar · Linux & macOS geplant
