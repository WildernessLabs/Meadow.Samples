using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Logging;

namespace MeadowApp
{
    public class SPIDisplay : DisplayBase
    {
        private St7789 _display;
        private IPin _chipSelect;
        private IPin _dc;
        private IPin _reset;

        public SPIDisplay(ISpiBus bus, IPin chipSelect, IPin dc, IPin reset, Logger logger)
            : base(logger, RotationType._90Degrees)
        {
            _chipSelect = chipSelect;
            _dc = dc;
            _reset = reset;
            _display = new St7789(bus, _chipSelect, _dc, _reset, 240, 240);
        }

        protected override IGraphicsDisplay Display => _display;
    }
}