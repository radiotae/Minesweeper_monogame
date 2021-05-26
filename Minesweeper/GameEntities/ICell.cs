using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    interface ICell
    {
        //The Value of a Cell will be the number of bombs around the Cell OR -1 if the Cell is a bomb.
        public int Value { get; set; }

        public int posX { get; set; }
        public int posY { get; set; }

        public CellState State { get; set; }

        void Draw(SpriteBatch spriteSheet, Vector2 position);

        //Will return false if it is a bomb, true for otherwise.
        bool Reveal();
    }
}
