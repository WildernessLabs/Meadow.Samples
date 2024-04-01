using Meadow;
using Meadow.Cloud;

namespace CloudSample;

public class CommandController
{
    public CommandController(ICommandService commandService)
    {
        commandService.Subscribe<SampleCommand>(OnSampleCommandRecevied);
    }

    private void OnSampleCommandRecevied(SampleCommand command)
    {
        Resolver.Log.Info($"Command received: Data = {command.Data}");
    }
}
