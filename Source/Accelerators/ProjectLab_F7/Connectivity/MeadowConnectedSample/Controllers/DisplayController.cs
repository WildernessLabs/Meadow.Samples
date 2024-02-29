using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controllers
{
    public class DisplayController
    {
        private static readonly Lazy<DisplayController> instance =
            new Lazy<DisplayController>(() => new DisplayController());
        public static DisplayController Instance => instance.Value;

        static Color backgroundColor = Color.FromHex("#23ABE3");

        CancellationTokenSource token;

        protected Image imgConnecting, imgConnected;
        protected MicroGraphics graphics;

        private DisplayController() { }

        public void Initialize(IPixelDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font12x20(),
                Stroke = 3,
            };

            graphics.Clear(true);
        }

        void DrawBackground()
        {
            var logo = Image.LoadFromResource("MeadowConnectedSample.Resources.img_meadow.bmp");

            graphics.Clear(backgroundColor);

            graphics.DrawImage(
                x: graphics.Width / 2 - logo.Width / 2,
                y: 34,
                image: logo);

            graphics.DrawCircle(
                centerX: graphics.Width / 2,
                centerY: graphics.Height / 2,
                radius: graphics.Width / 2 - 10,
                color: Color.Black,
                filled: false);
        }

        public void ShowSplashScreen()
        {
            DrawBackground();

            graphics.Show();
        }

        public async Task StartConnectingAnimation(bool isWiFi)
        {
            imgConnected = Image.LoadFromResource(isWiFi
                ? "MeadowConnectedSample.Resources.img_wifi_connected.bmp"
                : "MeadowConnectedSample.Resources.img_ble_paired.bmp");
            imgConnecting = Image.LoadFromResource(isWiFi
                ? "MeadowConnectedSample.Resources.img_wifi_connecting.bmp"
                : "MeadowConnectedSample.Resources.img_ble_pairing.bmp");

            token = new CancellationTokenSource();

            graphics.DrawRectangle(
                x: (graphics.Width - 140) / 2,
                y: 115,
                width: 140,
                height: 85,
                color: backgroundColor,
                filled: true);

            bool alternateImg = false;
            while (!token.IsCancellationRequested)
            {
                alternateImg = !alternateImg;

                graphics.DrawImage(
                    x: graphics.Width / 2 - imgConnecting.Width / 2,
                    y: 130,
                    image: alternateImg ? imgConnecting : imgConnected);
                graphics.Show();

                await Task.Delay(500);
            }
        }

        public void StopConnectingAnimation()
        {
            token.Cancel();
        }

        public void ShowMapleReady(string ipAddress)
        {
            graphics.DrawRectangle(
                x: (graphics.Width - 140) / 2,
                y: 120,
                width: 140,
                height: 85,
                color: backgroundColor,
                filled: true);

            graphics.CurrentFont = new Font12x16();
            graphics.DrawText(
                x: graphics.Width / 2,
                y: 128,
                text: "MAPLE",
                color: Color.Black,
                scaleFactor: ScaleFactor.X2,
                alignmentH: HorizontalAlignment.Center);

            graphics.DrawText(
                x: graphics.Width / 2,
                y: 171,
                text: $"{ipAddress}",
                color: Color.Black,
                scaleFactor: ScaleFactor.X1,
                alignmentH: HorizontalAlignment.Center);

            graphics.DrawText(
                x: graphics.Width / 2,
                y: 197,
                text: $"READY",
                color: Color.Black,
                scaleFactor: ScaleFactor.X1,
                alignmentH: HorizontalAlignment.Center);

            graphics.Show();
        }

        public void ShowBluetoothPaired()
        {
            StopConnectingAnimation();

            graphics.DrawRectangle(
                x: (graphics.Width - 140) / 2,
                y: 120,
                width: 140,
                height: 85,
                color: backgroundColor,
                filled: true);

            graphics.CurrentFont = new Font12x16();
            graphics.DrawText(
                x: graphics.Width / 2,
                y: 130,
                text: "BLUETOOTH",
                color: Color.Black,
                scaleFactor: ScaleFactor.X1,
                alignmentH: HorizontalAlignment.Center);

            graphics.DrawText(
                x: graphics.Width / 2,
                y: 160,
                text: "PAIRED",
                color: Color.Black,
                scaleFactor: ScaleFactor.X2,
                alignmentH: HorizontalAlignment.Center);

            graphics.Show();
        }
    }
}