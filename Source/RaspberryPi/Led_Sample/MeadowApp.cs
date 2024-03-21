using Meadow;
using Meadow.Foundation.Leds;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Led_Sample;

public class MeadowApp : App<RaspberryPi>
{
    private List<Led> leds;

    public override Task Initialize()
    {
        leds = new List<Led>
            {
                new Led(Device.Pins.GPIO2),
                new Led(Device.Pins.GPIO3),
                new Led(Device.Pins.GPIO4),
                new Led(Device.Pins.GPIO17),
                new Led(Device.Pins.GPIO27),
                new Led(Device.Pins.GPIO22),
                new Led(Device.Pins.GPIO10),
                new Led(Device.Pins.GPIO9),
                new Led(Device.Pins.GPIO11),
                new Led(Device.Pins.GPIO5),
                new Led(Device.Pins.GPIO6),
                new Led(Device.Pins.GPIO13),
                new Led(Device.Pins.GPIO19),
                new Led(Device.Pins.GPIO26),

                //new Led(Device.Pins.GPIO14), // Not Available
                //new Led(Device.Pins.GPIO15), // Not Available
                new Led(Device.Pins.GPIO18),
                new Led(Device.Pins.GPIO23),
                new Led(Device.Pins.GPIO24),
                new Led(Device.Pins.GPIO25),
                //new Led(Device.Pins.GPIO8), // Throws Exception
                //new Led(Device.Pins.GPIO7), // Throws Exception 
                new Led(Device.Pins.GPIO12),
                new Led(Device.Pins.GPIO16),
                new Led(Device.Pins.GPIO20),
                new Led(Device.Pins.GPIO21),
            };

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Error("Error level message");
        Resolver.Log.Warn("Warn level message");

        Resolver.Log.Info("TestLeds...");

        while (true)
        {
            Resolver.Log.Error("Turning on each led every 100ms");
            foreach (var led in leds)
            {
                led.IsOn = true;
                await Task.Delay(100);
            }

            await Task.Delay(1000);

            Resolver.Log.Info("Turning off each led every 100ms");
            foreach (var led in leds)
            {
                led.IsOn = false;
                await Task.Delay(100);
            }

            await Task.Delay(1000);

            //Resolver.Log.Info("Blinking the LEDs for a second each");
            //foreach (var led in leds)
            //{
            //    await led.StartBlink();
            //    await Task.Delay(3000);
            //    await led.StopAnimation();
            //}

            //Resolver.Log.Info("Blinking the LEDs for a second each with on (1s) and off (1s)");
            //foreach (var led in leds)
            //{
            //    await led.StartBlink(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //    await Task.Delay(3000);
            //    await led.StopAnimation();
            //}

            //await Task.Delay(3000);
        }
    }
}

/*
 * SAMPLE WIRING
 * 
      5V |
      5V |
Pi4  GND |----------(LED)-------------
     ... |                            |
      40 |-----[resistor]-------------
*/
