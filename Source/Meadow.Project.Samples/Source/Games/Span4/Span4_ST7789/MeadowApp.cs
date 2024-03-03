using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Span4
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        MicroGraphics graphics;

        IDigitalInputPort portLeft;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;
        IDigitalInputPort portReset;

        int ChipRadius = 11;
        int CellSize = 33;
        int CellOffset = 17;
        int yStart = 33;
        int xStart = 4;

        PiezoSpeaker speaker;

        Span4Game connectGame;

        byte currentColumn = 0;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            connectGame = new Span4Game();

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D02);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D03);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D04);
            portReset = Device.CreateDigitalInputPort(Device.Pins.D05);

            speaker = new PiezoSpeaker(Device.Pins.D06);

            var display = new St7789(
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: Device.Pins.D10,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            Console.WriteLine("Create graphics library");
            graphics = new MicroGraphics(display);
            graphics.CurrentFont = new Font12x16();

            return base.Initialize();
        }

        void StartGameLoop()
        {
            while (true)
            {
                CheckInput();

                graphics.Clear(false);
                DrawGame();
                graphics.Show();

                Thread.Sleep(150);
            }
        }

        void CheckInput()
        {
            if (portLeft.State == false)
            {
                if (currentColumn > 0)
                {
                    currentColumn -= 1;
                }
            }
            else if (portRight.State == false)
            {
                if (currentColumn < connectGame.Width - 1)
                {
                    currentColumn += 1;
                }
            }
            else if (portDown.State == false)
            {
                connectGame.AddChip(currentColumn);
                speaker?.PlayTone(new Frequency(440), TimeSpan.FromMilliseconds(200));

            }
            /*   else if (portReset.State == true)
               {
                   connectGame.Reset();
               } */
        }

        void DrawGame()
        {
            //draw gameboard
            graphics.DrawRectangle(xStart, yStart, CellSize * connectGame.Width + 1, CellSize * connectGame.Height + 1, Color.DarkBlue, true);
            graphics.DrawRectangle(xStart, yStart, CellSize * connectGame.Width + 1, CellSize * connectGame.Height + 1, Color.Blue, false);

            for (int i = 1; i < 7; i++)
            {
                graphics.DrawLine(xStart + CellSize * i,
                    yStart,
                    xStart + CellSize * i,
                    yStart + CellSize * 6 + 1,
                    Color.Blue);
            }

            for (int j = 1; j < 6; j++)
            {
                graphics.DrawLine(xStart,
                    yStart + j * CellSize,
                    CellSize * connectGame.Width + xStart,
                    yStart + j * CellSize,
                    Color.Blue);
            }

            for (int x = 0; x < connectGame.Width; x++)
            {
                for (int y = 0; y < connectGame.Height; y++)
                {
                    if (connectGame.GameField[x, y] == 0) { continue; }
                    DrawChipOnBoard(x, y, connectGame.GameField[x, y] == 1);
                    //DrawChipOnBoard(x, y, x % 2 == 0);
                }
            }

            //Game state
            switch (connectGame.GameState)
            {
                case Span4Game.GameStateType.Draw:
                    graphics.DrawText(2, 0, "Draw", Color.White);
                    break;
                case Span4Game.GameStateType.Player1Wins:
                    graphics.DrawText(2, 0, "Player 1 Wins!", Color.White);
                    break;
                case Span4Game.GameStateType.Player2Wins:
                    graphics.DrawText(2, 0, "Player 2 Wins!", Color.White);
                    break;
                case Span4Game.GameStateType.Player1Turn:
                    DrawPreviewChip(currentColumn, true);
                    break;
                case Span4Game.GameStateType.Player2Turn:
                    DrawPreviewChip(currentColumn, false);
                    break;
            }

            //Draw side display
            /*   int xText = 150;
               graphics.DrawText(xText, 0, "Span4!");

               graphics.DrawText(xText, 18, "Player 1");
               DrawChip(xText + 40, 21, 3, true);

               graphics.DrawText(xText, 27, "Player 2");
               DrawChip(xText + 40, 30, 3, false);

               graphics.DrawText(xText, 45, "Score:");
               graphics.DrawText(xText, 54, $"{connectGame.Player1Wins} to {connectGame.Player2Wins}");  */
        }

        void DrawPreviewChip(int column, bool isFilled)
        {
            DrawChip(xStart + column * CellSize + CellOffset,
                CellOffset,
                ChipRadius, isFilled);
        }

        void DrawChipOnBoard(int column, int row, bool isFilled)
        {
            DrawChip(xStart + column * CellSize + CellOffset,
                yStart + (connectGame.Height - row - 1) * CellSize + CellOffset,
                ChipRadius, isFilled);
        }
        void DrawChip(int xCenter, int yCenter, int radius, bool isFilled)
        {
            graphics.DrawCircle(xCenter, yCenter, radius,
                            isFilled ? Color.Red : Color.Yellow, true, true);
        }

        public override Task Run()
        {
            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Span4!");
            graphics.DrawText(0, 10, "v0.0.4");
            graphics.Show();

            Thread.Sleep(250);

            StartGameLoop();

            return base.Run();
        }
    }
}