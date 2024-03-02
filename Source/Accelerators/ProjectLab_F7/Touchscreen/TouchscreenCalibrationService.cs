using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

public class TouchscreenCalibrationService
{
    public event EventHandler<CalibrationPoint[]> CalibrationComplete;

    private readonly DisplayScreen _screen;
    private readonly Crosshair[] _calibrationPoints;
    private readonly ICalibratableTouchscreen _touchscreen;

    private int _currentPoint = 0;
    private CalibrationPoint[] _calPoints;
    private int _lastTouch = Environment.TickCount;
    private Label _instruction;

    public TouchscreenCalibrationService(DisplayScreen screen)
    {
        if (screen?.TouchScreen is ICalibratableTouchscreen cts)
        {
            _touchscreen = cts;
        }
        else
        {
            throw new ArgumentException("Touchscreen must be an ICalibratableTouchscreen");
        }

        var margin = 30;

        _screen = screen;
        _instruction = new Label(0, 0, screen.Width, screen.Height)
        {
            Font = new Font8x12(),
            Text = "Touch Cal Point",
            TextColor = Color.Yellow,
            HorizontalAlignment = Meadow.Foundation.Graphics.HorizontalAlignment.Center,
            VerticalAlignment = Meadow.Foundation.Graphics.VerticalAlignment.Center
        };

        _calibrationPoints = new Crosshair[]
        {
            new Crosshair(margin, margin) { ForeColor = Color.White },
            new Crosshair(_screen.Width - margin, _screen.Height - margin) { ForeColor = Color.White }
        };
        _calPoints = new CalibrationPoint[_calibrationPoints.Length];
    }

    public Task Calibrate()
    {
        _touchscreen.TouchUp += OnTouchUp;

        _screen.Controls.Clear();
        _screen.Controls.Add(_instruction);
        _screen.Controls.Add(_calibrationPoints[_currentPoint]);
        _screen.Invalidate();

        return Task.Run(async () =>
        {
            while (_currentPoint < _calibrationPoints.Length)
            {
                //                Resolver.Log.Info($"waiting for point {_currentPoint}");
                await Task.Delay(500);
            }
            _touchscreen.TouchUp -= OnTouchUp;
            Resolver.Log.Info($"calibration done");

            _touchscreen.SetCalibrationData(_calPoints);
            _screen.Controls.Clear();

            CalibrationComplete(this, _calPoints);
        });
    }

    private void OnTouchUp(ITouchScreen sender, TouchPoint point)
    {
        var now = Environment.TickCount;
        if (now - _lastTouch < 1000)
        {
            // ignore multiples (i.e. debounce)
            return;
        }
        _lastTouch = now;

        _calPoints[_currentPoint] = new CalibrationPoint(
            point.RawX,
            _calibrationPoints[_currentPoint].Left,
            point.RawY,
            _calibrationPoints[_currentPoint].Top);

        Resolver.Log.Info($"point {_currentPoint} captured ({point.RawX},{point.RawY})");

        _currentPoint++;

        if (_currentPoint < _calibrationPoints.Length)
        {
            Resolver.Log.Info($"Getting point {_currentPoint}");
            _screen.Controls.Clear();
            _screen.Controls.Add(_instruction);
            _screen.Controls.Add(_calibrationPoints[_currentPoint]);
        }
    }
}
