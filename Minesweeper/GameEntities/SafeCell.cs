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

        public int posX { get; set; }
        public int posY { get; set;  }

        public SafeCell()
        {
            Value = 0;
            State = CellState.Hidden;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            
        }

        public bool Reveal()
        {
            State = CellState.Revealed;
            return true;
        }

        
    }
}
