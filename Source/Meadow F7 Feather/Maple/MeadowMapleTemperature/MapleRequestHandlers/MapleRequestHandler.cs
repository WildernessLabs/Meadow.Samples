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
        var ledController = Resolver.Services.Get<LedController>();
        ledController.SetColor(Color.Cyan);

        var data = Resolver.Services.Get<TemperatureController>().TemperatureLogs;

        ledController.SetColor(Color.Green);
        return new JsonResult(data);
    }
}