using Meadow.Foundation.Web.Maple;
using Meadow.Foundation.Web.Maple.Routing;
using Meadow.Units;
using MeadowMapleServo.Controllers;

namespace MeadowMapleServo.MapleRequestHandlers
{
    public class ServoControllerRequestHandler : RequestHandlerBase
    {
        public ServoControllerRequestHandler() { }

        [HttpPost("/rotateto")]
        public IActionResult RotateTo()
        {
            int angle = int.Parse(Body);
            ServoController.Instance.RotateTo(new Angle(angle, Angle.UnitType.Degrees));
            return new OkResult();
        }

        [HttpPost("/startsweep")]
        public IActionResult StartSweep()
         {
            ServoController.Instance.StartSweep();
            return new OkResult();
        }

        [HttpPost("/stopsweep")]
        public IActionResult StopSweep()
        {
            ServoController.Instance.StopSweep();
            return new OkResult();
        }
    }
}