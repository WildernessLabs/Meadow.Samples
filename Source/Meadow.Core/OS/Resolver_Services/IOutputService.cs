using Meadow.Hardware;

namespace Threading_Basics
{
    public interface IOutputService
    {
        public IDigitalOutputPort OutputPort { get; }
    }
}