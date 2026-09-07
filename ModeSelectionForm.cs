using System.Drawing;
using System.Windows.Forms;

namespace ScrcpyVirtualDisplay;

public class ModeSelectionForm : Form
{
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

    private readonly AppSettings settings;

    private RoundedPanel mirrorCard = null!;
    private RoundedPanel externalCard = null!;

    private RoundedButton startButton = null!;

    public ModeSelectionForm(AppSettings settings)
    {
        this.settings = settings;

        Text = "Scrcpy Virtual Display";

        StartPosition =
            FormStartPosition.CenterScreen;

        ClientSize =
            new Size(720, 430);

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

        BuildUI();
    }

    private void BuildUI()
    {
        Label title = new()
        {
            Text = "Choose display mode",
            Font = new Font(
                "Segoe UI Semibold",
                25,
                FontStyle.Bold
            ),
            ForeColor = TextColor,
            AutoSize = true,
            Location = new Point(35, 30)
        };

        Label subtitle = new()
        {
            Text =
                "How do you want to use your Android device?",
            Font = new Font(
                "Segoe UI",
                11
            ),
            ForeColor = SecondaryTextColor,
            AutoSize = true,
            Location = new Point(38, 78)
        };

        Controls.Add(title);
        Controls.Add(subtitle);

        mirrorCard = CreateCard(
            "Mirror",
            "Show your normal Android screen\n" +
            "in a scrcpy window.",
            DisplayMode.Mirror
        );

        externalCard = CreateCard(
            "External",
            "Create a separate display.\n" +
            "Recommended for Samsung DeX.",
            DisplayMode.VirtualDisplay
        );

        mirrorCard.Left = 35;
        mirrorCard.Top = 125;

        externalCard.Left = 370;
        externalCard.Top = 125;

        Controls.Add(mirrorCard);
        Controls.Add(externalCard);

        startButton = new RoundedButton
        {
            Text = "Start",
            Width = 130,
            Height = 44,
            Left = ClientSize.Width - 165,
            Top = 350,

            BackColor = AccentColor,
            ForeColor = Color.White,
            BorderColor = AccentColor,
            Radius = 10
        };

        startButton.Click += (_, _) =>
        {
            SettingsManager.Save(settings);

            DialogResult =
                DialogResult.OK;

            Close();
        };

        Controls.Add(startButton);

        UpdateCards();
    }

    private RoundedPanel CreateCard(
        string title,
        string description,
        DisplayMode mode)
    {
        RoundedPanel card = new()
        {
            Width = 315,
            Height = 175,

            BackColor = CardColor,
            BorderColor = BorderColor,

            Radius = 16,

            Cursor = Cursors.Hand,

            Tag = mode
        };

        Label titleLabel = new()
        {
            Text = title,

            Font = new Font(
                "Segoe UI Semibold",
                17,
                FontStyle.Bold
            ),

            ForeColor = TextColor,

            AutoSize = true,

            Left = 23,
            Top = 23,

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

            Left = 23,
            Top = 68,

            Cursor = Cursors.Hand
        };

        Label check = new()
        {
            Text = "✓",

            Font = new Font(
                "Segoe UI Semibold",
                20,
                FontStyle.Bold
            ),

            ForeColor = AccentColor,

            AutoSize = true,

            Left = 265,
            Top = 125,

            Visible = false,

            Cursor = Cursors.Hand,

            Tag = "check"
        };

        card.Controls.Add(titleLabel);
        card.Controls.Add(descriptionLabel);
        card.Controls.Add(check);

        card.Click += (_, _) =>
        {
            SelectMode(mode);
        };

        titleLabel.Click += (_, _) =>
        {
            SelectMode(mode);
        };

        descriptionLabel.Click += (_, _) =>
        {
            SelectMode(mode);
        };

        check.Click += (_, _) =>
        {
            SelectMode(mode);
        };

        return card;
    }

    private void SelectMode(DisplayMode mode)
    {
        settings.Mode = mode;

        UpdateCards();
    }

    private void UpdateCards()
    {
        bool mirror =
            settings.Mode ==
            DisplayMode.Mirror;

        mirrorCard.BackColor =
            mirror
                ? SelectedCardColor
                : CardColor;

        externalCard.BackColor =
            !mirror
                ? SelectedCardColor
                : CardColor;

        mirrorCard.BorderColor =
            mirror
                ? AccentColor
                : BorderColor;

        externalCard.BorderColor =
            !mirror
                ? AccentColor
                : BorderColor;

        SetCheck(
            mirrorCard,
            mirror
        );

        SetCheck(
            externalCard,
            !mirror
        );

        mirrorCard.Invalidate();
        externalCard.Invalidate();
    }

    private void SetCheck(
        Control card,
        bool visible)
    {
        foreach (Control child in card.Controls)
        {
            if (child is Label label &&
                label.Tag?.ToString() == "check")
            {
                label.Visible = visible;
            }
        }
    }
}