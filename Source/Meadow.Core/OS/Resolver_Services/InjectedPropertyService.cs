using Meadow;

namespace Threading_Basics
{
    public class InjectedPropertyService : IService
    {
        public IOutputService OutputService { get; set; }

        public void SetOutputState(bool state)
        {
            OutputService.OutputPort.State = state;
        }

        public InjectedPropertyService()
        {
            Resolver.Log.Info($"InjectedPropertyService constructor has been called.");
        }
    }
}