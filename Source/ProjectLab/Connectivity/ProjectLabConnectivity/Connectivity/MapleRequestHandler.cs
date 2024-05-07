using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using ProjectLabConnectivity.Common.Models;
using ProjectLabConnectivity.Controllers;

namespace ProjectLabConnectivity.Connectivity;

public class MapleRequestHandler : RequestHandlerBase
{
    public MapleRequestHandler() { }

    [HttpPost("/toggle")]
    public IActionResult Toggle()
    {
        var commandController = Resolver.Services.Get<CommandController>();
        commandController.FireLedToggle();
        return new OkResult();
    }

    [HttpPost("/blink")]
    public IActionResult Blink()
    {
        var commandController = Resolver.Services.Get<CommandController>();
        commandController.FireLedBlink();
        return new OkResult();
    }

    [HttpPost("/pulse")]
    public IActionResult Pulse()
    {
        var commandController = Resolver.Services.Get<CommandController>();
        commandController.FireLedPulse();
        return new OkResult();
    }

    [HttpGet("/getEnvironmentalData")]
    public IActionResult GetEnvironmentalData()
    {
        var sensorController = Resolver.Services.Get<SensorController>();

        var data = new AtmosphericReadingsDTO()
        {
            Temperature = $"{sensorController.AtmosphericConditions.Temperature.Celsius:N1}",
            Humidity = $"{sensorController.AtmosphericConditions.Humidity.Percent:N1}",
            Pressure = $"{sensorController.AtmosphericConditions.Pressure.Millibar:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getLightData")]
    public IActionResult GetLightData()
    {
        var sensorController = Resolver.Services.Get<SensorController>();

        var data = new IlluminanceReadingsDTO()
        {
            Illuminance = $"{sensorController.LightConditions.Illuminance.Lux:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getMotionData")]
    public IActionResult GetMotionData()
    {
        var sensorController = Resolver.Services.Get<SensorController>();

        var data = new MotionReadingsDTO()
        {
            Acceleration3dX = $"{sensorController.MotionConditions.Acceleration3D.X.CentimetersPerSecondSquared:N2}",
            Acceleration3dY = $"{sensorController.MotionConditions.Acceleration3D.Y.CentimetersPerSecondSquared:N2}",
            Acceleration3dZ = $"{sensorController.MotionConditions.Acceleration3D.Z.CentimetersPerSecondSquared:N2}",
            AngularVelocity3dX = $"{sensorController.MotionConditions.AngularVelocity3D.X.DegreesPerSecond:N2}",
            AngularVelocity3dY = $"{sensorController.MotionConditions.AngularVelocity3D.Y.DegreesPerSecond:N2}",
            AngularVelocity3dZ = $"{sensorController.MotionConditions.AngularVelocity3D.Z.DegreesPerSecond:N2}",
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }
}