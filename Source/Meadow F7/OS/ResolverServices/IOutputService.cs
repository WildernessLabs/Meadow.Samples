using Meadow.Hardware;

namespace ResolverServices;

public interface IOutputService
{
    public IDigitalOutputPort OutputPort { get; }
}