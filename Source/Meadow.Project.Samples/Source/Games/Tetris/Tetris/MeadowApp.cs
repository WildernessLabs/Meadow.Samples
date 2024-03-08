using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        int tick = 0;
        MicroGraphics graphics;

        IDigitalInputPort portLeft;
        IDigitalInputPort portUp;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;

        TetrisGame game;

        int BLOCK_SIZE = 6;

        public override Task Initialize()
        {
            Console.WriteLine("Tetris");

            game = new TetrisGame(8, 18);

            var config = new SpiClockConfiguration(
                speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
                mode: SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                clock: Device.Pins.SCK,
                copi: Device.Pins.MOSI,
                cipo: Device.Pins.MISO,
                config: config);
            var display = new Ssd1309
            (
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            graphics = new MicroGraphics(display);
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = RotationType._270Degrees;

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D12);
            portUp = Device.CreateDigitalInputPort(Device.Pins.D13);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D07);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D11);

            return base.Initialize();
        }

        void StartGameLoop()
        {
            while (true)
            {
                tick++;
                CheckInput(tick);

                graphics.Clear();
                DrawTetrisField();
                graphics.Show();

                Thread.Sleep(50);
            }
        }

        void CheckInput(int tick)
        {
            if (tick % (21 - game.Level) == 0)
            {
                game.OnDown(true);
            }

            if (portLeft.State == true)
            {
                game.OnLeft();
            }
            else if (portRight.State == true)
            {
                game.OnRight();
            }
            else if (portUp.State == true)
            {
                game.OnRotate();
            }
            else if (portDown.State == true)
            {
                game.OnDown();
            }
        }

        void DrawTetrisField()
        {
            int xIndent = 8;
            int yIndent = 12;

            graphics.DrawText(xIndent, 0, $"Lines: {game.LinesCleared}");

            graphics.DrawRectangle(6, 10, 52, 112);

            //draw current piece
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (game.IsPieceLocationSet(i, j))
                    {
                        //  graphics.DrawPixel(i, j);
                        graphics.DrawRectangle((game.CurrentPiece.X + i) * BLOCK_SIZE + xIndent,
                            (game.CurrentPiece.Y + j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE + 1, BLOCK_SIZE, true, true);//+1 hack until we fix the graphics lib
                    }
                }
            }

            //draw gamefield
            for (int i = 0; i < game.Width; i++)
            {
                for (int j = 0; j < game.Height; j++)
                {
                    if (game.IsGameFieldSet(i, j))
                    {
                        graphics.DrawRectangle((i) * BLOCK_SIZE + xIndent,
                            (j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE + 1, BLOCK_SIZE, true, true);//+1 hack until we fix the graphics lib
                    }
                }
            }
        }

        public override Task Run()
        {
            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Tetris");
            graphics.DrawText(0, 10, "v0.0.2");
            graphics.Show();

            Thread.Sleep(1000);

            StartGameLoop();

            return base.Run();
        }
    }
}