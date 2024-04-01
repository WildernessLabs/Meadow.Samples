using Meadow.Cloud;
using System;
using System.Collections.Generic;

namespace CloudSample;

public class TelemetryController
{
    private const int DataEventId = 2000;

    private Random random = new();
    private IMeadowCloudService cloudService;

    public TelemetryController(IMeadowCloudService cloudService)
    {
        this.cloudService = cloudService;
    }

    public void LogTelemetry()
    {
        var data = new Dictionary<string, object>
        {
            { "Int Value", random.Next(43) },
            { "String Value", BitConverter.ToString(BitConverter.GetBytes(random.NextDouble())) }
        };

        cloudService.SendEvent(DataEventId, "CloudSample Data", data);
    }
}
