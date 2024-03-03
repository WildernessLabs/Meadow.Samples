using Meadow.Foundation.Displays;
using Meadow.Hardware;
using Meadow.Logging;
using Meadow.Peripherals.Displays;

namespace MeadowApp
{
    public class I2CDisplay : DisplayBase
    {
        private Ssd1306 _display;

        public I2CDisplay(II2cBus bus, Logger logger)
            : base(logger)
        {
            //_display = new Ssd1306(bus, Ssd1306.Addresses.Default, displayType: Ssd130xBase.DisplayType.OLED128x64);
            _display = new Ssd1306(bus, displayType: Ssd130xBase.DisplayType.OLED128x64);
        }

        protected override IPixelDisplay Display => _display;
    }
}
