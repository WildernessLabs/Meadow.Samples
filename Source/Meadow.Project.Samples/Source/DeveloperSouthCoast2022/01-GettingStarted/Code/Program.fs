open System
open Meadow.Devices
open Meadow
open Meadow.Foundation.Leds
open Meadow.Foundation

type MeadowApp() =
    inherit App<F7Micro, MeadowApp>()

    do Console.WriteLine "Init with FSharp!"
    let led =
        new RgbPwmLed(MeadowApp.Device, MeadowApp.Device.Pins.OnboardLedRed, MeadowApp.Device.Pins.OnboardLedGreen,
                      MeadowApp.Device.Pins.OnboardLedBlue, 3.3f, 3.3f, 3.3f,
                      Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode)

    let ShowColorPulses color duration =
        led.StartPulse(color, (duration / 2)) |> ignore
        Threading.Thread.Sleep(int duration) |> ignore
        led.Stop |> ignore


    let CycleColors duration =
        while true do
            ShowColorPulses Color.Blue duration
            ShowColorPulses Color.Cyan duration
            ShowColorPulses Color.Green duration
            ShowColorPulses Color.GreenYellow duration
            ShowColorPulses Color.Yellow duration
            ShowColorPulses Color.Orange duration
            ShowColorPulses Color.OrangeRed duration
            ShowColorPulses Color.Red duration
            ShowColorPulses Color.MediumVioletRed duration
            ShowColorPulses Color.Purple duration
            ShowColorPulses Color.Magenta duration
            ShowColorPulses Color.Pink duration

    do CycleColors 1000

[<EntryPoint>]
let main argv =
    Console.WriteLine "Hello World from F#!"
    let app = new MeadowApp()
    Threading.Thread.Sleep(System.Threading.Timeout.Infinite)
    0 // return an integer exit code
