using Meadow;

namespace Threading_Basics
{
    public class MockService : IService
    {
        public MockService()
        {
            Resolver.Log.Info($"MockService constructor has been called.");
        }

        public void SetOutputState(bool state)
        {
            Resolver.Log.Info($"SET OUTPUT TO {state}");
        }
    }
}