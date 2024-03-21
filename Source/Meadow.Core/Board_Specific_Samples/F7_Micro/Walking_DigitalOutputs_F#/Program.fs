// Learn more about F# at http://fsharp.org

open Meadow
open Meadow.Devices
open System.Threading


type OutputApp() =
    inherit App<F7Micro, OutputApp>()  
    
    let outs = [OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.OnboardLedRed
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.OnboardLedGreen
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.OnboardLedBlue
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D00
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D01
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D02
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D03
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D04
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D05
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D06
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D07
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D08
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D09
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D10
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D11
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D12
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D13
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D14
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.D15
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A00
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A01
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A02
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A03
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A04
                OutputApp.Device.CreateDigitalOutputPort OutputApp.Device.Pins.A05
                ]
    // walk outputs
    do outs |> List.iter (fun port -> 
                            port.State <- true
                            Thread.Sleep 250
                            port.State <- false
                            )

    // dispose of ports
    do outs |> List.iter (fun port -> port.Dispose())
    
    


[<EntryPoint>]
let main argv =
    printfn "Hello from F#!"
    let app = new OutputApp()
    Thread.Sleep(Timeout.Infinite)
    0 // return an integer exit code
