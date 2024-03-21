// Learn more about F# at http://fsharp.org

open Meadow
open Meadow.Devices
open System.Threading

type MeadowApp() =
    inherit App<F7Micro, MeadowApp>()
    do printfn "Hello from F#"
    let redLED = MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedRed
    let blueLED = MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedBlue
    let greenLED = MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedGreen

    let rec showLights state = 
        let newState = not state
        printfn"State is %b" state
        redLED.State <- newState
        Thread.Sleep 200
        greenLED.State <- newState
        Thread.Sleep 200
        blueLED.State <- newState
        Thread.Sleep 200
        showLights newState
    do showLights false

[<EntryPoint>]
let main argv =    
    let app = new MeadowApp()
    0
    
