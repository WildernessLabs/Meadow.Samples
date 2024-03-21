using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace DigitalInterruptPort_Basics
{
    /// <summary>
    /// This sample illustrates using the IFilterableObserver pattern. To wire up, add
    /// a PushButton connected to D04, with the circuit terminating on the 3.3V rail, so that
    /// when the button is pressed, the input is raised high.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        IDigitalInterruptPort input;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initializing hardware...");

            //==== create an input port on D04. 
            input = Device.CreateDigitalInterruptPort(
                Device.Pins.D04,
                InterruptMode.EdgeBoth,
                ResistorMode.InternalPullDown);

            //==== Classic .NET Events
            input.Changed += (object sender, DigitalPortResult result) =>
            {
                Resolver.Log.Info($"Old school event raised; Time: {result.New.Time}, Value: {result.New.State}");
            };

            //===== Filterable Observer
            // this illustrates using a FilterableObserver. Note that the filter is an optional
            // parameter, if you're interested in all notifications, don't pass a filter/predicate.
            // in this case, we filter on events by time, and only notify if the new event is > 1 second from
            // the last event.
            var observer = IDigitalInterruptPort.CreateObserver(
                handler: result =>
                {
                    Resolver.Log.Info($"Observer filter satisfied, time: {result.New.Time}");
                },
                // Optional filter paramter, showing a 1 second filter, i.e., only notify
                // if the new event is > 1 second from last time it was notified.
                filter: result =>
                {
                    if (result.Old is { } old)
                    { // C# 8 null pattern matching for not null
                        //return (result.New.Time - old.Time) > TimeSpan.FromSeconds(1);
                        return (result.New.Time - old.Time) > TimeSpan.FromSeconds(1);
                    }
                    else return false;
                }
                // OR, for all events, use:
                // filter: null
                );
            input.Subscribe(observer);

            Resolver.Log.Info("Hardware initialized.");

            return Task.CompletedTask;
        }
    }
}