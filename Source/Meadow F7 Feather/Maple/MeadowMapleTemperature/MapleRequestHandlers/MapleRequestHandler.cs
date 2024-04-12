using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowMapleTemperature.Controllers;

namespace MeadowMapleTemperature;

public class MapleRequestHandler : RequestHandlerBase
{
    public MapleRequestHandler() { }

    [HttpGet("/gettemperaturelogs")]
    public IActionResult GetTemperatureLogs()
    {
        LedController.Instance.SetColor(Color.Cyan);

        var data = TemperatureController.Instance.TemperatureLogs;

        LedController.Instance.SetColor(Color.Green);
        return new JsonResult(data);
    }
}