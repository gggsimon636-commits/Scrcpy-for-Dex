using System.Diagnostics;

namespace ScrcpyVirtualDisplay;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        AppSettings settings;

        // ========================================================
        // FIRST START
        // ========================================================

        if (!SettingsManager.Exists())
        {
            using SetupForm setup = new();

            if (setup.ShowDialog() != DialogResult.OK)
                return;

            settings = setup.Settings;

            SettingsManager.Save(settings);
        }
        else
        {
            settings = SettingsManager.Load();
        }

        // ========================================================
        // ASK FOR MODE ON EVERY START
        // ========================================================

        if (settings.AskForModeOnStartup)
        {
            using ModeSelectionForm modeForm =
                new(settings);

            if (modeForm.ShowDialog() != DialogResult.OK)
                return;

            // Save the selected mode so it becomes the
            // new default for the next start.
            SettingsManager.Save(settings);
        }

        // ========================================================
        // START SCRCPY
        // ========================================================

        try
        {
            StartScrcpy(settings);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Scrcpy Virtual Display",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    // ============================================================
    // START SCRCPY
    // ============================================================

    private static void StartScrcpy(
        AppSettings settings)
    {
        string scrcpyExe =
            settings.ScrcpyPath;

        // ========================================================
        // SCRCPY PATH
        // ========================================================

        // User selected scrcpy.exe directly
        if (File.Exists(scrcpyExe))
        {
            // Already correct.
        }
        // User selected the folder containing scrcpy.exe
        else if (Directory.Exists(scrcpyExe))
        {
            scrcpyExe =
                Path.Combine(
                    scrcpyExe,
                    "scrcpy.exe"
                );
        }

        if (!File.Exists(scrcpyExe))
        {
            throw new FileNotFoundException(
                "scrcpy.exe wurde nicht gefunden.\n\n" +
                "Aktueller Pfad:\n" +
                scrcpyExe +
                "\n\n" +
                "Öffne die Einstellungen und wähle " +
                "den richtigen scrcpy-Ordner."
            );
        }

        // ========================================================
        // ARGUMENTS
        // ========================================================

        List<string> arguments = new();

        // ========================================================
        // EXTERNAL / VIRTUAL DISPLAY
        // ========================================================

        if (settings.Mode ==
            DisplayMode.VirtualDisplay)
        {
            arguments.Add(
                $"--new-display=" +
                $"{settings.Width}x" +
                $"{settings.Height}/" +
                $"{settings.Dpi}"
            );
        }

        // ========================================================
        // AUDIO
        // ========================================================

        if (!settings.Audio)
        {
            arguments.Add(
                "--no-audio"
            );
        }

        // ========================================================
        // FULLSCREEN
        // ========================================================

        if (settings.Fullscreen)
        {
            arguments.Add(
                "--fullscreen"
            );
        }

        // ========================================================
        // DEVICE SERIAL
        // ========================================================

        if (!string.IsNullOrWhiteSpace(
            settings.DeviceSerial))
        {
            arguments.Add(
                $"--serial=\"{settings.DeviceSerial}\""
            );
        }

        // ========================================================
        // START PROCESS
        // ========================================================

        string? workingDirectory =
            Path.GetDirectoryName(
                scrcpyExe
            );

        ProcessStartInfo startInfo = new()
        {
            FileName = scrcpyExe,

            Arguments =
                string.Join(
                    " ",
                    arguments
                ),

            UseShellExecute = false,

            CreateNoWindow = false,

            WorkingDirectory =
                workingDirectory ?? ""
        };

        Process.Start(startInfo);
    }
}