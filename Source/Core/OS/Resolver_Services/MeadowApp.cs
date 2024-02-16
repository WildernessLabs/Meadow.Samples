using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace Threading_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private bool _useMock = false; // change this value to either output to the red LED or to a console message
        private bool _useConstructorInjection = true; // change this value to how injection can either use the contrsuctor or a public settable property

        public override Task Initialize()
        {
            // manually create an object and put it into the DI container
            var outputService = new FeatherV2OutputService(Device);
            // register it as the interface, not concrete type
            Resolver.Services.Add<IOutputService>(outputService);

            // this check could be from an app config setting, or even a digital input
            if (_useMock)
            {
                // let the resolver create a service for us
                // the created instance of MockService will get registered as IService
                Resolver.Services.Create<MockService, IService>();
            }
            else
            {
                // let the resolver create a service for us - this will inject the IOutputService in the constructor for us
                // the created instance of MyBasicService will get registered as IService
                if (_useConstructorInjection)
                {
                    Resolver.Services.Create<InjectedConstructorService, IService>();
                }
                else
                {
                    Resolver.Services.Create<InjectedPropertyService, IService>();
                }

            }

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var state = true;

            // retrieve the service by registered type
            var service = Resolver.Services.Get<IService>();

            while (true)
            {
                service.SetOutputState(state);
                state = !state;
                await Task.Delay(1000);
            }
        }
    }
}