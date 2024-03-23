using CommonContracts.Models;
using Connectivity.Common.Models;
using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using MeadowConnectedSample.Controller;
using MeadowConnectedSample.Controllers;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Connectivity
{
    public class MapleRequestHandler : RequestHandlerBase
    {
        public MapleRequestHandler() { }

        [HttpPost("/toggle")]
        public async Task<IActionResult> Toggle()
        {
            await LedController.Instance.Toggle();
            return new OkResult();
        }

        [HttpPost("/blink")]
        public async Task<IActionResult> Blink()
        {
            await LedController.Instance.StartBlink();
            return new OkResult();
        }

        [HttpPost("/pulse")]
        public async Task<IActionResult> Pulse()
        {
            await LedController.Instance.StartPulse();
            return new OkResult();
        }

        [HttpGet("/getLightData")]
        public IActionResult GetLightData()
        {
            var reading = MainController.Instance.LightReading;
            var data = new IlluminanceModel()
            {
                Illuminance = $"{(int)reading?.Lux}"
            };

            Context.Response.ContentType = ContentTypes.Application_Json;
            return new JsonResult(data);
        }

        [HttpGet("/getMotionData")]
        public IActionResult GetMotionData()
        {
            var reading = MainController.Instance.MotionReading;
            var data = new MotionModel()
            {
                Acceleration3dX = $"{reading.acceleration3D.Value.X.CentimetersPerSecondSquared:N2}",
                Acceleration3dY = $"{reading.acceleration3D.Value.Y.CentimetersPerSecondSquared:N2}",
                Acceleration3dZ = $"{reading.acceleration3D.Value.Z.CentimetersPerSecondSquared:N2}",
                AngularVelocity3dX = $"{reading.angularVelocity3D.Value.X.DegreesPerSecond:N2}",
                AngularVelocity3dY = $"{reading.angularVelocity3D.Value.Y.DegreesPerSecond:N2}",
                AngularVelocity3dZ = $"{reading.angularVelocity3D.Value.Z.DegreesPerSecond:N2}",
                Temperature = $"{reading.temperature.Value.Celsius:N2}"
            };

            Context.Response.ContentType = ContentTypes.Application_Json;
            return new JsonResult(data);
        }

        [HttpGet("/getEnvironmentalData")]
        public IActionResult GetEnvironmentalData()
        {
            var reading = MainController.Instance.EnvironmentalReading;
            var data = new ClimateModel()
            {
                Temperature = $"{(int)reading.Temperature.Value.Celsius}",
                Humidity = $"{(int)reading.Humidity.Value.Percent}",
                Pressure = $"{(int)reading.Pressure.Value.Millibar}"
            };

            Context.Response.ContentType = ContentTypes.Application_Json;
            return new JsonResult(data);
        }
    }
}