using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Logging;
using Meadow.Peripherals.Displays;

namespace MeadowApp
{
    public abstract class DisplayBase
    {
        private MicroGraphics _canvas;
        private RotationType _rotation;

        protected Logger Logger { get; private set; }
        protected abstract IPixelDisplay Display { get; }

        public DisplayBase(Logger logger, RotationType rotation = RotationType.Default)
        {
            Logger = logger;
            _rotation = rotation;
        }

        private void CheckCanvas()
        {
            if (_canvas == null)
            {
                _canvas = new MicroGraphics(Display);
                _canvas.Clear(true);
                _canvas.CurrentFont = new Font12x20();
                _canvas.Rotation = _rotation;
            }
        }

        public void ShowText(string text, int line = 1)
        {
            CheckCanvas();

            var lineheight = 20;

            var y = 5 + (line * lineheight);

            _canvas.DrawRectangle(0, y, _canvas.Width, 20, Color.Black, true);
            _canvas.DrawText(5, y, text, Color.White);
            _canvas.Show();
        }
    }
}