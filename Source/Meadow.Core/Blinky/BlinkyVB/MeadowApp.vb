Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Peripherals.Leds

Public Class MeadowApp
    'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
    Inherits App(Of F7FeatherV2)

    Private onboardLed As RgbPwmLed

    Public Overrides Async Function Run() As Task
        Resolver.Log.Info("Run... (VB.NET)")

        Await CycleColors(TimeSpan.FromMilliseconds(1000))

    End Function

    Public Overrides Function Initialize() As Task
        Resolver.Log.Info("Initialize... (VB.NET)")

        onboardLed = New RgbPwmLed(
            Device.Pins.OnboardLedRed,
            Device.Pins.OnboardLedGreen,
            Device.Pins.OnboardLedBlue,
            CommonType.CommonAnode)

        Return MyBase.Run()
    End Function

    Private Async Function CycleColors(ByVal duration As TimeSpan) As Task
        Resolver.Log.Info("Cycle colors...")

        While True
            Await ShowColorPulse(Color.Blue, duration)
            Await ShowColorPulse(Color.Cyan, duration)
            Await ShowColorPulse(Color.Green, duration)
            Await ShowColorPulse(Color.GreenYellow, duration)
            Await ShowColorPulse(Color.Yellow, duration)
            Await ShowColorPulse(Color.Orange, duration)
            Await ShowColorPulse(Color.OrangeRed, duration)
            Await ShowColorPulse(Color.Red, duration)
            Await ShowColorPulse(Color.MediumVioletRed, duration)
            Await ShowColorPulse(Color.Purple, duration)
            Await ShowColorPulse(Color.Magenta, duration)
            Await ShowColorPulse(Color.Pink, duration)
        End While

    End Function

    Private Async Function ShowColorPulse(ByVal color As Color, ByVal duration As TimeSpan) As Task
        Await onboardLed.StartPulse(color, duration / 2)
        Await Task.Delay(duration)
    End Function

End Class