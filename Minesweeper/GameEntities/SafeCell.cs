using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    class SafeCell : ICell
    {
        private const int CELL_SIZE = 15;

        public int Value { get; set; }
        public CellState State { get; set; }

        public int posX { get; set; }
        public int posY { get; set;  }

        public SafeCell()
        {
            Value = 0;
            State = CellState.Hidden;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rect, Texture2D spriteSheet)
        {
            
            if (State == CellState.Hidden)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(0, 0, CELL_SIZE, CELL_SIZE), Color.White);
            else if (State == CellState.Revealed)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(0, 656 - (CELL_SIZE + 1) * Value, CELL_SIZE, CELL_SIZE), Color.White);
            else if (State == CellState.Pressed)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(0, 656, CELL_SIZE, CELL_SIZE), Color.White);
            else if (State == CellState.Flagged)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(128, 624, CELL_SIZE, CELL_SIZE), Color.White);
            else if (State == CellState.WrongFlag)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(0, 256, CELL_SIZE, CELL_SIZE), Color.White);

            //spriteBatch.Draw(spriteSheet, position, new Rectangle(0, 656, 15, 15), Color.White);

        }

        public bool Reveal()
        {
            State = CellState.Revealed;
            return true;
        }

        
    }
}
