using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using System;
using System.Threading.Tasks;

namespace GameOfLife
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        IJuegoHardware juego;
        MicroGraphics graphics;

        readonly Random rand = new Random();

        readonly int NUMBER_OF_GENERATIONS = 1000;
        static readonly int NUM_COLUMNS = 160;
        static readonly int NUM_ROWS = 120;
        readonly int SIZE = 2;

        readonly byte[,] currentBoard = new byte[NUM_COLUMNS, NUM_ROWS];
        readonly byte[,] nextBoard = new byte[NUM_COLUMNS, NUM_ROWS];

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            juego = Juego.Create();

            graphics = new MicroGraphics(juego.Display);

            InitBoard();

            Console.WriteLine("Initialize complete");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            graphics.Clear();

            for (int i = 0; i < NUMBER_OF_GENERATIONS; i++)
            {
                ComputeCA();
                DrawBoard();

                for (int x = 1; x < NUM_COLUMNS - 1; x++)
                {
                    for (int y = 1; y < NUM_ROWS - 1; y++)
                    {
                        currentBoard[x, y] = nextBoard[x, y];
                    }
                }

                graphics.Show();
            }

            return Task.CompletedTask;
        }

        void InitBoard()
        {
            for (int x = 1; x < NUM_COLUMNS - 1; x++)
            {
                for (int y = 1; y < NUM_ROWS - 1; y++)
                {
                    nextBoard[x, y] = 0;

                    if (x == 0 || x == NUM_COLUMNS - 1 || y == 0 || y == NUM_ROWS - 1)
                    {
                        currentBoard[x, y] = 0;
                    }
                    else
                    {
                        if (rand.Next(2) == 0)
                        {
                            currentBoard[x, y] = 1;
                        }
                        else
                        {
                            currentBoard[x, y] = 0;
                        }
                    }
                }
            }
        }

        void DrawBoard()
        {
            for (int x = 1; x < NUM_COLUMNS - 1; x++)
            {
                for (int y = 1; y < NUM_ROWS - 1; y++)
                {
                    if (currentBoard[x, y] != nextBoard[x, y])
                    {
                        if (nextBoard[x, y] == 1)
                        {
                            graphics.DrawRectangle(x * SIZE, y * SIZE, SIZE, SIZE, Color.LawnGreen, true);
                        }
                        else
                        {
                            graphics.DrawRectangle(x * SIZE, y * SIZE, SIZE, SIZE, Color.Black, true);
                        }
                    }
                }
            }
        }

        void ComputeCA()
        {
            for (int x = 1; x < NUM_COLUMNS - 1; x++)
            {
                for (int y = 1; y < NUM_ROWS - 1; y++)
                {
                    int neighbors = GetNumberOfNeighbors(x, y);
                    if (currentBoard[x, y] == 1 && (neighbors == 2 || neighbors == 3))
                    {
                        nextBoard[x, y] = 1;
                    }
                    else if (currentBoard[x, y] == 1)
                    {
                        nextBoard[x, y] = 0;
                    }

                    if (currentBoard[x, y] == 0 && (neighbors == 3))
                    {
                        nextBoard[x, y] = 1;
                    }
                    else if (currentBoard[x, y] == 0)
                    {
                        nextBoard[x, y] = 0;
                    }
                }
            }
        }

        int GetNumberOfNeighbors(int x, int y)
        {
            return currentBoard[x - 1, y] +
                currentBoard[x - 1, y - 1] +
                currentBoard[x, y - 1] + currentBoard[x + 1, y - 1] +
                currentBoard[x + 1, y] + currentBoard[x + 1, y + 1] +
                currentBoard[x, y + 1] + currentBoard[x - 1, y + 1];
        }
    }
}