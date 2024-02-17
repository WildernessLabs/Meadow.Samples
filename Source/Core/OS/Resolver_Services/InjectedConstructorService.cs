using Meadow;

namespace Threading_Basics
{
    public class InjectedConstructorService : IService
    {
        private IOutputService OutputService { get; }

        public void SetOutputState(bool state)
        {
            OutputService.OutputPort.State = state;
        }

        public InjectedConstructorService(IOutputService outputService)
        {
            Resolver.Log.Info($"InjectedConstructorService constructor has been called.");
            OutputService = outputService;
        }
    }
}