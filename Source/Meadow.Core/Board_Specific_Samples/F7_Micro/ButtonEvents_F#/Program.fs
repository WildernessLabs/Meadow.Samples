// Learn more about F# at http://fsharp.org

open Meadow
open Meadow.Devices
open Meadow.Hardware
open System.Threading


type ButtonEventsApp() =
    inherit App<F7Micro, ButtonEventsApp>()    
          
    let input = ButtonEventsApp.Device.CreateDigitalInputPort( ButtonEventsApp.Device.Pins.D02, InterruptMode.EdgeBoth, debounceDuration = (float 20))    
    do input.Changed.Add(fun e -> 
                            printfn "Changed %b Time:%s"  e.Value (e.New.ToString())
                        )


[<EntryPoint>]
let main argv =
    printfn "Hello from F#!"
    let app = new ButtonEventsApp()
    Thread.Sleep(Timeout.Infinite)
    0 // return an integer exit code
