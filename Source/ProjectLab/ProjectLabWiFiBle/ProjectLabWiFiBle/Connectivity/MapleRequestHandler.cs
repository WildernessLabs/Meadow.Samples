using CommonContracts.Models;
using Connectivity.Common.Models;
using Meadow;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowConnectedSample.Controllers;
using MeadowConnectedSample.Models;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Connectivity;

public class MapleRequestHandler : RequestHandlerBase, ICommandController
{
    private AtmosphericConditions atmosphericConditions;
    private LightConditions lightConditions;
    private MotionConditions motionConditions;

    public event EventHandler<bool> PairingValueSet = default!;
    public event EventHandler<bool> LedToggleValueSet = default!;
    public event EventHandler<bool> LedBlinkValueSet = default!;
    public event EventHandler<bool> LedPulseValueSet = default!;

    public MapleRequestHandler()
    {
        Resolver.Services.Add<ICommandController>(this);

        var sensorController = Resolver.Services.Get<SensorController>();
        sensorController.AtmosphericConditionsChanged += (s, e) => atmosphericConditions = e;
        sensorController.LightConditionsChanged += (s, e) => lightConditions = e;
        sensorController.MotionConditionsChanged += (s, e) => motionConditions = e;
    }

    [HttpPost("/toggle")]
    public async Task<IActionResult> Toggle()
    {
        LedToggleValueSet.Invoke(this, true);
        return new OkResult();
    }

    [HttpPost("/blink")]
    public async Task<IActionResult> Blink()
    {
        LedBlinkValueSet.Invoke(this, true);
        return new OkResult();
    }

    [HttpPost("/pulse")]
    public async Task<IActionResult> Pulse()
    {
        LedPulseValueSet.Invoke(this, true);
        return new OkResult();
    }

    [HttpGet("/getEnvironmentalData")]
    public IActionResult GetEnvironmentalData()
    {
        var data = new ClimateModel()
        {
            Temperature = $"{atmosphericConditions.Temperature.Celsius:N1}",
            Humidity = $"{atmosphericConditions.Humidity.Percent:N1}",
            Pressure = $"{atmosphericConditions.Pressure.Millibar:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getLightData")]
    public IActionResult GetLightData()
    {
        var data = new IlluminanceModel()
        {
            Illuminance = $"{lightConditions.Illuminance.Lux:N1}"
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }

    [HttpGet("/getMotionData")]
    public IActionResult GetMotionData()
    {
        var data = new MotionModel()
        {
            Acceleration3dX = $"{motionConditions.Acceleration3D.X.CentimetersPerSecondSquared:N2}",
            Acceleration3dY = $"{motionConditions.Acceleration3D.Y.CentimetersPerSecondSquared:N2}",
            Acceleration3dZ = $"{motionConditions.Acceleration3D.Z.CentimetersPerSecondSquared:N2}",
            AngularVelocity3dX = $"{motionConditions.AngularVelocity3D.X.DegreesPerSecond:N2}",
            AngularVelocity3dY = $"{motionConditions.AngularVelocity3D.Y.DegreesPerSecond:N2}",
            AngularVelocity3dZ = $"{motionConditions.AngularVelocity3D.Z.DegreesPerSecond:N2}",
        };

        Context.Response.ContentType = ContentTypes.Application_Json;
        return new JsonResult(data);
    }
}