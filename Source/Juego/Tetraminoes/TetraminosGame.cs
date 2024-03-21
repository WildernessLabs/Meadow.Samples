using Meadow;
using Meadow.Foundation.Audio;
using System;

namespace Tetraminos;

public partial class TetraminosGame
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

    enum UserInput
    {
        None,
        Rotate,
        Down,
        Left,
        Right,
        Drop,
    }

    public Tetramino CurrentPiece { get; protected set; }
    public Tetramino NextPiece { get; protected set; }

    public int Score { get; private set; }
    public int Level { get; private set; }
    public int LinesCleared { get; private set; }

    public int Width { get; private set; } = 8;
    public int Height { get; private set; } = 16;

    public byte[,] GameField { get; private set; }

    readonly Random rand;

    UserInput lastInput = UserInput.None;

    private readonly byte[][] Tetraminos =
    {   //lazy blank piece hack
        new byte[] { 0,0,0,0,
                     0,0,0,0,
                     0,0,0,0,
                     0,0,0,0},
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

    public TetraminosGame(int width = 9, int height = 19)
    {
        Width = width;
        Height = height;

        GameField = new byte[Width, Height];
        rand = new Random();
    }

    public void Reset()
    {
        Score = 0;
        Level = 1;
        LinesCleared = 0;

        for (int x = 0; x < GameField.GetLength(0); x++)
        {
            for (int y = 0; y < GameField.GetLength(1); y++)
            {
                GameField[x, y] = 0;
            }
        }

        CurrentPiece = GetNewPiece();
        NextPiece = GetNewPiece();
        lastInput = UserInput.None;
    }

    Tetramino GetNewPiece()
    {
        byte index = (byte)(rand.Next(6) + 1);

        return new Tetramino()
        {
            X = 2,
            Y = 0,
            Rotation = 0,
            PieceType = index,
        };
    }

    int tick = 0;
    void GameStateUpdate()
    {
        tick++;
        if (tick % (21 - Level) == 0)
        {
            MoveDown(true);
        }

        switch (lastInput)
        {
            case UserInput.Left:
                MoveLeft();
                break;
            case UserInput.Right:
                MoveRight();
                break;
            case UserInput.Rotate:
                Roate();
                break;
            case UserInput.Down:
                MoveDown();
                break;
            case UserInput.Drop:
                DropPiece();
                break;
        }

        lastInput = UserInput.None;
    }

    //game is in portrait so rotate input
    public void Up()
    {
        lastInput = UserInput.Rotate;
    }

    public void Down()
    {
        lastInput = UserInput.Down;
    }

    public void Left()
    {
        lastInput = UserInput.Left;
    }

    public void Right()
    {
        lastInput = UserInput.Right;
    }

    public void Drop()
    {
        lastInput = UserInput.Drop;
    }

    void MoveLeft()
    {
        if (IsPositionValid(CurrentPiece.X - 1,
                           CurrentPiece.Y,
                           CurrentPiece.Rotation,
                           Tetraminos[CurrentPiece.PieceType]) == true)
        {
            CurrentPiece.X += -1;

        }
    }

    void MoveRight()
    {
        if (IsPositionValid(CurrentPiece.X + 1,
                            CurrentPiece.Y,
                            CurrentPiece.Rotation,
                            Tetraminos[CurrentPiece.PieceType]) == true)
        {
            CurrentPiece.X += 1;
        }
    }

    //rotate
    void Roate()
    {
        var rotation = (CurrentPiece.Rotation + 1) % 4;

        if (IsPositionValid(CurrentPiece.X,
                            CurrentPiece.Y,
                            rotation,
                            Tetraminos[CurrentPiece.PieceType]) == true)
        {
            CurrentPiece.Rotate();
            _ = moveAudio.PlayGameSound(GameSoundEffect.MenuNavigate);
        }
    }

    void DropPiece()
    {
        while (MoveDown(false)) ;
        _ = moveAudio.PlayGameSound(GameSoundEffect.MenuNavigate);
    }

    public bool MoveDown(bool setOnFail = false)
    {
        if (IsPositionValid(CurrentPiece.X, CurrentPiece.Y + 1,
                            CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]) == true)
        {
            CurrentPiece.Y += 1;
            return true;
        }
        else if (setOnFail)
        {
            SetPieceToField(CurrentPiece.PieceType);
            CheckForCompletedLines(CurrentPiece.Y);
            CurrentPiece = NextPiece;
            NextPiece = GetNewPiece();

            //check for endgame state
            if (IsPositionValid(CurrentPiece.X,
                                CurrentPiece.Y,
                                CurrentPiece.Rotation,
                                Tetraminos[CurrentPiece.PieceType]) == false)
            {
                Resolver.Log.Info($"Game over: {LinesCleared} lines cleared");
                Reset(); //start a new game
            }
        }
        return false; //for drop function ... should improve
    }

    void SetPieceToField(byte pieceType)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (IsPieceLocationSet(i, j, CurrentPiece))
                {
                    GameField[CurrentPiece.X + i, CurrentPiece.Y + j] = pieceType;
                }
            }
        }
    }

    void CheckForCompletedLines(int yPos)
    {
        bool complete = false;
        for (int j = 0; j < 4; j++)
        {
            complete = true;
            for (int i = 0; i < Width; i++)
            {
                if (IsGameFieldSet(i, j + yPos) == false)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                ClearLine(j + yPos); //we're moving down so this is valid
            }
        }

        if (complete)
        {
            _ = effectsAudio.PlayGameSound(GameSoundEffect.Victory);
        }
    }

    void ClearLine(int yPos)
    {
        Resolver.Log.Info("ClearLine");
        LinesCleared++;

        if (LinesCleared % 10 == 0)
        {
            Level++;
        }

        for (int j = yPos; j > 0; j--)
        {
            for (int i = 0; i < Width; i++)
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (IsPieceLocationSet(i, j, rotation, pieceData))
                {
                    if (x + i < 0 || x + i >= Width || y + j >= Height) //x bounds checking
                    {
                        return false;
                    }
                    if (IsGameFieldSet(x + i, y + j) == true)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public byte GetGameFieldValue(int x, int y)
    {
        return GameField[x, y];
    }

    public bool IsGameFieldSet(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return false; //we'll state out of bounds positions as set (i.e. not free)
        }

        return GameField[x, y] != 0;
    }

    //relative to tetramino, not game field
    public bool IsPieceLocationSet(int x, int y, Tetramino piece)
    {
        return IsPieceLocationSet(x, y, piece.Rotation, Tetraminos[piece.PieceType]);
    }

    //relative to tetramino, not game field
    public bool IsPieceLocationSet(int x, int y, int rotation, byte[] pieceData)
    {
        if (x < 0 || x > 3 || y < 0 || y > 3)
        {
            return false;
        }

        return (rotation % 4) switch
        {
            0 => pieceData[y * 4 + x] == 1,
            1 => pieceData[12 + y - (x * 4)] == 1,
            2 => pieceData[15 - (y * 4) - x] == 1,
            _ => pieceData[3 - y + (x * 4)] == 1,
        };
    }
}