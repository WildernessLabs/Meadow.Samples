using Meadow.Cloud;

namespace CloudSample;

/*
Sample JSON for pasing into MC UI
{
    "Data": 42
}
*/
public class SampleCommand : IMeadowCommand
{
    public int Data { get; set; }
}
