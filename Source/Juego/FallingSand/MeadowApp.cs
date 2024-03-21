using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors.Hid;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FallingSand
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        IJuegoHardware juego;
        MicroGraphics graphics;

        Random rand;

        //adjust rows, columns, and size together to fit the screen
        static readonly int NUM_COLUMNS = 80;
        static readonly int NUM_ROWS = 60;
        readonly int PARTICLE_SIZE = 4;

        //values for particles used for color
        const byte EMPTY = 0;
        const byte SAND = 1;
        const byte DIRT = 2;
        const byte SNOW = 3;
        const byte GRAVEL = 4;
        const byte USED = 255;

        readonly double ACCELEROMETER_SENSITIVITY = 0.4;

        //we use two boards, one to read current particle positions, one to write new positions
        readonly byte[] board1 = new byte[NUM_COLUMNS * NUM_ROWS];
        readonly byte[] board2 = new byte[NUM_COLUMNS * NUM_ROWS];

        //we switch between the two boards each frame
        bool useBoard1 = true;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            rand = new Random();

            juego = Juego.Create();

            graphics = new MicroGraphics(juego.Display);

            ClearBoards();

            juego.MotionSensor.StartUpdating(TimeSpan.FromMilliseconds(200));

            Console.WriteLine("Initialize complete");

            return DrawSlashScreen();
        }

        public override Task Run()
        {
            graphics.Clear(true);
            byte[] bCurrent, bTarget;
            int sourceIndex;
            int targetIndex;

            bool iterateForwards = true;

            while (true)
            {
                //assign the current current and target boards
                bCurrent = useBoard1 ? board1 : board2;
                bTarget = useBoard1 ? board2 : board1;

                //clear the target board
                Array.Clear(array: bTarget, index: 0, length: bTarget.Length);

                iterateForwards = false;
                var direction = GetPositionFromAccelerometer();
                if (direction == DigitalJoystickPosition.UpLeft ||
                    direction == DigitalJoystickPosition.UpRight ||
                    direction == DigitalJoystickPosition.Up ||
                    direction == DigitalJoystickPosition.Left)
                {
                    iterateForwards = true;
                }

                for (int i = 0; i < board1.Length; i++)
                {
                    sourceIndex = iterateForwards ? i : board1.Length - 1 - i;

                    if (IsParticle(bCurrent[sourceIndex]) != true) continue;

                    targetIndex = GetNewIndexForParticle(bCurrent, sourceIndex, direction);
                    bTarget[targetIndex] = bCurrent[sourceIndex];
                    bCurrent[sourceIndex] = 0;
                    bCurrent[targetIndex] = USED;
                }

                //switch the boards
                useBoard1 = !useBoard1;

                CheckButtonsAndAddParticle();

                graphics.Clear();
                DrawBoard();
                graphics.Show();

                Thread.Sleep(0);
            }
        }

        Task DrawSlashScreen()
        {
            graphics.CurrentFont = new Font8x12();
            graphics.Clear();

            graphics.DrawText(graphics.Width / 2, 50, "Falling Sand v1.0", Color.White, alignmentH: HorizontalAlignment.Center);

            graphics.DrawText(graphics.Width / 2, 130, "Move Juego to change gravity", Color.White, alignmentH: HorizontalAlignment.Center);
            graphics.DrawText(graphics.Width / 2, 100, "Press left buttons to add prticles", Color.White, alignmentH: HorizontalAlignment.Center);
            graphics.DrawText(graphics.Width / 2, 160, "Press start to reset", Color.White, alignmentH: HorizontalAlignment.Center);

            graphics.Show();

            return Task.Delay(3000);
        }

        void CheckButtonsAndAddParticle()
        {
            if (juego.StartButton.State == true)
                ClearBoards();
            if (juego.Left_UpButton.State == true)
                AddParticle(NUM_COLUMNS / 2, 0, SAND);
            if (juego.Left_DownButton.State == true)
                AddParticle(NUM_COLUMNS / 2, NUM_ROWS - 1, DIRT);
            if (juego.Left_LeftButton.State == true)
                AddParticle(0, NUM_ROWS / 2, SNOW);
            if (juego.Left_RightButton.State == true)
                AddParticle(NUM_COLUMNS - 1, NUM_ROWS / 2, GRAVEL);
        }

        DigitalJoystickPosition GetPositionFromAccelerometer()
        {
            if (juego.MotionSensor.Acceleration3D == null)
            {
                return DigitalJoystickPosition.Center;
            }

            var accel = juego.MotionSensor.Acceleration3D.Value;

            if (accel.X.Gravity < 0 - ACCELEROMETER_SENSITIVITY)
            {
                if (accel.Y.Gravity > ACCELEROMETER_SENSITIVITY)
                    return DigitalJoystickPosition.UpLeft;
                else if (accel.Y.Gravity < 0 - ACCELEROMETER_SENSITIVITY)
                    return DigitalJoystickPosition.DownLeft;
                else
                    return DigitalJoystickPosition.Left;
            }
            else if (accel.X.Gravity > ACCELEROMETER_SENSITIVITY)
            {
                if (accel.Y.Gravity > ACCELEROMETER_SENSITIVITY)
                    return DigitalJoystickPosition.UpRight;
                else if (accel.Y.Gravity < 0 - ACCELEROMETER_SENSITIVITY)
                    return DigitalJoystickPosition.DownRight;
                else
                    return DigitalJoystickPosition.Right;
            }
            else if (accel.Y.Gravity > ACCELEROMETER_SENSITIVITY)
            {
                return DigitalJoystickPosition.Up;
            }
            else if (accel.Y.Gravity < 0 - ACCELEROMETER_SENSITIVITY)
            {
                return DigitalJoystickPosition.Down;
            }
            else
            {
                return DigitalJoystickPosition.Center;
            }
        }

        int GetNewIndexForDirection(byte[] board, int index,
            DigitalJoystickPosition direction,
            DigitalJoystickPosition directionAlt1,
            DigitalJoystickPosition directionAlt2)
        {
            if (CanMoveParticle(board, index, direction))
            {
                return GetRawIndexForDirection(index, direction);
            }
            else
            {
                if (rand.Next(2) == 0)
                {
                    if (CanMoveParticle(board, index, directionAlt1))
                        return GetRawIndexForDirection(index, directionAlt1);
                    else if (CanMoveParticle(board, index, directionAlt2))
                        return GetRawIndexForDirection(index, directionAlt2);
                }
                else
                {
                    if (CanMoveParticle(board, index, directionAlt2))
                        return GetRawIndexForDirection(index, directionAlt2);
                    else if (CanMoveParticle(board, index, directionAlt1))
                        return GetRawIndexForDirection(index, directionAlt1);
                }
            }
            return index;
        }

        int GetNewIndexForParticle(byte[] board, int index, DigitalJoystickPosition gravityDirection)
        {
            return gravityDirection switch
            {
                DigitalJoystickPosition.Up => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Up, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.UpRight),
                DigitalJoystickPosition.UpLeft => GetNewIndexForDirection(board, index, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.Up, DigitalJoystickPosition.Left),
                DigitalJoystickPosition.UpRight => GetNewIndexForDirection(board, index, DigitalJoystickPosition.UpRight, DigitalJoystickPosition.Up, DigitalJoystickPosition.Right),
                DigitalJoystickPosition.Down => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Down, DigitalJoystickPosition.DownLeft, DigitalJoystickPosition.DownRight),
                DigitalJoystickPosition.DownLeft => GetNewIndexForDirection(board, index, DigitalJoystickPosition.DownLeft, DigitalJoystickPosition.Down, DigitalJoystickPosition.Left),
                DigitalJoystickPosition.DownRight => GetNewIndexForDirection(board, index, DigitalJoystickPosition.DownRight, DigitalJoystickPosition.Down, DigitalJoystickPosition.Right),
                DigitalJoystickPosition.Left => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Left, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.DownLeft),
                DigitalJoystickPosition.Right => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Right, DigitalJoystickPosition.UpRight, DigitalJoystickPosition.DownRight),
                _ => index,
            };
        }
        int GetRawIndexForDirection(int index, DigitalJoystickPosition position)
        {
            return position switch
            {
                DigitalJoystickPosition.Up => index - NUM_COLUMNS,
                DigitalJoystickPosition.UpRight => index - NUM_COLUMNS + 1,
                DigitalJoystickPosition.Right => index + 1,
                DigitalJoystickPosition.DownRight => index + NUM_COLUMNS + 1,
                DigitalJoystickPosition.Down => index + NUM_COLUMNS,
                DigitalJoystickPosition.DownLeft => index + NUM_COLUMNS - 1,
                DigitalJoystickPosition.Left => index - 1,
                DigitalJoystickPosition.UpLeft => index - NUM_COLUMNS - 1,
                _ => index,
            };
        }

        bool IsOnEdgeForDirection(int index, DigitalJoystickPosition position)
        {
            return position switch
            {
                DigitalJoystickPosition.Up => IsTopEdge(index),
                DigitalJoystickPosition.UpRight => IsTopEdge(index) || IsRightEdge(index),
                DigitalJoystickPosition.Right => IsRightEdge(index),
                DigitalJoystickPosition.DownRight => IsBottomEdge(index) || IsRightEdge(index),
                DigitalJoystickPosition.Down => IsBottomEdge(index),
                DigitalJoystickPosition.DownLeft => IsBottomEdge(index) || IsLeftEdge(index),
                DigitalJoystickPosition.Left => IsLeftEdge(index),
                DigitalJoystickPosition.UpLeft => IsTopEdge(index) || IsLeftEdge(index),
                _ => false,
            };
        }

        bool CanMoveParticle(byte[] board, int index, DigitalJoystickPosition position)
        {
            if (IsOnEdgeForDirection(index, position)) return false;
            if (board[GetRawIndexForDirection(index, position)] == EMPTY) return true;
            return false;
        }

        bool IsParticle(byte value)
        {
            if (value == SAND || value == SNOW || value == DIRT || value == GRAVEL) return true;
            return false;
        }

        bool IsLeftEdge(int index) => index % NUM_COLUMNS == 0;
        bool IsRightEdge(int index) => index % NUM_COLUMNS == NUM_COLUMNS - 1;
        bool IsTopEdge(int index) => index / NUM_COLUMNS == 0;
        bool IsBottomEdge(int index) => index / NUM_COLUMNS == NUM_ROWS - 1;

        public void AddParticle(int x, int y, byte value)
        {
            var board = useBoard1 ? board1 : board2;

            //add jitter to the particle position
            if (x == 0 || x == NUM_COLUMNS - 1)
                y += rand.Next(-1, 2);
            else
                x += rand.Next(-1, 2);

            board[x + y * NUM_COLUMNS] = value;
        }

        void ClearBoards()
        {
            Array.Clear(array: board1, index: 0, length: board1.Length);
        }

        void DrawBoard()
        {
            var board = useBoard1 ? board1 : board2;

            Color particleColor;

            for (int i = 0; i < board1.Length; i++)
            {
                if (IsParticle(board[i]) == true)
                {
                    int x = i % NUM_COLUMNS;
                    int y = i / NUM_COLUMNS;

                    particleColor = board[i] switch
                    {
                        SNOW => WildernessLabsColors.GalleryWhite,
                        DIRT => WildernessLabsColors.MetallicBronze,
                        GRAVEL => WildernessLabsColors.DustyGray,
                        _ => WildernessLabsColors.Sandrift,
                    };
                    graphics.DrawRectangle(x * PARTICLE_SIZE, y * PARTICLE_SIZE, PARTICLE_SIZE, PARTICLE_SIZE, particleColor, true);
                }
            }
        }
    }
}