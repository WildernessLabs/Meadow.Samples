using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using Meadow.Units;
using MeadowMapleServo.Controllers;

namespace MeadowMapleServo.MapleRequestHandlers;

public class ServoControllerRequestHandler : RequestHandlerBase
{
    public ServoControllerRequestHandler() { }

    [HttpPost("/rotateto")]
    public IActionResult RotateTo()
    {
        int angle = int.Parse(Body);
        Resolver.Services.Get<ServoController>().RotateTo(new Angle(angle));
        return new OkResult();
    }

    [HttpPost("/startsweep")]
    public IActionResult StartSweep()
    {
        Resolver.Services.Get<ServoController>().StartSweep();
        return new OkResult();
    }

    [HttpPost("/stopsweep")]
    public IActionResult StopSweep()
    {
        Resolver.Services.Get<ServoController>().StopSweep();
        return new OkResult();
    }
}