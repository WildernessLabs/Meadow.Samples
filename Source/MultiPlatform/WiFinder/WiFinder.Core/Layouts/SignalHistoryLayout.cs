using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Gateway.WiFi;

namespace WiFinder.Core;

internal class SignalHistoryLayout : AbsoluteLayout
{
    protected IFont LargeFont { get; }

    private Label networkLabel;
    private Label signalLabel;
    private VerticalBarChart signalHistory;

    private WifiNetwork? displayedNetwork;
    private List<float> signalSeries = new();

    private const int HistoryDepth = 60;

    public SignalHistoryLayout(DisplayScreen screen)
        : base(screen)
    {
        LargeFont = new Font16x24();

        networkLabel = new Label(0, 0, 220, LargeFont.Height)
        {
            VerticalAlignment = VerticalAlignment.Center
        };
        signalLabel = new Label(220, 0, 100, LargeFont.Height)
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        signalHistory = new VerticalBarChart(0, networkLabel.Bottom + 3, screen.Width, screen.Height - networkLabel.Bottom)
        {
            SeriesColor = Color.AntiqueWhite,
            ShowXAxisLabels = false
        };

        for (var i = 0; i < HistoryDepth; i++)
        {
            signalSeries.Add(0);
        }

        this.Controls.Add(networkLabel, signalLabel, signalHistory);
    }

    public void Update(WifiNetwork? network)
    {
        if (network == null)
        {
            networkLabel.Text = "Select network";
            signalLabel.Text = string.Empty;
            return;
        }

        networkLabel.Text = $"{network.Ssid}";
        signalLabel.Text = $"{network.SignalDbStrength:N2}Db";
        AddSeriesValue(100f - network.SignalDbStrength * -1);
    }

    private void AddSeriesValue(float value)
    {
        signalSeries.Add(value);
        while (signalSeries.Count > HistoryDepth)
        {
            signalSeries.RemoveAt(0);
        }
        signalHistory.Series = signalSeries.ToArray();

    }

    protected override void OnDraw(MicroGraphics graphics)
    {
        base.OnDraw(graphics);
    }
}
