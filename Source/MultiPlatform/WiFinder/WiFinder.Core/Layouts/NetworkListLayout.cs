using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Gateway.WiFi;

namespace WiFinder.Core;

internal class NetworkListLayout : AbsoluteLayout
{
    protected IFont MediumFont { get; }
    protected IFont LargeFont { get; }

    private Label titleLabel;
    private Label antennaLabel;
    private bool antennaSelected;

    public ListBox NetworkList { get; private set; }

    public NetworkListLayout(DisplayScreen screen)
        : base(screen)
    {
        LargeFont = new Font12x20();

        titleLabel = new Label(0, 0, screen.Width - 50, LargeFont.Height)
        {
            Font = LargeFont,
            VerticalAlignment = VerticalAlignment.Center,
            Text = "Available networks",
        };

        antennaLabel = new Label(screen.Width - 50, 0, 50, LargeFont.Height)
        {
            Font = LargeFont,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Text = "INT",
        };

        NetworkList = new ListBox(3, titleLabel.Bottom + 3, screen.Width - 6, screen.Height - titleLabel.Bottom - 3, LargeFont)
        {
            SelectedRowColor = Color.AntiqueWhite,
            SelectedTextColor = Color.Black,
            RowFormatter = (row) =>
            {
                if (row is WifiNetwork network)
                {
                    return $"{network.SignalDbStrength:N2}db | {network.Ssid}";
                }
                return row.ToString();
            },
        };

        this.Controls.Add(titleLabel, NetworkList, antennaLabel);

    }

    public string AntennaText
    {
        get => antennaLabel.Text;
        set => antennaLabel.Text = value;
    }

    public bool AntennaSelected
    {
        get => antennaSelected;
        set
        {
            antennaSelected = value;

            if (AntennaSelected)
            {
                antennaLabel.BackColor = Color.Red;
            }
            else
            {
                antennaLabel.BackColor = Color.Transparent;
            }
        }
    }
}
