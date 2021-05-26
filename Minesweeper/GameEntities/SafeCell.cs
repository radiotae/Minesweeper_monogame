using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    class SafeCell : ICell
    {
        public int Value { get; set; }
        public CellState State { get; set; }

        public SafeCell()
        {
            Value = 0;
        }

        public void Draw(SpriteBatch spriteSheet, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public bool Reveal()
        {
            throw new NotImplementedException();
        }

        
    }
}
