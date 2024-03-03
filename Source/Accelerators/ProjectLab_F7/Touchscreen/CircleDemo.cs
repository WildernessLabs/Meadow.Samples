using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Touchscreen_Demo;

public class CircleDemo
{
    private ITouchScreen _touchscreen;

    private DisplayScreen _display;
    private List<Circle> _circles = new();
    private Circle? _currentCircle;
    private Random _random = new();
    private Timer? _inflateTimer;

    private int inflateRate = 20;
    private int inflateSize = 2;

    public CircleDemo(DisplayScreen display, ITouchScreen touchscreen)
    {
        _touchscreen = touchscreen;
        _display = display;
    }

    public void Start()
    {
        if (_inflateTimer == null)
        {
            _touchscreen.TouchDown += Touchscreen_TouchDown;
            _touchscreen.TouchUp += Touchscreen_TouchUp;

            _inflateTimer = new Timer(InflateTimerproc);
        }
    }

    public void Stop()
    {
        if (_inflateTimer != null)
        {
            _touchscreen.TouchDown -= Touchscreen_TouchDown;
            _touchscreen.TouchUp -= Touchscreen_TouchUp;

            _inflateTimer.Dispose();
            _inflateTimer = null;
        }
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
        _circles.Add(_currentCircle);
        _display.Controls.Add(_currentCircle);
        _inflateTimer.Change(inflateRate, -1);
    }
}
