using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace HMI_Views.Views;

public class HistogramView
{
    private DisplayScreen screen;
    private HistogramChart spectraChart1;
    private HistogramChart spectraChart2;

    public HistogramView(IPixelDisplay _display)
    {
        screen = new DisplayScreen(_display);

        spectraChart1 = new HistogramChart(10, 0, screen.Width - 20, screen.Height / 2);
        spectraChart1.Series.Add(new HistogramChartSeries { ForegroundColor = Color.Red });

        spectraChart2 = new HistogramChart(10, screen.Height / 2, screen.Width - 20, screen.Height / 2);
        spectraChart2.Series.Add(new HistogramChartSeries { ForegroundColor = Color.Green });

        screen.Controls.Add(spectraChart1, spectraChart2);
    }

    public async Task Run()
    {
        var count = 30;

        while (true)
        {
            screen.BeginUpdate();

            var s1 = new (int X, int Y)[count];
            s1[0].X = 1;
            s1[0].Y = 0;
            for (var i = 1; i < s1.Length - 1; i++)
            {
                s1[i].X = Random.Shared.Next(1, 50000);
                s1[i].Y = Random.Shared.Next(1, 300);
            }
            s1[s1.Length - 1].X = 50000;
            s1[s1.Length - 1].Y = 0;

            spectraChart1.Series[0].DataElements = s1;

            var s2 = new (int X, int Y)[count];
            s2[0].X = 100;
            s2[0].Y = 0;
            for (var i = 1; i < s2.Length - 1; i++)
            {
                s2[i].X = Random.Shared.Next(100, 50000);
                s2[i].Y = Random.Shared.Next(1, 300);
            }
            s2[s2.Length - 1].X = 50000;
            s2[s2.Length - 1].Y = 0;

            spectraChart2.Series[0].DataElements = s2;

            screen.EndUpdate();

            await Task.Delay(500);
        }
    }

}
