using Meadow;
using Meadow.Foundation.ICs.ADC;
using System;
using System.Threading.Tasks;

namespace Ads1015_Sample;

public class MeadowApp : LinuxApp<RaspberryPi>
{
    private Ads1015 _adc;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize hardware...");
        _adc = new Ads1015(
            Device.CreateI2cBus(1, Meadow.Hardware.I2cBusSpeed.FastPlus),
            Ads1015.Addresses.Default,
            Ads1015.MeasureMode.Continuous,
            Ads1015.ChannelSetting.A0SingleEnded,
            Ads1015.SampleRateSetting.Sps3300);

        _adc.Gain = Ads1015.FsrGain.TwoThirds;

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        await TestSpeed();
        await TakeMeasurements();
    }

    async Task TestSpeed()
    {
        var totalSamples = 1000;

        var start = Environment.TickCount;
        long sum = 0;

        for (var i = 0; i < totalSamples; i++)
        {
            sum += await _adc.ReadRaw();
        }

        var end = Environment.TickCount;

        var mean = sum / (double)totalSamples;
        Resolver.Log.Info($"{totalSamples} reads in {end - start} ticks gave a raw mean of {mean:0.00}");
    }

    async Task TakeMeasurements()
    {
        var i = 0;

        while (true)
        {
            try
            {
                var value = await _adc.Read();
                Resolver.Log.Info($"ADC Reading {++i}: {value.Volts}V");
            }
            catch (Exception ex)
            {
                Resolver.Log.Info(ex.ToString());
            }
            await Task.Delay(5000);
        }
    }
}