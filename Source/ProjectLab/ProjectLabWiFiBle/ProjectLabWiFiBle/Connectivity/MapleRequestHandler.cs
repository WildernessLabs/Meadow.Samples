using CommonContracts.Models;
using Connectivity.Common.Models;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowConnectedSample.Controllers;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Connectivity;

public class MapleRequestHandler : RequestHandlerBase
{
    public MapleRequestHandler() { }

    [HttpPost("/toggle")]
    public async Task<IActionResult> Toggle()
    {
        //await LedController.Instance.Toggle();
        return new OkResult();
    }

    [HttpPost("/blink")]
    public async Task<IActionResult> Blink()
    {
        //await LedController.Instance.StartBlink();
        return new OkResult();
    }

    [HttpPost("/pulse")]
    public async Task<IActionResult> Pulse()
    {
        //await LedController.Instance.StartPulse();
        return new OkResult();
    }

    [HttpGet("/getEnvironmentalData")]
    public IActionResult GetEnvironmentalData()
    {
        var data = new ClimateModel()
        {
            Temperature = $"{AtmosphericConditions.Temperature.Celsius:N1}",
            Humidity = $"{AtmosphericConditions.Humidity.Percent:N1}",
            Pressure = $"{AtmosphericConditions.Pressure.Millibar:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getLightData")]
    public IActionResult GetLightData()
    {
        var data = new IlluminanceModel()
        {
            Illuminance = $"{LightConditions.Illuminance.Lux:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getMotionData")]
    public IActionResult GetMotionData()
    {
        var data = new MotionModel()
        {
            Acceleration3dX = $"{MotionConditions.Acceleration3D.X.CentimetersPerSecondSquared:N2}",
            Acceleration3dY = $"{MotionConditions.Acceleration3D.Y.CentimetersPerSecondSquared:N2}",
            Acceleration3dZ = $"{MotionConditions.Acceleration3D.Z.CentimetersPerSecondSquared:N2}",
            AngularVelocity3dX = $"{MotionConditions.AngularVelocity3D.X.DegreesPerSecond:N2}",
            AngularVelocity3dY = $"{MotionConditions.AngularVelocity3D.Y.DegreesPerSecond:N2}",
            AngularVelocity3dZ = $"{MotionConditions.AngularVelocity3D.Z.DegreesPerSecond:N2}",
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }
}