using Eyeball.Core;
using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;

namespace EyaballJuego;

public class MeadowApp : App<F7CoreComputeV2>
{
    IJuegoHardware juego;

    EyeballController eyeballController;

    public override Task Initialize()
    {
        Console.WriteLine("Initialize...");

        juego = Juego.Create();

        eyeballController = new EyeballController(juego.Display);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Console.WriteLine("Run...");

        eyeballController.DrawEyeball();

        while (true)
        {
            eyeballController.Delay();
            eyeballController.RandomEyeMovement();
            eyeballController.Delay();
            eyeballController.CenterEye();
        }
    }
}