using System;

namespace Tetris
{
    public class TetrisGame
    {
        public class Tetramino
        {
            public int X { get; set; }
            public int Y { get; set; }
            public byte Rotation { get; set; }
            public byte PieceType { get; set; }

            public void Rotate()
            {
                Rotation = (byte)((Rotation += 1) % 4);
            }
        }
        
        public Tetramino CurrentPiece { get; protected set; }

        public int Score { get; private set; }
        public int Level { get; private set; }
        public int LinesCleared { get; private set; }

        public int Width { get; private set; } = 8;
        public int Height { get; private set; } = 16;

        public byte[,] GameField { get; private set; }

        Random rand;

        private byte[][] Tetraminos =
        {
            new byte[] { 0,0,1,0,
                         0,1,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,0,1,0,
                         0,0,1,0,
                         0,0,1,0,
                         0,0,1,0},
            new byte[] { 0,0,0,0,
                         0,1,1,0,
                         0,1,1,0,
                         0,0,0,0},
            new byte[] { 0,1,1,0,
                         0,0,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,1,1,0,
                         0,1,0,0,
                         0,1,0,0,
                         0,0,0,0},
            new byte[] { 0,1,0,0,
                         0,1,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,0,1,0,
                         0,1,1,0,
                         0,1,0,0,
                         0,0,0,0},
        };

        public TetrisGame(int width = 8, int height = 20)
        {
            Width = width;
            Height = height;

            Init();
            Reset();
        }

        void Init()
        {
            GameField = new byte[Width, Height];
            rand = new Random();
        }

        public void Reset ()
        {
            Score = 0;
            Level = 1;
            LinesCleared = 0;

            for(int x = 0; x < GameField.GetLength(0); x++)
            {
                for(int y = 0; y < GameField.GetLength(1); y++)
                {
                    GameField[x, y] = 0;
                }
            }

            CurrentPiece = GetNewPiece();
        }

        Tetramino GetNewPiece()
        {
            byte index = (byte)rand.Next(6);

            return new Tetramino()
            {
                X = 2,
                Y = 0,
                Rotation = 0,
                PieceType = index,
            };
        }

        public void OnLeft()
        {
            if(IsPositionValid(CurrentPiece.X - 1,
                               CurrentPiece.Y, 
                               CurrentPiece.Rotation,
                               Tetraminos[CurrentPiece.PieceType]) == true)
            {
                CurrentPiece.X += -1;
            }
        }

        public void OnRight()
        {
            if (IsPositionValid(CurrentPiece.X + 1,
                                CurrentPiece.Y,
                                CurrentPiece.Rotation,
                                Tetraminos[CurrentPiece.PieceType]) == true)
            {
                CurrentPiece.X += 1;
            }
        }

        public void OnRotate()
        {
            var rotation = (CurrentPiece.Rotation + 1) % 4;

            if (IsPositionValid(CurrentPiece.X,
                                CurrentPiece.Y,
                                rotation,
                                Tetraminos[CurrentPiece.PieceType]) == true)
            {
                CurrentPiece.Rotate();
            }
        }

        public void OnDown(bool setOnFail = false)
        {
            if (IsPositionValid(CurrentPiece.X, CurrentPiece.Y + 1,
                                CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]) == true)
            {
                CurrentPiece.Y += 1;
            }
            else if(setOnFail)
            {
                SetPieceToField();
                CheckForCompletedLines(CurrentPiece.Y);
                CurrentPiece = GetNewPiece();

                //check for endgame state
                if (IsPositionValid(CurrentPiece.X,
                                    CurrentPiece.Y,
                                    CurrentPiece.Rotation,
                                    Tetraminos[CurrentPiece.PieceType]) == false)
                {
                    Console.WriteLine($"Game over: {LinesCleared} lines cleared");
                    Reset(); //start a new game
                }
            }
        }

        void SetPieceToField()
        {
            for(int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if(IsPieceLocationSet(i, j)) {   
                        GameField[CurrentPiece.X + i, CurrentPiece.Y + j] = 1; 
                    }
                }
            }
        }

        void CheckForCompletedLines(int yPos)
        {
            Console.WriteLine("Check for completed lines");
            bool complete;
            for(int j = 0; j < 4; j++)
            {
                complete = true;
                for(int i = 0; i < Width; i++)
                {
                    if(IsGameFieldSet(i, j + yPos) == false)
                    {
                        complete = false;
                        break;
                    }
                }
                if(complete)
                {
                    ClearLine(j + yPos); //we're moving down so this is valid
                }
            }
        }

        void ClearLine(int yPos)
        {
            Console.WriteLine("ClearLine");
            LinesCleared++;

            if(LinesCleared % 10 == 0)
            {
                Level++;
            }

            for(int j = yPos; j > 0; j--)
            {
                for(int i = 0; i < Width; i++)
                {   //should switch to an array of arrays so we can just assign vs copy 
                    GameField[i, j] = GameField[i, j - 1];
                }
            }

            //and clear the top line
            for (int i = 0; i < Width; i++)
            {
                GameField[i, 0] = 0;
            }
        }

        bool IsPositionValid(int x, int y, int rotation, byte[] pieceData)
        {
            //loop over every point in the tetramino data for the current piece
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(IsPieceLocationSet(i, j, rotation, pieceData))
                    {
                        if(x + i < 0 || x + i >= Width || y + j >= Height) //x bounds checking
                        {
                            return false;
                        }
                        if(IsGameFieldSet(x + i, y + j) == true)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool IsGameFieldSet(int x, int y)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false; //we'll state out of bounds positions as set (i.e. not free)
            }

            return GameField[x, y] != 0;
        }

        //relative to tetramino, not game field
        public bool IsPieceLocationSet(int x, int y)
        {
            return IsPieceLocationSet(x, y, CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]);
        }


        //relative to tetramino, not game field
        public bool IsPieceLocationSet(int x, int y, int rotation, byte[] pieceData)
        {
            if (x < 0 || x > 3 || y < 0 || y > 3)
            {
                return false;
            }

            switch (rotation % 4)
            {
                case 0: return pieceData[y * 4 + x] == 1;
                case 1: return pieceData[12 + y - (x * 4)] == 1;
                case 2: return pieceData[15 - (y * 4) - x] == 1;
                case 3:
                default:
                    return pieceData[3 - y + (x * 4)] == 1;
            }
        }
    }
}