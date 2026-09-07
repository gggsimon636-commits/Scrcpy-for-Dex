using System.Text.Json;

namespace ScrcpyVirtualDisplay;

public class AppSettings
{
    public string ScrcpyPath { get; set; } = "";

    public DisplayMode Mode { get; set; } = DisplayMode.VirtualDisplay;

    public int Width { get; set; } = 1920;

    public int Height { get; set; } = 1080;

    public int Dpi { get; set; } = 160;

    public bool Audio { get; set; } = true;

    public bool Fullscreen { get; set; } = false;

    public string? DeviceSerial { get; set; }

    public bool AskForModeOnStartup { get; set; } = false;
}

public enum DisplayMode
{
    Mirror,
    VirtualDisplay
}

public static class SettingsManager
{
    private static readonly string Folder =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "ScrcpyVirtualDisplay"
        );

    private static readonly string FilePath =
        Path.Combine(Folder, "settings.json");

    public static bool Exists()
    {
        return File.Exists(FilePath);
    }

    public static AppSettings Load()
    {
        if (!File.Exists(FilePath))
            return new AppSettings();

        try
        {
            string json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<AppSettings>(json)
                   ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        Directory.CreateDirectory(Folder);

        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(
            settings,
            options
        );

        File.WriteAllText(FilePath, json);
    }

    public static string GetSettingsPath()
    {
        return FilePath;
    }
}