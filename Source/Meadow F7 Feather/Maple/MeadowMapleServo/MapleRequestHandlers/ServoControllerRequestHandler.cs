using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleServo.Controllers;

namespace MeadowMapleServo.MapleRequestHandlers;

public class ServoControllerRequestHandler : RequestHandlerBase
{
    public ServoControllerRequestHandler() { }

    [HttpPost("/rotateto")]
    public IActionResult RotateTo()
    {
        int angle = int.Parse(Body);
        Resolver.Services.Get<CommandController>().FireServoRotateTo(angle);
        return new OkResult();
    }

    [HttpPost("/startsweep")]
    public IActionResult StartSweep()
    {
        Resolver.Services.Get<CommandController>().FireServoStartSweep();
        return new OkResult();
    }

    [HttpPost("/stopsweep")]
    public IActionResult StopSweep()
    {
        Resolver.Services.Get<CommandController>().FireServoStopSweep();
        return new OkResult();
    }
}