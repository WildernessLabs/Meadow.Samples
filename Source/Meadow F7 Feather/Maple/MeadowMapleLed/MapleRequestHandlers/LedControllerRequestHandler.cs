using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleLed.Controllers;

namespace MeadowMapleLed.MapleRequestHandlers;

public class LedControllerRequestHandler : RequestHandlerBase
{
    public LedControllerRequestHandler() { }

    [HttpPost("/turnon")]
    public IActionResult TurnOn()
    {
        Resolver.Services.Get<LedController>().TurnOn();
        return new OkResult();
    }

    [HttpPost("/turnoff")]
    public IActionResult TurnOff()
    {
        Resolver.Services.Get<LedController>().TurnOff();
        return new OkResult();
    }

    [HttpPost("/startblink")]
    public IActionResult StartBlink()
    {
        Resolver.Services.Get<LedController>().StartBlink();
        return new OkResult();
    }

    [HttpPost("/startpulse")]
    public IActionResult StartPulse()
    {
        Resolver.Services.Get<LedController>().StartPulse();
        return new OkResult();
    }

    [HttpPost("/startrunningcolors")]
    public IActionResult StartRunningColors()
    {
        Resolver.Services.Get<LedController>().StartRunningColors();
        return new OkResult();
    }
}