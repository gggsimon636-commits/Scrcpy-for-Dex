using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScrcpyVirtualDisplay;

public class SetupForm : Form
{
    // ============================================================
    // COLORS
    // ============================================================

    private static readonly Color BackgroundColor =
        Color.FromArgb(15, 15, 18);

    private static readonly Color CardColor =
        Color.FromArgb(24, 24, 28);

    private static readonly Color SelectedCardColor =
        Color.FromArgb(27, 39, 54);

    private static readonly Color BorderColor =
        Color.FromArgb(48, 48, 55);

    private static readonly Color TextColor =
        Color.FromArgb(245, 245, 247);

    private static readonly Color SecondaryTextColor =
        Color.FromArgb(160, 160, 170);

    private static readonly Color AccentColor =
        Color.FromArgb(0, 120, 215);

    // ============================================================
    // CONTROLS
    // ============================================================

    private readonly Panel contentPanel;
    private readonly Panel progressPanel;
    private readonly Label progressLabel;
    private readonly Label pageLabel;

    private readonly RoundedButton backButton;
    private readonly RoundedButton nextButton;

    private int currentPage = 0;

    private const int PageCount = 6;

    // ============================================================
    // SETTINGS
    // ============================================================

    public AppSettings Settings { get; private set; } = new();

    private TextBox? scrcpyPathBox;

    private RoundedPanel? mirrorCard;
    private RoundedPanel? virtualCard;

    private ComboBox? resolutionBox;
    private NumericUpDown? dpiBox;

    private CheckBox? audioBox;
    private CheckBox? fullscreenBox;

    private CheckBox? askModeOnStartupBox;

    private TextBox? serialBox;

    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    public SetupForm()
    {
        Text = "Scrcpy Virtual Display";

        StartPosition =
            FormStartPosition.CenterScreen;

        ClientSize =
            new Size(850, 600);

        MinimumSize =
            new Size(750, 520);

        BackColor =
            BackgroundColor;

        ForeColor =
            TextColor;

        Font =
            new Font(
                "Segoe UI",
                10
            );

        FormBorderStyle =
            FormBorderStyle.FixedSingle;

        MaximizeBox = false;

        // ========================================================
        // TOP BAR
        // ========================================================

        Panel topBar = new()
        {
            Dock = DockStyle.Top,
            Height = 72,
            BackColor = BackgroundColor
        };

        Label logo = new()
        {
            Text = "SCRCPY",
            Font = new Font(
                "Segoe UI Semibold",
                12,
                FontStyle.Bold
            ),
            ForeColor = AccentColor,
            AutoSize = true,
            Location = new Point(32, 26)
        };

        pageLabel = new Label
        {
            Text = "Welcome",
            Font = new Font(
                "Segoe UI",
                10
            ),
            ForeColor = SecondaryTextColor,
            AutoSize = true,
            Location = new Point(120, 28)
        };

        progressLabel = new Label
        {
            Text = "1 / 6",
            Font = new Font(
                "Segoe UI Semibold",
                10
            ),
            ForeColor = SecondaryTextColor,
            AutoSize = true
        };

        topBar.Controls.Add(logo);
        topBar.Controls.Add(pageLabel);
        topBar.Controls.Add(progressLabel);

        // ========================================================
        // PROGRESS BAR
        // ========================================================

        progressPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 3,
            BackColor = BorderColor
        };

        // ========================================================
        // CONTENT
        // ========================================================

        contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = BackgroundColor,
            Padding =
                new Padding(
                    55,
                    30,
                    55,
                    20
                )
        };

        // ========================================================
        // NAVIGATION
        // ========================================================

        Panel navigation = new()
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = BackgroundColor
        };

        backButton = new RoundedButton
        {
            Text = "Back",
            Width = 115,
            Height = 42,

            BackColor = CardColor,
            ForeColor = TextColor,
            BorderColor = BorderColor,

            Radius = 10
        };

        nextButton = new RoundedButton
        {
            Text = "Next",
            Width = 125,
            Height = 42,

            BackColor = AccentColor,
            ForeColor = Color.White,
            BorderColor = AccentColor,

            Radius = 10
        };

        navigation.Controls.Add(backButton);
        navigation.Controls.Add(nextButton);

        // ========================================================
        // IMPORTANT:
        // DOCKED CONTROLS MUST BE ADDED IN THIS ORDER
        // ========================================================

        Controls.Add(navigation);
        Controls.Add(contentPanel);
        Controls.Add(progressPanel);
        Controls.Add(topBar);

        // ========================================================
        // RESIZE
        // ========================================================

        Resize += (_, _) =>
        {
            progressLabel.Left =
                ClientSize.Width -
                progressLabel.Width -
                35;

            nextButton.Left =
                navigation.ClientSize.Width -
                nextButton.Width -
                25;

            backButton.Left = 25;

            UpdateProgressBar();
        };

        progressLabel.Left =
            ClientSize.Width -
            progressLabel.Width -
            35;

        nextButton.Left =
            navigation.ClientSize.Width -
            nextButton.Width -
            25;

        backButton.Left = 25;

        // ========================================================
        // EVENTS
        // ========================================================

        backButton.Click +=
            BackButton_Click;

        nextButton.Click +=
            NextButton_Click;

        // ========================================================
        // SHOW FIRST PAGE
        // ========================================================

        ShowPage();
    }

    // ============================================================
    // PAGE MANAGEMENT
    // ============================================================

    private void ShowPage()
    {
        contentPanel.SuspendLayout();

        contentPanel.Controls.Clear();

        backButton.Enabled =
            currentPage > 0;

        nextButton.Text =
            currentPage == PageCount - 1
                ? "Finish"
                : "Next";

        progressLabel.Text =
            $"{currentPage + 1} / {PageCount}";

        pageLabel.Text =
            GetPageName();

        UpdateProgressBar();

        switch (currentPage)
        {
            case 0:
                ShowWelcome();
                break;

            case 1:
                ShowScrcpyPath();
                break;

            case 2:
                ShowDisplayMode();
                break;

            case 3:
                ShowResolution();
                break;

            case 4:
                ShowOptions();
                break;

            case 5:
                ShowSummary();
                break;
        }

        contentPanel.ResumeLayout();
    }

    private string GetPageName()
    {
        return currentPage switch
        {
            0 => "Welcome",
            1 => "scrcpy",
            2 => "Display Mode",
            3 => "Display",
            4 => "Options",
            5 => "Finish",
            _ => "Setup"
        };
    }

    // ============================================================
    // PROGRESS
    // ============================================================

    private void UpdateProgressBar()
    {
        if (progressPanel.Width <= 0)
            return;

        progressPanel.Controls.Clear();

        int width =
            (int)(
                progressPanel.Width *
                ((double)(currentPage + 1) /
                 PageCount)
            );

        Panel progress = new()
        {
            Dock = DockStyle.Left,
            Width = width,
            Height = 3,
            BackColor = AccentColor
        };

        progressPanel.Controls.Add(progress);
    }

    // ============================================================
    // TEXT HELPERS
    // ============================================================

    private Label CreateTitle(string text)
    {
        return new Label
        {
            Text = text,

            Font = new Font(
                "Segoe UI Semibold",
                26,
                FontStyle.Bold
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Location =
                new Point(0, 5)
        };
    }

    private Label CreateSubtitle(string text)
    {
        return new Label
        {
            Text = text,

            Font = new Font(
                "Segoe UI",
                11
            ),

            ForeColor =
                SecondaryTextColor,

            AutoSize = true,

            MaximumSize =
                new Size(680, 0),

            Location =
                new Point(0, 55)
        };
    }

    // ============================================================
    // PAGE 0 - WELCOME
    // ============================================================

    private void ShowWelcome()
    {
        contentPanel.Controls.Add(
            CreateTitle("Welcome.")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Let's configure scrcpy for your Android device."
            )
        );

        RoundedPanel info = new()
        {
            Left = 0,
            Top = 115,
            Width = 680,
            Height = 180,

            BackColor = CardColor,
            BorderColor = BorderColor,
            Radius = 16
        };

        Label icon = new()
        {
            Text = "▣",

            Font = new Font(
                "Segoe UI",
                30
            ),

            ForeColor = AccentColor,

            AutoSize = true,

            Left = 25,
            Top = 25
        };

        Label text = new()
        {
            Text =
                "This wizard will help you configure your\n" +
                "scrcpy connection and display settings.\n\n" +
                "You can use normal mirroring or create a\n" +
                "separate virtual Android display.",

            Font = new Font(
                "Segoe UI",
                11
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 80,
            Top = 30
        };

        info.Controls.Add(icon);
        info.Controls.Add(text);

        contentPanel.Controls.Add(info);
    }

    // ============================================================
    // PAGE 1 - SCRCPY
    // ============================================================

    private void ShowScrcpyPath()
    {
        contentPanel.Controls.Add(
            CreateTitle("Choose scrcpy")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Tell us where scrcpy.exe is installed."
            )
        );

        Label pathLabel = new()
        {
            Text = "scrcpy location",

            Font = new Font(
                "Segoe UI Semibold",
                10
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 0,
            Top = 120
        };

        contentPanel.Controls.Add(pathLabel);

        scrcpyPathBox = new TextBox
        {
            Left = 0,
            Top = 150,
            Width = 535,
            Height = 40,

            BackColor =
                Color.FromArgb(
                    20,
                    20,
                    24
                ),

            ForeColor = TextColor,

            BorderStyle =
                BorderStyle.FixedSingle,

            Font = new Font(
                "Segoe UI",
                11
            ),

            Text = Settings.ScrcpyPath ?? ""
        };

        RoundedButton browse = new()
        {
            Text = "Browse",

            Left = 550,
            Top = 148,

            Width = 120,
            Height = 42,

            BackColor = CardColor,
            ForeColor = TextColor,
            BorderColor = BorderColor,

            Radius = 10
        };

        browse.Click += (_, _) =>
        {
            using FolderBrowserDialog dialog = new();

            dialog.Description =
                "Select the folder containing scrcpy.exe";

            if (dialog.ShowDialog(this) ==
                DialogResult.OK)
            {
                scrcpyPathBox.Text =
                    dialog.SelectedPath;
            }
        };

        contentPanel.Controls.Add(scrcpyPathBox);
        contentPanel.Controls.Add(browse);

        Label hint = new()
        {
            Text =
                @"Example: C:\Tools\scrcpy",

            Font = new Font(
                "Segoe UI",
                9
            ),

            ForeColor =
                SecondaryTextColor,

            AutoSize = true,

            Left = 0,
            Top = 200
        };

        contentPanel.Controls.Add(hint);
    }

    // ============================================================
    // PAGE 2 - DISPLAY MODE
    // ============================================================

    private void ShowDisplayMode()
    {
        contentPanel.Controls.Add(
            CreateTitle("Display mode")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Choose how your Android device should appear."
            )
        );

        mirrorCard = CreateModeCard(
            "Mirror",
            "Mirror the Android device's\n" +
            "main display in a scrcpy window."
        );

        virtualCard = CreateModeCard(
            "Virtual Display",
            "Create a separate Android display\n" +
            "with its own resolution and DPI."
        );

        mirrorCard.Left = 0;
        mirrorCard.Top = 125;

        virtualCard.Left = 350;
        virtualCard.Top = 125;

        contentPanel.Controls.Add(mirrorCard);
        contentPanel.Controls.Add(virtualCard);

        mirrorCard.Click += (_, _) =>
        {
            Settings.Mode =
                DisplayMode.Mirror;

            UpdateModeCards();
        };

        virtualCard.Click += (_, _) =>
        {
            Settings.Mode =
                DisplayMode.VirtualDisplay;

            UpdateModeCards();
        };

        foreach (Control control in mirrorCard.Controls)
        {
            control.Click += (_, _) =>
            {
                Settings.Mode =
                    DisplayMode.Mirror;

                UpdateModeCards();
            };
        }

        foreach (Control control in virtualCard.Controls)
        {
            control.Click += (_, _) =>
            {
                Settings.Mode =
                    DisplayMode.VirtualDisplay;

                UpdateModeCards();
            };
        }

        UpdateModeCards();
    }

    private RoundedPanel CreateModeCard(
        string title,
        string description)
    {
        RoundedPanel card = new()
        {
            Width = 320,
            Height = 190,

            BackColor = CardColor,
            BorderColor = BorderColor,

            Radius = 16,

            Cursor = Cursors.Hand
        };

        Label titleLabel = new()
        {
            Text = title,

            Font = new Font(
                "Segoe UI Semibold",
                15,
                FontStyle.Bold
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 22,
            Top = 22,

            Cursor = Cursors.Hand
        };

        Label descriptionLabel = new()
        {
            Text = description,

            Font = new Font(
                "Segoe UI",
                10
            ),

            ForeColor =
                SecondaryTextColor,

            AutoSize = true,

            Left = 22,
            Top = 65,

            Cursor = Cursors.Hand
        };

        Label check = new()
        {
            Text = "✓",

            Font = new Font(
                "Segoe UI Semibold",
                18,
                FontStyle.Bold
            ),

            ForeColor = AccentColor,

            AutoSize = true,

            Left = 270,
            Top = 135,

            Visible = false,

            Cursor = Cursors.Hand
        };

        card.Controls.Add(titleLabel);
        card.Controls.Add(descriptionLabel);
        card.Controls.Add(check);

        return card;
    }

    private void UpdateModeCards()
    {
        if (mirrorCard == null ||
            virtualCard == null)
            return;

        bool isMirror =
            Settings.Mode ==
            DisplayMode.Mirror;

        mirrorCard.BackColor =
            isMirror
                ? SelectedCardColor
                : CardColor;

        virtualCard.BackColor =
            !isMirror
                ? SelectedCardColor
                : CardColor;

        mirrorCard.BorderColor =
            isMirror
                ? AccentColor
                : BorderColor;

        virtualCard.BorderColor =
            !isMirror
                ? AccentColor
                : BorderColor;

        SetCheckVisibility(
            mirrorCard,
            isMirror
        );

        SetCheckVisibility(
            virtualCard,
            !isMirror
        );

        mirrorCard.Invalidate();
        virtualCard.Invalidate();
    }

    private void SetCheckVisibility(
        Control control,
        bool visible)
    {
        foreach (Control child in control.Controls)
        {
            if (child is Label label &&
                label.Text == "✓")
            {
                label.Visible = visible;
            }
        }
    }

    // ============================================================
    // PAGE 3 - RESOLUTION
    // ============================================================

    private void ShowResolution()
    {
        contentPanel.Controls.Add(
            CreateTitle("Display settings")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Configure the size and density of your virtual display."
            )
        );

        Label resolutionLabel = new()
        {
            Text = "Resolution",

            Font = new Font(
                "Segoe UI Semibold",
                10
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 0,
            Top = 125
        };

        resolutionBox = new ComboBox
        {
            Left = 0,
            Top = 155,

            Width = 320,
            Height = 40,

            BackColor = CardColor,
            ForeColor = TextColor,

            DropDownStyle =
                ComboBoxStyle.DropDownList
        };

        resolutionBox.Items.AddRange(
            new object[]
            {
                "1280 x 720",
                "1600 x 900",
                "1920 x 1080",
                "2560 x 1440",
                "3840 x 2160"
            }
        );

        int selectedResolution = 2;

        if (Settings.Width > 0 &&
            Settings.Height > 0)
        {
            string current =
                $"{Settings.Width} x {Settings.Height}";

            int index =
                resolutionBox.Items.IndexOf(
                    current
                );

            if (index >= 0)
                selectedResolution = index;
        }

        resolutionBox.SelectedIndex =
            selectedResolution;

        contentPanel.Controls.Add(resolutionLabel);
        contentPanel.Controls.Add(resolutionBox);

        Label dpiLabel = new()
        {
            Text = "DPI",

            Font = new Font(
                "Segoe UI Semibold",
                10
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 370,
            Top = 125
        };

        dpiBox = new NumericUpDown
        {
            Left = 370,
            Top = 155,

            Width = 180,
            Height = 40,

            Minimum = 72,
            Maximum = 640,

            Value =
                Math.Clamp(
                    Settings.Dpi > 0
                        ? Settings.Dpi
                        : 160,
                    72,
                    640
                ),

            BackColor = CardColor,
            ForeColor = TextColor,

            Font = new Font(
                "Segoe UI",
                11
            )
        };

        contentPanel.Controls.Add(dpiLabel);
        contentPanel.Controls.Add(dpiBox);

        RoundedPanel preview = new()
        {
            Left = 0,
            Top = 225,

            Width = 680,
            Height = 130,

            BackColor = CardColor,
            BorderColor = BorderColor,

            Radius = 16
        };

        Label previewTitle = new()
        {
            Text = "Virtual display",

            Font = new Font(
                "Segoe UI Semibold",
                11,
                FontStyle.Bold
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 20,
            Top = 20
        };

        Label previewText = new()
        {
            Font = new Font(
                "Segoe UI",
                10
            ),

            ForeColor =
                SecondaryTextColor,

            AutoSize = true,

            Left = 20,
            Top = 55
        };

        preview.Controls.Add(previewTitle);
        preview.Controls.Add(previewText);

        contentPanel.Controls.Add(preview);

        resolutionBox.SelectedIndexChanged +=
            (_, _) =>
            {
                UpdateResolutionPreview(
                    previewText
                );
            };

        dpiBox.ValueChanged +=
            (_, _) =>
            {
                UpdateResolutionPreview(
                    previewText
                );
            };

        UpdateResolutionPreview(previewText);
    }

    private void UpdateResolutionPreview(
        Label label)
    {
        string resolution =
            resolutionBox?.SelectedItem
                ?.ToString()
            ?? "1920 x 1080";

        string dpi =
            dpiBox?.Value.ToString()
            ?? "160";

        label.Text =
            $"Resolution: {resolution}\n" +
            $"Density: {dpi} DPI";
    }

    // ============================================================
    // PAGE 4 - OPTIONS
    // ============================================================

    private void ShowOptions()
    {
        contentPanel.Controls.Add(
            CreateTitle("Options")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Fine-tune how Scrcpy Virtual Display behaves when it starts."
            )
        );

        RoundedPanel optionsCard = new()
        {
            Left = 0,
            Top = 120,

            Width = 680,
            Height = 280,

            BackColor = CardColor,
            BorderColor = BorderColor,

            Radius = 16
        };

        audioBox = new CheckBox
        {
            Text = "Forward audio",

            Left = 25,
            Top = 20,

            Width = 300,
            Height = 30,

            Checked =
                Settings.Audio,

            ForeColor = TextColor,
            BackColor = Color.Transparent,

            Font = new Font(
                "Segoe UI",
                10
            )
        };

        fullscreenBox = new CheckBox
        {
            Text = "Start in fullscreen",

            Left = 25,
            Top = 65,

            Width = 300,
            Height = 30,

            Checked =
                Settings.Fullscreen,

            ForeColor = TextColor,
            BackColor = Color.Transparent,

            Font = new Font(
                "Segoe UI",
                10
            )
        };

        // ========================================================
        // NEW:
        // ALWAYS ASK FOR MIRROR / EXTERNAL
        // ========================================================

        askModeOnStartupBox = new CheckBox
        {
            Text = "Ask for Mirror or External mode when starting",

            Left = 25,
            Top = 110,

            Width = 600,
            Height = 30,

            Checked =
                Settings.AskForModeOnStartup,

            ForeColor = TextColor,
            BackColor = Color.Transparent,

            Font = new Font(
                "Segoe UI",
                10
            )
        };

        Label modeHint = new()
        {
            Text =
                "External mode is intended for Samsung DeX / a separate display.",

            Left = 50,
            Top = 138,

            AutoSize = true,

            ForeColor =
                SecondaryTextColor,

            Font = new Font(
                "Segoe UI",
                9
            )
        };

        Label deviceLabel = new()
        {
            Text = "Device ID (optional)",

            Left = 25,
            Top = 175,

            AutoSize = true,

            ForeColor = TextColor,

            Font = new Font(
                "Segoe UI Semibold",
                10
            )
        };

        serialBox = new TextBox
        {
            Left = 25,
            Top = 202,

            Width = 400,
            Height = 35,

            BackColor =
                Color.FromArgb(
                    18,
                    18,
                    22
                ),

            ForeColor = TextColor,

            BorderStyle =
                BorderStyle.FixedSingle,

            Font = new Font(
                "Segoe UI",
                10
            ),

            Text =
                Settings.DeviceSerial ?? ""
        };

        optionsCard.Controls.Add(audioBox);
        optionsCard.Controls.Add(fullscreenBox);
        optionsCard.Controls.Add(askModeOnStartupBox);
        optionsCard.Controls.Add(modeHint);
        optionsCard.Controls.Add(deviceLabel);
        optionsCard.Controls.Add(serialBox);

        contentPanel.Controls.Add(optionsCard);
    }

    // ============================================================
    // PAGE 5 - SUMMARY
    // ============================================================

    private void ShowSummary()
    {
        contentPanel.Controls.Add(
            CreateTitle("You're ready.")
        );

        contentPanel.Controls.Add(
            CreateSubtitle(
                "Review your configuration before starting scrcpy."
            )
        );

        RoundedPanel summary = new()
        {
            Left = 0,
            Top = 120,

            Width = 680,
            Height = 330,

            BackColor = CardColor,
            BorderColor = BorderColor,

            Radius = 16
        };

        string mode =
            Settings.Mode ==
            DisplayMode.VirtualDisplay
                ? "Virtual Display / External"
                : "Normal Mirroring";

        string resolution =
            resolutionBox?.SelectedItem
                ?.ToString()
            ?? "1920 x 1080";

        string dpi =
            dpiBox?.Value.ToString()
            ?? "160";

        string audio =
            audioBox?.Checked == true
                ? "Enabled"
                : "Disabled";

        string fullscreen =
            fullscreenBox?.Checked == true
                ? "Enabled"
                : "Disabled";

        string askMode =
            askModeOnStartupBox?.Checked == true
                ? "Yes"
                : "No";

        AddSummaryLine(
            summary,
            "Mode",
            mode,
            25
        );

        AddSummaryLine(
            summary,
            "Resolution",
            resolution,
            70
        );

        AddSummaryLine(
            summary,
            "DPI",
            dpi,
            115
        );

        AddSummaryLine(
            summary,
            "Audio",
            audio,
            160
        );

        AddSummaryLine(
            summary,
            "Fullscreen",
            fullscreen,
            205
        );

        AddSummaryLine(
            summary,
            "Ask at startup",
            askMode,
            250
        );

        contentPanel.Controls.Add(summary);
    }

    private void AddSummaryLine(
        Panel parent,
        string name,
        string value,
        int top)
    {
        Label nameLabel = new()
        {
            Text = name,

            Left = 25,
            Top = top,

            ForeColor =
                SecondaryTextColor,

            Font = new Font(
                "Segoe UI",
                10
            ),

            AutoSize = true
        };

        Label valueLabel = new()
        {
            Text = value,

            Left = 200,
            Top = top,

            ForeColor = TextColor,

            Font = new Font(
                "Segoe UI Semibold",
                10
            ),

            AutoSize = true
        };

        parent.Controls.Add(nameLabel);
        parent.Controls.Add(valueLabel);
    }

    // ============================================================
    // NAVIGATION
    // ============================================================

    private void BackButton_Click(
        object? sender,
        EventArgs e)
    {
        if (currentPage <= 0)
            return;

        // Save before going back so changes aren't lost.
        SaveCurrentPage();

        currentPage--;

        ShowPage();
    }

    private void NextButton_Click(
        object? sender,
        EventArgs e)
    {
        if (!ValidateCurrentPage())
            return;

        SaveCurrentPage();

        if (currentPage < PageCount - 1)
        {
            currentPage++;

            ShowPage();

            return;
        }

        SettingsManager.Save(Settings);

        DialogResult =
            DialogResult.OK;

        Close();
    }

    // ============================================================
    // VALIDATION
    // ============================================================

    private bool ValidateCurrentPage()
    {
        if (currentPage == 1)
        {
            if (string.IsNullOrWhiteSpace(
                scrcpyPathBox?.Text))
            {
                ShowError(
                    "Please select the folder containing scrcpy.exe."
                );

                return false;
            }

            string path =
                scrcpyPathBox.Text.Trim();

            if (Directory.Exists(path))
            {
                path =
                    Path.Combine(
                        path,
                        "scrcpy.exe"
                    );
            }

            if (!File.Exists(path))
            {
                ShowError(
                    "scrcpy.exe could not be found in the selected folder."
                );

                return false;
            }
        }

        return true;
    }

    private void ShowError(
        string message)
    {
        MessageBox.Show(
            this,
            message,
            "Setup Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        );
    }

    // ============================================================
    // SAVE SETTINGS
    // ============================================================

    private void SaveCurrentPage()
    {
        if (scrcpyPathBox != null)
        {
            Settings.ScrcpyPath =
                scrcpyPathBox.Text.Trim();
        }

        if (resolutionBox != null &&
            resolutionBox.SelectedItem != null)
        {
            string resolution =
                resolutionBox
                    .SelectedItem
                    .ToString()!
                    .Replace(" ", "");

            string[] parts =
                resolution.Split('x');

            if (parts.Length == 2)
            {
                if (int.TryParse(
                    parts[0],
                    out int width))
                {
                    Settings.Width =
                        width;
                }

                if (int.TryParse(
                    parts[1],
                    out int height))
                {
                    Settings.Height =
                        height;
                }
            }
        }

        if (dpiBox != null)
        {
            Settings.Dpi =
                (int)dpiBox.Value;
        }

        if (audioBox != null)
        {
            Settings.Audio =
                audioBox.Checked;
        }

        if (fullscreenBox != null)
        {
            Settings.Fullscreen =
                fullscreenBox.Checked;
        }

        if (askModeOnStartupBox != null)
        {
            Settings.AskForModeOnStartup =
                askModeOnStartupBox.Checked;
        }

        if (serialBox != null)
        {
            Settings.DeviceSerial =
                string.IsNullOrWhiteSpace(
                    serialBox.Text)
                    ? null
                    : serialBox.Text.Trim();
        }
    }
}


// ====================================================================
// ROUNDED PANEL
// ====================================================================

public class RoundedPanel : Panel
{
    public int Radius { get; set; } = 12;

    public Color BorderColor { get; set; } =
        Color.FromArgb(
            48,
            48,
            55
        );

    public RoundedPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;

        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true
        );
    }

    protected override void OnPaint(
        PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode =
            SmoothingMode.AntiAlias;

        Rectangle rect =
            new Rectangle(
                0,
                0,
                Width - 1,
                Height - 1
            );

        using GraphicsPath path =
            CreateRoundedPath(
                rect,
                Radius
            );

        using Pen pen =
            new Pen(
                BorderColor,
                1
            );

        e.Graphics.DrawPath(
            pen,
            path
        );
    }

    private static GraphicsPath
        CreateRoundedPath(
            Rectangle rect,
            int radius)
    {
        GraphicsPath path =
            new GraphicsPath();

        radius =
            Math.Min(
                radius,
                Math.Min(
                    rect.Width / 2,
                    rect.Height / 2
                )
            );

        int diameter =
            radius * 2;

        path.AddArc(
            rect.X,
            rect.Y,
            diameter,
            diameter,
            180,
            90
        );

        path.AddArc(
            rect.Right - diameter,
            rect.Y,
            diameter,
            diameter,
            270,
            90
        );

        path.AddArc(
            rect.Right - diameter,
            rect.Bottom - diameter,
            diameter,
            diameter,
            0,
            90
        );

        path.AddArc(
            rect.X,
            rect.Bottom - diameter,
            diameter,
            diameter,
            90,
            90
        );

        path.CloseFigure();

        return path;
    }
}


// ====================================================================
// ROUNDED BUTTON
// ====================================================================

public class RoundedButton : Control
{
    public int Radius { get; set; } = 10;

    public Color BorderColor { get; set; } =
        Color.Transparent;

    private bool hovered;
    private bool pressed;

    public RoundedButton()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true
        );

        Cursor =
            Cursors.Hand;

        TabStop = true;

        MouseEnter += (_, _) =>
        {
            hovered = true;
            Invalidate();
        };

        MouseLeave += (_, _) =>
        {
            hovered = false;
            pressed = false;
            Invalidate();
        };

        MouseDown += (_, e) =>
        {
            if (e.Button ==
                MouseButtons.Left)
            {
                pressed = true;
                Invalidate();
            }
        };

        MouseUp += (_, e) =>
        {
            if (e.Button ==
                MouseButtons.Left)
            {
                pressed = false;
                Invalidate();
            }
        };
    }

    protected override void OnPaint(
        PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode =
            SmoothingMode.AntiAlias;

        Rectangle rect =
            new Rectangle(
                0,
                0,
                Width - 1,
                Height - 1
            );

        Color background =
            BackColor;

        if (pressed)
        {
            background =
                Darken(
                    BackColor,
                    0.85f
                );
        }
        else if (hovered)
        {
            background =
                Lighten(
                    BackColor,
                    0.12f
                );
        }

        using GraphicsPath path =
            CreateRoundedPath(
                rect,
                Radius
            );

        using SolidBrush brush =
            new SolidBrush(
                background
            );

        e.Graphics.FillPath(
            brush,
            path
        );

        if (BorderColor !=
            Color.Transparent)
        {
            using Pen pen =
                new Pen(
                    BorderColor,
                    1
                );

            e.Graphics.DrawPath(
                pen,
                path
            );
        }

        TextRenderer.DrawText(
            e.Graphics,
            Text,
            Font,
            rect,
            ForeColor,
            TextFormatFlags.HorizontalCenter |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding
        );
    }

    protected override void OnKeyDown(
        KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.KeyCode == Keys.Enter ||
            e.KeyCode == Keys.Space)
        {
            pressed = true;

            Invalidate();
        }
    }

    protected override void OnKeyUp(
        KeyEventArgs e)
    {
        base.OnKeyUp(e);

        if (e.KeyCode == Keys.Enter ||
            e.KeyCode == Keys.Space)
        {
            pressed = false;

            Invalidate();

            PerformClick();
        }
    }

    private void PerformClick()
    {
        OnClick(EventArgs.Empty);
    }

    private static Color Lighten(
        Color color,
        float amount)
    {
        int r =
            color.R +
            (int)(
                (255 - color.R) *
                amount
            );

        int g =
            color.G +
            (int)(
                (255 - color.G) *
                amount
            );

        int b =
            color.B +
            (int)(
                (255 - color.B) *
                amount
            );

        return Color.FromArgb(
            color.A,
            Math.Min(255, r),
            Math.Min(255, g),
            Math.Min(255, b)
        );
    }

    private static Color Darken(
        Color color,
        float amount)
    {
        return Color.FromArgb(
            color.A,
            (int)(color.R * amount),
            (int)(color.G * amount),
            (int)(color.B * amount)
        );
    }

    private static GraphicsPath
        CreateRoundedPath(
            Rectangle rect,
            int radius)
    {
        GraphicsPath path =
            new GraphicsPath();

        radius =
            Math.Min(
                radius,
                Math.Min(
                    rect.Width / 2,
                    rect.Height / 2
                )
            );

        int diameter =
            radius * 2;

        path.AddArc(
            rect.X,
            rect.Y,
            diameter,
            diameter,
            180,
            90
        );

        path.AddArc(
            rect.Right - diameter,
            rect.Y,
            diameter,
            diameter,
            270,
            90
        );

        path.AddArc(
            rect.Right - diameter,
            rect.Bottom - diameter,
            diameter,
            diameter,
            0,
            90
        );

        path.AddArc(
            rect.X,
            rect.Bottom - diameter,
            diameter,
            diameter,
            90,
            90
        );

        path.CloseFigure();

        return path;
    }
}