using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    class BombCell : ICell
    {
        public int Value { get; set; }
        public CellState State { get; set; }

        public int posX { get; set; }
        public int posY { get; set; }

        public BombCell()
        {
            Value = -1;
            State = CellState.Hidden;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteFont font)
        {
            if (State == CellState.Hidden)
                spriteBatch.DrawString(font, Value.ToString(), position, Color.Gray);
            else if (State == CellState.Revealed)
                spriteBatch.DrawString(font, Value.ToString(), position, Color.Black);
            else if (State == CellState.Pressed)
                spriteBatch.DrawString(font, Value.ToString(), position, Color.Green);
            else if (State == CellState.Flagged)
                spriteBatch.DrawString(font, Value.ToString(), position, Color.Red);
        }

        public bool Reveal()
        {
            State = CellState.Revealed;
            return false;
        }
    }
}
