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

        public void Draw(SpriteBatch spriteSheet, Vector2 position)
        {

        }

        public bool Reveal()
        {
            State = CellState.Revealed;
            return false;
        }
    }
}
