namespace MeadowApp

open Meadow
open Meadow.Devices
open System.Threading
open System.Threading.Tasks


type MeadowApp() =
    inherit App<F7FeatherV2>()  
    
    let outs = [MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedRed
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedGreen
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.OnboardLedBlue
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D00
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D01
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D02
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D03
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D04
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D05
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D06
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D07
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D08
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D09
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D10
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D11
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D12
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D13
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D14
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.D15
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A00
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A01
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A02
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A03
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A04
                MeadowApp.Device.CreateDigitalOutputPort MeadowApp.Device.Pins.A05
                ]

    let WalkOutputs() = 
        do outs |> List.iter (fun port -> 
                            port.State <- true
                            Thread.Sleep 250
                            port.State <- false
                            )

        do outs |> List.iter (fun port -> port.Dispose())

    override this.Initialize() =
        do Resolver.Log.Info "Initialize... (F#)"

        

        base.Initialize()
        
    override this.Run () : Task =
        let runAsync = async {
            do Resolver.Log.Info "Run... (F#)"
            do WalkOutputs()
        }
        Async.StartAsTask(runAsync) :> Task
