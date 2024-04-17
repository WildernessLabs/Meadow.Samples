using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleLed.Controller;

namespace MeadowMapleLed.MapleRequestHandlers;

public class LedControllerRequestHandler : RequestHandlerBase
{
    public LedControllerRequestHandler() { }

    [HttpPost("/turnon")]
    public IActionResult TurnOn()
    {
        LedController.Instance.TurnOn();
        return new OkResult();
    }

    [HttpPost("/turnoff")]
    public IActionResult TurnOff()
    {
        LedController.Instance.TurnOff();
        return new OkResult();
    }

    [HttpPost("/startblink")]
    public IActionResult StartBlink()
    {
        LedController.Instance.StartBlink();
        return new OkResult();
    }

    [HttpPost("/startpulse")]
    public IActionResult StartPulse()
    {
        LedController.Instance.StartPulse();
        return new OkResult();
    }

    [HttpPost("/startrunningcolors")]
    public IActionResult StartRunningColors()
    {
        LedController.Instance.StartRunningColors();
        return new OkResult();
    }
}