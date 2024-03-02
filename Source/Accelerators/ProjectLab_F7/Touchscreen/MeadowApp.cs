using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

public class MeadowApp : App<F7CoreComputeV2>
{
    private IProjectLabHardware _projectLab;
    private DisplayScreen _screen;
    private List<Circle> _circles = new();
    private Circle? _currentCircle;
    private Random _random = new();
    private Timer _inflateTimer;

    private int inflateRate = 20;
    private int inflateSize = 2;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");
        _projectLab = ProjectLab.Create();
        _screen = new DisplayScreen(
            _projectLab.Display,
            touchScreen: _projectLab.Touchscreen,
            rotation: Meadow.Peripherals.Displays.RotationType._270Degrees);

        Resolver.Log.Info($"Running on ProjectLab Hardware {_projectLab.RevisionString}");

        if (_projectLab.Touchscreen == null)
        {
            Resolver.Log.Error($"This demo requires a touchscreen, which is available of version hardware revision 3.f or later");
        }
        else
        {
            // TODO: retrieve stored calibration
            var ts = new TouchscreenCalibrationService(_screen);
            ts.CalibrationComplete += (s, e) =>
            {
                // TODO: save calibration data
            };

            await ts.Calibrate();
            _projectLab.Touchscreen.TouchDown += Touchscreen_TouchDown;
            _projectLab.Touchscreen.TouchUp += Touchscreen_TouchUp;
        }

        _inflateTimer = new Timer(InflateTimerproc);
    }

    private void InflateTimerproc(object o)
    {
        if (_currentCircle == null) return;
        _currentCircle.Radius += inflateSize;
        _inflateTimer.Change(inflateRate, -1);
    }

    private void Touchscreen_TouchUp(ITouchScreen sender, TouchPoint point)
    {
        _inflateTimer.Change(-1, -1);
        _currentCircle = null;
        Resolver.Log.Info($"touch up @ ({point.ScreenX}, {point.ScreenY})");
    }

    private Color GetRandomColor()
    {
        return Color.FromRgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
    }

    private void Touchscreen_TouchDown(ITouchScreen sender, TouchPoint point)
    {
        _currentCircle = new Circle(point.ScreenX, point.ScreenY, 10)
        {
            ForeColor = GetRandomColor(),
            IsFilled = true
        };
        Resolver.Log.Info($"touch down @ ({point.ScreenX}, {point.ScreenY})");
        _circles.Add(_currentCircle);
        _screen.Controls.Add(_currentCircle);
        _inflateTimer.Change(inflateRate, -1);
    }
}