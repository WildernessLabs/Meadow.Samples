using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;

namespace PWM
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IPwmPort pwm04;
        IDigitalOutputPort d03;

        public override Task Initialize()
        {
            pwm04 = Device.CreatePwmPort(Device.Pins.D04, new Meadow.Units.Frequency(500), 0.5f);

            d03 = Device.CreateDigitalOutputPort(Device.Pins.D03);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            try
            {
                await Task.Run(() =>
                {
                    var c = 0;

                    while (true)
                    {
                        d03.State = !d03.State;
                        Thread.Sleep(1000);
                    }
                });

                await Task.Delay(5000);

                pwm04.Start();
            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"Error: {ex.Message}");
            }
        }

        private void MultiplePwms()
        {
            var pwmA = Device.CreatePwmPort(Device.Pins.D11, new Frequency(100), 0.5f);
            var pwmB = Device.CreatePwmPort(Device.Pins.D12, new Frequency(200), 0.5f);
            var pwmC = Device.CreatePwmPort(Device.Pins.D13, new Frequency(400), 0.25f);

            pwmA.Start();
            pwmB.Start();
            pwmC.Start();
        }

        async Task TimeScaleChecks(IPwmPort pwm)
        {
            var delta = 100;

            pwm.Frequency = new Meadow.Units.Frequency(50f);

            pwm.Start();
            while (true)
            {
                pwm.TimeScale = TimeScale.Seconds;
                pwm.Period = 0.02f;
                Resolver.Log.Info($"Freq: {pwm.Frequency.Hertz}  Period: {(int)pwm.Period} {pwm.TimeScale}");
                await Task.Delay(2000);

                pwm.TimeScale = TimeScale.Milliseconds;
                Resolver.Log.Info($"Freq: {pwm.Frequency.Hertz}  Period: {(int)pwm.Period} {pwm.TimeScale}");
                await Task.Delay(2000);
                pwm.Period = 50f;
                Resolver.Log.Info($"Freq: {pwm.Frequency.Hertz}  Period: {(int)pwm.Period} {pwm.TimeScale}");
                await Task.Delay(2000);

                pwm.TimeScale = TimeScale.Microseconds;
                Resolver.Log.Info($"Freq: {pwm.Frequency.Hertz}  Period: {(int)pwm.Period} {pwm.TimeScale}");
                pwm.Period = 80f;
                Resolver.Log.Info($"Freq: {pwm.Frequency.Hertz}  Period: {(int)pwm.Period} {pwm.TimeScale}");
                await Task.Delay(2000);
            }
        }

        async Task FrequencyChecks(IPwmPort pwm)
        {
            Frequency delta = new Frequency(100);

            pwm.Start();

            while (true)
            {
                Resolver.Log.Info($"Freq: {pwm.Frequency}  Period: {pwm.Period} {pwm.TimeScale}");
                await Task.Delay(5000);

                pwm.Frequency += delta;
                if (pwm.Frequency <= new Frequency(100) || pwm.Frequency >= new Frequency(1000))
                {
                    delta *= -1;
                }
            }
        }

        async Task DutyCycleChecks(IPwmPort pwm)
        {
            var delta = 0.10000f;

            pwm.Start();

            while (true)
            {
                Resolver.Log.Info($"Duty: {pwm.DutyCycle}  Duration: {pwm.Duration} {pwm.TimeScale}");
                await Task.Delay(2000);

                var temp = Math.Round(pwm.DutyCycle + delta, 1);
                pwm.DutyCycle = (float)temp;

                if (pwm.DutyCycle <= .00 || pwm.DutyCycle >= 1.0)
                {
                    delta *= -1;
                }
            }
        }

        async Task DurationChecks(IPwmPort pwm)
        {
            var delta = 1f;
            pwm.TimeScale = TimeScale.Milliseconds;

            pwm.Start();
            while (true)
            {
                Resolver.Log.Info($"Duty: {pwm.DutyCycle}  Duration: {pwm.Duration} {pwm.TimeScale}");
                await Task.Delay(2000);

                var temp = Math.Round(pwm.Duration + delta, 0);
                pwm.Duration = (float)temp;

                if (pwm.Duration <= 000 || pwm.Duration >= 10.0)
                {
                    delta *= -1;
                }
            }
        }
    }
}