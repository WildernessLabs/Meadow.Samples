namespace Span4;

public partial class Span4Game
{
    public enum GameStateType
    {
        Player1Turn,
        Player2Turn,
        Player1Wins,
        Player2Wins,
        Draw,
    };

    enum UserInput
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public byte Width { get; private set; }
    public byte Height { get; private set; }

    public int ChipsToWin { get; private set; }

    public int Player1Wins { get; private set; }
    public int Player2Wins { get; private set; }

    public byte[,] GameField { get; private set; }

    public GameStateType GameState { get; private set; }

    readonly byte player1Value = 1;
    readonly byte player2Value = 2;

    UserInput lastInput;

    public byte CurrentColumn { get; private set; }

    public Span4Game(byte width = 7, byte height = 6, byte chipsToWin = 4)
    {
        Width = width;
        Height = height;
        ChipsToWin = chipsToWin;
        Reset();
    }

    public bool AddChip(byte column)
    {
        if (GameState != GameStateType.Player1Turn &&
            GameState != GameStateType.Player2Turn)
        {
            return false;
        }

        for (int i = 0; i < Height; i++)
        {
            if (GameField[column, i] != 0) { continue; }
            {
                GameField[column, i] = (GameState == GameStateType.Player1Turn) ? player1Value : player2Value;
                UpdateGameState();
                return true;
            }
        }

        return false;
    }

    public void Start()
    {
        Reset();
        UpdateScreen();
    }

    public void Reset()
    {
        GameField = new byte[Width, Height];
        GameState = GameStateType.Player1Turn;
        CurrentColumn = 0;
        lastInput = UserInput.None;
    }

    void Update()
    {
        switch (lastInput)
        {
            case UserInput.Left:
                MoveLeft();
                break;
            case UserInput.Right:
                MoveRight();
                break;
            case UserInput.Down:
                Drop();
                break;
        }

        lastInput = UserInput.None;

        UpdateScreen();
    }

    void UpdateScreen()
    {
        graphics.Clear();
        DrawGame(graphics);
        graphics.Show();
    }

    void UpdateGameState()
    {
        if (GameState == GameStateType.Player1Turn)
        {
            if (DidPlayerWin(player1Value))
            {
                GameState = GameStateType.Player1Wins;
                Player1Wins++;
            }
            else if (IsBoardFilled())
            {
                GameState = GameStateType.Draw;
            }
            else
            {
                GameState = GameStateType.Player2Turn; //next turn
            }
        }
        else if (GameState == GameStateType.Player2Turn)
        {
            if (DidPlayerWin(player2Value))
            {
                GameState = GameStateType.Player2Wins;
                Player2Wins++;
            }
            else if (IsBoardFilled())
            {
                GameState = GameStateType.Draw;
            }
            else
            {
                GameState = GameStateType.Player1Turn; //next turn
            }
        }
        else
        {   //no change - game isn't active
            return;
        }
    }

    bool DidPlayerWin(byte playerValue)
    {
        for (byte i = 0; i < Width; i++)
        {
            for (byte j = 0; j < Height; j++)
            { //could optimize but the code is so simple it shouldn't matter
                if (IsHorizontalWin(playerValue, i, j)) { return true; }
                if (IsVerticalWin(playerValue, i, j)) { return true; }
                if (IsDiagonalUpWin(playerValue, i, j)) { return true; }
                if (IsDiagnonalDownWin(playerValue, i, j)) { return true; }
            }
        }
        return false;
    }

    bool IsHorizontalWin(byte playerValue, byte xStart, byte y)
    {
        if (y >= Height) { return false; }

        for (int i = 0; i < ChipsToWin; i++)
        {
            if (xStart + i >= Width) { return false; }

            if (GameField[i + xStart, y] != playerValue) { return false; }
        }
        return true;
    }

    bool IsVerticalWin(byte playerValue, byte x, byte yStart)
    {
        if (x >= Width) { return false; }

        for (int i = 0; i < ChipsToWin; i++)
        {
            if (yStart + i >= Height) { return false; }

            if (GameField[x, i + yStart] != playerValue) { return false; }
        }
        return true;
    }

    bool IsDiagonalUpWin(byte playerValue, byte xStart, byte yStart)
    {
        if (xStart > Width - ChipsToWin) { return false; }
        if (yStart > Height - ChipsToWin) { return false; }

        for (int i = 0; i < ChipsToWin; i++)
        {
            if (GameField[xStart + i, yStart + i] != playerValue) { return false; }
        }
        return true;
    }

    bool IsDiagnonalDownWin(byte playerValue, byte xStart, byte yStart)
    {
        if (xStart > Width - ChipsToWin) { return false; }
        if (yStart < 3) { return false; }
        if (yStart >= Height) { return false; } //should never happen

        for (int i = 0; i < ChipsToWin; i++)
        {
            if (GameField[xStart + i, yStart - i] != playerValue) { return false; }
        }
        return true;
    }

    bool IsBoardFilled()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (GameField[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Up()
    {
        Reset();
        Update();
    }

    public void Down()
    {
        lastInput = UserInput.Down;
        Update();
    }

    public void Left()
    {
        lastInput = UserInput.Left;
        Update();
    }

    public void Right()
    {
        lastInput = UserInput.Right;
        Update();
    }

    void MoveLeft()
    {
        if (CurrentColumn > 0) { CurrentColumn--; }
    }

    void MoveRight()
    {
        if (CurrentColumn < Width - 1) { CurrentColumn++; }
    }

    void Drop()
    {
        AddChip(CurrentColumn);
    }
}