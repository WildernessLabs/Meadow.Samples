using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace MathLineCharts;

public class MeadowApp : App<F7CoreComputeV2>
{
    private DisplayScreen _screen;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var projectLab = ProjectLab.Create();

        _screen = new DisplayScreen(projectLab.Display, RotationType._270Degrees);

        _screen.BackgroundColor = Color.AntiqueWhite;

        var chart1 = new LineChart(0, 0, _screen.Width, _screen.Height / 2)
        {
            BackgroundColor = Color.FromHex("111111"),
            ShowYAxisLabels = true
        };

        chart1.Series.Add(
            GetSineSeries(),
            GetCosineSeries(4, 4.2, 0));

        var chart2 = new LineChart(0, _screen.Height / 2, _screen.Width, _screen.Height / 2)
        {
            BackgroundColor = Color.FromHex("222222"),
            ShowYAxisLabels = true
        };

        chart2.Series.Add(
            GetSineSeries(2, 2),
            GetCosineSeries(4, 4.2, 4.5));

        _screen.Controls.Add(chart1, chart2);

        return base.Initialize();
    }

    private const int PointsPerSeries = 50;

    private LineChartSeries GetSineSeries(double xScale = 4, double yScale = 1.5, double yOffset = 1.5)
    {
        var series = new LineChartSeries()
        {
            LineColor = Color.Red,
            PointColor = Color.Green,
            LineStroke = 1,
            PointSize = 2,
            ShowLines = true,
            ShowPoints = true,
        };

        for (var p = 0; p < PointsPerSeries; p++)
        {
            series.Points.Add(p * 2, (Math.Sin(p / xScale) * yScale) + yOffset);
        }

        return series;
    }

    private LineChartSeries GetCosineSeries(double xScale = 4, double yScale = 1.5, double yOffset = 4.5)
    {
        var series = new LineChartSeries()
        {
            LineColor = Color.DarkBlue,
            PointColor = Color.DarkGreen,
            LineStroke = 2,
            PointSize = 3,
            ShowLines = true,
            ShowPoints = true,
        };

        for (var p = 0; p < PointsPerSeries; p++)
        {
            series.Points.Add(p * 2, (Math.Cos(p / xScale) * yScale) + yOffset);
        }

        return series;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        return base.Run();
    }
}