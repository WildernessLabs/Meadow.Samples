using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;
using SimpleJpegDecoder;
using System.IO;
using System.Reflection;

namespace GalleryViewer.Controllers
{
    internal class DisplayController
    {
        readonly MicroGraphics graphics;
        readonly string[] images = new string[3]
        {
            "GalleryViewer.Resources.image1.jpg",
            "GalleryViewer.Resources.image2.jpg",
            "GalleryViewer.Resources.image3.jpg"
        };

        public DisplayController(IPixelDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true
            };
        }

        IPixelBuffer LoadJpeg(byte[] jpgData)
        {
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            return new BufferRgb888(decoder.Width, decoder.Height, jpg);
        }

        byte[] LoadResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public void DrawImage(int imageIndex)
        {
            var buffer = LoadJpeg(LoadResource(images[imageIndex]));
            graphics.DrawBuffer((graphics.Width - buffer.Width) / 2, 0, buffer);
            graphics.Show();
        }
    }
}