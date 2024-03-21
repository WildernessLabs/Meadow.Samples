using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using System;
using System.Threading;

namespace Tetraminos;

public partial class TetraminosGame
{
    MicroAudio moveAudio;
    MicroAudio effectsAudio;
    MicroGraphics graphics;

    int blockSize;
    int topIndent;
    int leftIndent;

    public void Init(MicroGraphics graphics, MicroAudio moveAudio, MicroAudio effectsAudio)
    {
        this.moveAudio = moveAudio;
        this.effectsAudio = effectsAudio;
        this.graphics = graphics;

        graphics.Clear();
        graphics.DrawText(2, 0, "Meadow Tetraminoes");
        graphics.DrawText(2, 20, "v0.3.0");
        graphics.Show();

        leftIndent = 2;
        topIndent = 2;

        graphics.CurrentFont = new Font12x16();

        //little hacky but works out nicely for the low res displays
        blockSize = (graphics.Height - topIndent - 4) / 19;

        Thread.Sleep(1000);
    }

    public void Update()
    {
        GameStateUpdate();
        graphics.Clear();
        DrawGameField(graphics);
        DrawPreview(graphics);
        graphics.Show();

        Thread.Sleep(Math.Max(50 - Level, 0));
    }

    //ToDo - scale
    void DrawPreview(MicroGraphics graphics)
    {
        //draw next piece
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (IsPieceLocationSet(i, j, NextPiece))
                {
                    graphics.DrawRectangle(i * 8 + 160, 40 + j * 8, 8, 8);
                }
            }
        }
    }

    void DrawGameField(MicroGraphics graphics)
    {
        int xIndent = 3;
        int yIndent = topIndent + 2;

        graphics.DrawText(160, 0, $"Lines: {LinesCleared}");

        graphics.DrawRectangle(leftIndent,
            topIndent,
            4 + blockSize * 9,
            4 + 19 * blockSize);

        //draw current piece
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (IsPieceLocationSet(i, j, CurrentPiece))
                {
                    graphics.DrawRectangle((CurrentPiece.X + i) * blockSize + xIndent,
                        (CurrentPiece.Y + j) * blockSize + yIndent,
                        blockSize, blockSize, GetColorForPiece(CurrentPiece.PieceType), true);
                }
            }
        }

        //draw gamefield
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (IsGameFieldSet(i, j))
                {
                    graphics.DrawRectangle((i) * blockSize + xIndent,
                        (j) * blockSize + yIndent,
                        blockSize, blockSize, GetColorForPiece(GetGameFieldValue(i, j)), true);
                }
            }
        }
    }

    Color GetColorForPiece(byte pieceType)
    {
        return pieceType switch
        {
            1 => Color.Blue,
            2 => Color.Orange,
            3 => Color.Yellow,
            4 => Color.Green,
            5 => Color.Purple,
            6 => Color.Red,
            _ => Color.White,
        };
    }
}